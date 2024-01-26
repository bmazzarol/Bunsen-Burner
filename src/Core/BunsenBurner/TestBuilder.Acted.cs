namespace BunsenBurner;

public abstract partial record TestBuilder<TSyntax>
{
    /// <summary>
    /// A <see cref="TestBuilder{TSyntax}"/> that has been arranged and acted on ready for assertions
    /// </summary>
    /// <typeparam name="TData">type of `data` required to act on the <see cref="TestBuilder{TSyntax}"/></typeparam>
    /// <typeparam name="TResult">`result` of acting</typeparam>
    public sealed record Acted<TData, TResult> : TestBuilder<TSyntax>
    {
        private readonly Func<Task<TData>> _arrangeStep;
        private readonly Func<TData, Task<TResult>> _actStep;

        internal Acted(
            string? name,
            Func<Task<TData>> arrangeStep,
            Func<TData, Task<TResult>> actStep,
            HashSet<object> disposables
        )
            : base(name, disposables)
        {
            _arrangeStep = arrangeStep;
            _actStep = actStep;
        }

        internal Func<Task<TData>> ArrangeStep =>
            async () =>
            {
                var result = await _arrangeStep();
                TrackPotentialDisposal(result);
                return result;
            };

        internal Func<TData, Task<TResult>> ActStep =>
            async data =>
            {
                var result = await _actStep(data);
                TrackPotentialDisposal(result);
                return result;
            };
    }
}
