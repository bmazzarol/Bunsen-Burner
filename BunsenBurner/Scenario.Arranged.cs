namespace BunsenBurner;

public abstract partial record Scenario<TSyntax>
{
    /// <summary>
    /// A scenario that has been arranged and is ready to act on
    /// </summary>
    /// <typeparam name="TData">type of data required to act on the scenario</typeparam>
    public sealed record Arranged<TData> : Scenario<TSyntax>
    {
        private readonly Func<Task<TData>> _arrangeScenario;

        internal Arranged(
            string? name,
            Func<Task<TData>> arrangeScenario,
            HashSet<IDisposable> disposables
        ) : base(name, disposables) => _arrangeScenario = arrangeScenario;

        internal Func<Task<TData>> ArrangeScenario =>
            async () =>
            {
                var result = await _arrangeScenario();
                TrackPotentialDisposal(result);
                return result;
            };
    }
}
