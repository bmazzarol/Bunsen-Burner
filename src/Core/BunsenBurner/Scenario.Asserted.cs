namespace BunsenBurner;

public abstract partial record Scenario<TSyntax>
{
    /// <summary>
    /// A <see cref="Scenario{T}"/> that has been arranged and acted and asserted against
    /// </summary>
    /// <typeparam name="TData">type of data required to act on the <see cref="Scenario{T}"/></typeparam>
    /// <typeparam name="TResult">type of data returned from the result of acting</typeparam>
    public sealed record Asserted<TData, TResult> : Scenario<TSyntax>
    {
        private readonly Func<Task<TData>> _arrangeScenario;
        private readonly Func<TData, Task<TResult>> _actOnScenario;

        internal Asserted(
            string? name,
            Func<Task<TData>> arrangeScenario,
            Func<TData, Task<TResult>> actOnScenario,
            Func<TData, TResult, Task> assertAgainstResult,
            HashSet<object> disposables
        )
            : base(name, disposables)
        {
            _arrangeScenario = arrangeScenario;
            _actOnScenario = actOnScenario;
            AssertAgainstResult = assertAgainstResult;
        }

        internal Func<Task<TData>> ArrangeScenario =>
            async () =>
            {
                var result = await _arrangeScenario();
                TrackPotentialDisposal(result);
                return result;
            };

        internal Func<TData, Task<TResult>> ActOnScenario =>
            async x =>
            {
                var result = await _actOnScenario(x);
                TrackPotentialDisposal(result);
                return result;
            };

        internal Func<TData, TResult, Task> AssertAgainstResult { get; }

        internal async Task Run()
        {
            try
            {
                var data = await ArrangeScenario();
                var result = await ActOnScenario(data);
                await AssertAgainstResult(data, result);
            }
            finally
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
}
