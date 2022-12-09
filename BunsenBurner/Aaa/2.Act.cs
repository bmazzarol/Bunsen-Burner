namespace BunsenBurner;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static partial class Aaa
{
    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResult> Act<TData, TResult>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) => Shared.Act(scenario, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">result of acting on the test data</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResult> Act<TData, TResult>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) => Shared.Act(scenario, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">initial result of acting on the test data</typeparam>
    /// <typeparam name="TResultNext">next result of acting on the test data and last result</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task<TResultNext>> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Acts on the test data and returns a result to assert against
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">act function</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">initial result of acting on the test data</typeparam>
    /// <typeparam name="TResultNext">next result of acting on the test data and last result</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this AaaScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, TResultNext> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Resets the acted scenario back to arranged, throwing away the act information
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TData">test data</typeparam>
    /// <typeparam name="TResult">test result</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> ResetToArranged<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario
    ) => Shared.ResetToArranged(scenario);
}
