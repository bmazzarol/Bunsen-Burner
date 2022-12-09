namespace BunsenBurner;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static partial class Bdd
{
    /// <summary>
    /// When the scenario is run
    /// </summary>
    /// <param name="scenario">ready to run scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResult> When<TData, TResult>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) => scenario.Act(fn);

    /// <summary>
    /// When the scenario is run
    /// </summary>
    /// <param name="scenario">ready to run scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResult> When<TData, TResult>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) => scenario.Act(fn);

    /// <summary>
    /// Allows for additional when steps
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">initial result of running the scenario</typeparam>
    /// <typeparam name="TResultNext">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this BddScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task<TResultNext>> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Allows for additional when steps
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">initial result of running the scenario</typeparam>
    /// <typeparam name="TResultNext">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this BddScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, TResultNext> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Resets the completed scenario back to given, throwing away the when information
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> ResetToGiven<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario
    ) => scenario.ResetToArranged();
}
