namespace BunsenBurner;

public static partial class BddSyntax
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
    public static ScenarioBuilder.Acted<TData, TResult> When<TData, TResult>(
        this ScenarioBuilder.Arranged<TData> scenario,
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
    public static ScenarioBuilder.Acted<TData, TResult> When<TData, TResult>(
        this ScenarioBuilder.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) => scenario.Act(fn);
}
