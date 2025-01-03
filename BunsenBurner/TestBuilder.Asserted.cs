using System.Linq.Expressions;
using BunsenBurner.Extensions;

namespace BunsenBurner;

public abstract partial record TestBuilder<TSyntax>
{
    /// <summary>
    /// A <see cref="TestBuilder{TSyntax}"/> that has been arranged and acted and asserted against.
    /// This is a complete test and can be run
    /// </summary>
    /// <typeparam name="TData">type of `data` required to act on the <see cref="TestBuilder{TSyntax}"/></typeparam>
    /// <typeparam name="TResult">`result` of acting</typeparam>
    public sealed record Asserted<TData, TResult> : TestBuilder<TSyntax>
    {
        private readonly Func<Task<TData>> _arrangeStep;
        private readonly Func<TData, Task<TResult>> _actStep;
        private readonly List<Func<TData, TResult, Task>> _assertions;

        internal Asserted(
            Func<Task<TData>> arrangeStep,
            Func<TData, Task<TResult>> actStep,
            Func<TData, TResult, Task> assertStep
        )
        {
            _arrangeStep = arrangeStep;
            _actStep = actStep;
            _assertions = [assertStep];
        }

        /// <summary>
        /// Arrange step
        /// </summary>
        public Func<Task<TData>> ArrangeStep =>
            async () =>
            {
                var result = await _arrangeStep();
                TrackPotentialDisposal(result);
                return result;
            };

        /// <summary>
        /// Act step
        /// </summary>
        public Func<TData, Task<TResult>> ActStep =>
            async x =>
            {
                var result = await _actStep(x);
                TrackPotentialDisposal(result);
                return result;
            };

        /// <summary>
        /// Assert step
        /// </summary>
        public Func<TData, TResult, Task> AssertStep =>
            (data, result) =>
                Task.WhenAll(
                        _assertions.Select(assertion => Task.Run(() => assertion(data, result)))
                    )
                    .ContinueWith(
                        continuationFunction: x =>
                            x.IsFaulted
                            && x.Exception is { } ex
                            && (
                                ex.InnerExceptions.Count > 1
                                || ex.InnerException is AggregateException
                            )
                                ? Task.FromException(ex.Flatten())
                                : x,
                        cancellationToken: CancellationToken.None,
                        TaskContinuationOptions.ExecuteSynchronously,
                        scheduler: TaskScheduler.Default
                    )
                    .Unwrap();

        /// <summary>
        /// Allows for additional asserting of test data
        /// </summary>
        /// <param name="fn">async function asserting test data</param>
        /// <returns>asserted test</returns>
        [Pure]
        public Asserted<TData, TResult> And(Func<TData, TResult, Task> fn)
        {
            _assertions.Add(fn);
            return this;
        }

        /// <summary>
        /// Allows for additional asserting of test data
        /// </summary>
        /// <param name="fn">async function asserting test data</param>
        /// <returns>asserted test</returns>
        [Pure]
        public Asserted<TData, TResult> And(Func<TResult, Task> fn) => And((_, r) => fn(r));

        /// <summary>
        /// Allows for additional asserting of test data
        /// </summary>
        /// <param name="fn">function asserting test data</param>
        /// <returns>asserted test</returns>
        [Pure]
        public Asserted<TData, TResult> And(Action<TData, TResult> fn) =>
            And(
                (d, r) =>
                {
                    fn(d, r);
                    return Task.CompletedTask;
                }
            );

        /// <summary>
        /// Allows for additional asserting of test data
        /// </summary>
        /// <param name="fn">function asserting test data</param>
        /// <returns>asserted test</returns>
        [Pure]
        public Asserted<TData, TResult> And(Action<TResult> fn) => And((_, r) => fn(r));

        /// <summary>
        /// Allows for additional asserting of test data
        /// </summary>
        /// <param name="expression">expression to assert against</param>
        /// <returns>asserted test</returns>
        [Pure]
        public Asserted<TData, TResult> And(Expression<Func<TResult, bool>> expression) =>
            And(expression.RunExpressionAssertion);

        /// <summary>
        /// Allows for additional asserting of test data
        /// </summary>
        /// <param name="expression">expression to assert against</param>
        /// <returns>asserted test</returns>
        [Pure]
        public Asserted<TData, TResult> And(Expression<Func<TData, TResult, bool>> expression) =>
            And(expression.RunExpressionAssertion);

        /// <summary>
        /// Disables auto disposal of captured disposables
        /// </summary>
        /// <returns>an asserted test</returns>
        [Pure]
        public Asserted<TData, TResult> NoDisposal() => this with { AutoDispose = false };

        /// <summary>
        /// Flag to indicate if the <see cref="TestBuilder{TSyntax}"/> should auto-dispose captured disposables
        /// </summary>
        public bool AutoDispose { get; init; } = true;

        /// <summary>
        /// Runs the <see cref="TestBuilder{TSyntax}"/> definition of a test
        /// </summary>
        public async Task Run()
        {
            try
            {
                var data = await ArrangeStep();
                var result = await ActStep(data);
                await AssertStep(data, result);
            }
            finally
            {
                if (AutoDispose && _disposables is not null)
                {
                    foreach (var disposable in _disposables)
                    {
                        switch (disposable)
                        {
                            case IAsyncDisposable ad:
                                await ad.DisposeAsync();
                                break;
                            case IDisposable d:
                                d.Dispose();
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Awaiter for a test so it can be run
        /// </summary>
        /// <returns>awaiter</returns>
        public TaskAwaiter GetAwaiter() => Run().GetAwaiter();

        /// <summary>
        /// Implicit conversion to a task so a test can be run
        /// </summary>
        /// <param name="asserted">asserted test</param>
        /// <returns>task</returns>
        public static implicit operator Task(Asserted<TData, TResult> asserted) => asserted.Run();
    }
}
