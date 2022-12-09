namespace BunsenBurner;

/// <summary>
/// Shared builder function implementations for act steps
/// </summary>
internal static partial class Shared
{
    /// <summary>
    /// Acts on a scenario
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">act function</param>
    /// <typeparam name="TData">context data</typeparam>
    /// <typeparam name="TResult">result data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) where TSyntax : struct, Syntax =>
        new(scenario.Name, scenario.ArrangeScenario, fn, scenario.Disposables);

    /// <summary>
    /// Acts on a scenario
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">act function</param>
    /// <typeparam name="TData">context data</typeparam>
    /// <typeparam name="TResult">result data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Acted<TData, TResult> Act<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) where TSyntax : struct, Syntax => scenario.Act(x => Task.FromResult(fn(x)));

    /// <summary>
    /// Acts again on a scenario
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">act function</param>
    /// <typeparam name="TData">context data</typeparam>
    /// <typeparam name="TResult">result data</typeparam>
    /// <typeparam name="TResultNext">next result data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Acted<TData, TResultNext> And<
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
                var result = await scenario.ActOnScenario(data);
                var nextResult = await fn(data, result);
                return nextResult;
            },
            scenario.Disposables
        );

    /// <summary>
    /// Acts again on a scenario
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">act function</param>
    /// <typeparam name="TData">context data</typeparam>
    /// <typeparam name="TResult">result data</typeparam>
    /// <typeparam name="TResultNext">next result data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Acted<TData, TResultNext> And<
        TData,
        TResult,
        TResultNext,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TData, TResult> scenario, Func<TData, TResult, TResultNext> fn)
        where TSyntax : struct, Syntax => scenario.And((d, r) => Task.FromResult(fn(d, r)));

    /// <summary>
    /// Resets the acted scenario back to arranged, throwing away the act information
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TData> ResetToArranged<TData, TResult, TSyntax>(
        this Scenario<TSyntax>.Acted<TData, TResult> scenario
    ) where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(scenario.ArrangeScenario);
}
