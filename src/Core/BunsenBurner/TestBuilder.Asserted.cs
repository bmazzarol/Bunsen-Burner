using System.Linq.Expressions;

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

        internal Asserted(
            Func<Task<TData>> arrangeStep,
            Func<TData, Task<TResult>> actStep,
            Func<TData, TResult, Task> assertStep,
            HashSet<object> disposables
        )
            : base(disposables)
        {
            _arrangeStep = arrangeStep;
            _actStep = actStep;
            AssertStep = assertStep;
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
        public Func<TData, TResult, Task> AssertStep { get; }

        /// <summary>
        /// Allows for additional asserting of test data
        /// </summary>
        /// <param name="fn">async function asserting test data</param>
        /// <returns>asserted test</returns>
        [Pure]
        public Asserted<TData, TResult> And(Func<TData, TResult, Task> fn) =>
            new(
                ArrangeStep,
                ActStep,
                async (data, result) =>
                {
                    await AssertStep(data, result);
                    await fn(data, result);
                },
                Disposables
            );

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
            And(r => r.RunExpressionAssertion(expression));

        /// <summary>
        /// Allows for additional asserting of test data
        /// </summary>
        /// <param name="expression">expression to assert against</param>
        /// <returns>asserted test</returns>
        [Pure]
        public Asserted<TData, TResult> And(Expression<Func<TData, TResult, bool>> expression) =>
            And((d, r) => d.RunExpressionAssertion(r, expression));

        /// <summary>
        /// Disables auto disposal of captured disposables
        /// </summary>
        /// <returns>an asserted test</returns>
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
                if (AutoDispose)
                {
                    foreach (var disposable in Disposables)
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
    }
}
