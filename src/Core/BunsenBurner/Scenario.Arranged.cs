namespace BunsenBurner;

public abstract partial record Scenario<TSyntax>
{
    /// <summary>
    /// A <see cref="Scenario{T}"/> that has been arranged and is ready to act on
    /// </summary>
    /// <typeparam name="TData">type of data required to act on the <see cref="Scenario{T}"/></typeparam>
    public sealed record Arranged<TData> : Scenario<TSyntax>
    {
        private readonly Func<Task<TData>> _arrangeScenario;

        internal Arranged(
            string? name,
            Func<Task<TData>> arrangeScenario,
            HashSet<object> disposables
        )
            : base(name, disposables)
        {
            _arrangeScenario = arrangeScenario;
        }

        internal Func<Task<TData>> ArrangeScenario =>
            async () =>
            {
                var data = await _arrangeScenario();
                TrackPotentialDisposal(data);
                return data;
            };
    }
}
