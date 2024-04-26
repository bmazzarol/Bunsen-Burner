namespace BunsenBurner;

/// <summary>
/// DSL for building tests using a <see cref="Syntax.Bdd"/>
/// </summary>
public static partial class BddSyntax
{
    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static ScenarioBuilder.Arranged<TData> Given<TData>(Func<Task<TData>> fn) =>
        Shared.Arrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="fn">function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static ScenarioBuilder.Arranged<TData> Given<TData>(Func<TData> fn) =>
        Shared.Arrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="data">scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static ScenarioBuilder.Arranged<TData> Given<TData>(this TData data) =>
        Shared.Arrange<TData, Syntax.Bdd>(data);
}
