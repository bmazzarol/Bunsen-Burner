﻿namespace BunsenBurner;

public abstract partial record Scenario<TSyntax>
{
    /// <summary>
    /// A scenario that has been arranged and acted on
    /// </summary>
    /// <typeparam name="TData">type of data required to act on the scenario</typeparam>
    /// <typeparam name="TResult">type of data returned from the result of acting</typeparam>
    public sealed record Acted<TData, TResult> : Scenario<TSyntax>
    {
        private readonly Func<Task<TData>> _arrangeScenario;
        private readonly Func<TData, Task<TResult>> _actOnScenario;

        internal Acted(
            string? name,
            Func<Task<TData>> arrangeScenario,
            Func<TData, Task<TResult>> actOnScenario,
            HashSet<IDisposable> disposables
        )
            : base(name, disposables)
        {
            _arrangeScenario = arrangeScenario;
            _actOnScenario = actOnScenario;
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
    }
}
