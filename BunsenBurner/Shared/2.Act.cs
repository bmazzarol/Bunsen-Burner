namespace BunsenBurner;

/// <summary>
/// Shared builder function implementations for act steps
/// </summary>
internal static partial class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) where TSyntax : struct, Syntax => new(scenario.Name, scenario.ArrangeScenario, fn);

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) where TSyntax : struct, Syntax => scenario.Act(x => Task.FromResult(fn(x)));

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResultNext> And<
        TData,
        TResult,
        TResultNext,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task<TResultNext>> fn
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data =>
            {
                var result = await scenario.ActOnScenario(data).ConfigureAwait(false);
                var nextResult = await fn(data, result).ConfigureAwait(false);
                return nextResult;
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResultNext> And<
        TData,
        TResult,
        TResultNext,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Func<TData, TResult, TResultNext> fn)
        where TSyntax : struct, Syntax => scenario.And((d, r) => Task.FromResult(fn(d, r)));
}
