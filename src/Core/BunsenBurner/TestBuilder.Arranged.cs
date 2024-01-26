namespace BunsenBurner;

public abstract partial record TestBuilder<TSyntax>
{
    /// <summary>
    /// A <see cref="TestBuilder{TSyntax}"/> that has been arranged and is ready to act on
    /// </summary>
    /// <typeparam name="TData">type of `data` required to act on the <see cref="TestBuilder{TSyntax}"/></typeparam>
    public sealed record Arranged<TData> : TestBuilder<TSyntax>
    {
        private readonly Func<Task<TData>> _arrangeStep;

        internal Arranged(string? name, Func<Task<TData>> arrangeStep, HashSet<object> disposables)
            : base(name, disposables) => _arrangeStep = arrangeStep;

        internal Func<Task<TData>> ArrangeStep =>
            async () =>
            {
                var data = await _arrangeStep();
                TrackPotentialDisposal(data);
                return data;
            };
    }
}
