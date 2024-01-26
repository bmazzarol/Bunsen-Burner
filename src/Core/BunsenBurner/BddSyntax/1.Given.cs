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
    public static ScenarioBuilder.Arranged<TData> Given<TData>(TData data) =>
        Shared.Arrange<TData, Syntax.Bdd>(data);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static ScenarioBuilder.Arranged<TData> Given<TData>(
        this string name,
        Func<Task<TData>> fn
    ) => name.Arrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static ScenarioBuilder.Arranged<TData> Given<TData>(this string name, Func<TData> fn) =>
        name.Arrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="data">scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static ScenarioBuilder.Arranged<TData> Given<TData>(this string name, TData data) =>
        name.Arrange<TData, Syntax.Bdd>(data);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="data">scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static ScenarioBuilder.Arranged<TData> GivenData<TData>(this TData data) =>
        Shared.Arrange<TData, Syntax.Bdd>(data);

    /// <summary>
    /// Allows for additional given steps
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async function transforming scenario data into scenario data</param>
    /// <typeparam name="TData">initial scenario data</typeparam>
    /// <typeparam name="TDataNext">next scenario data</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static ScenarioBuilder.Arranged<TDataNext> And<TData, TDataNext>(
        this ScenarioBuilder.Arranged<TData> scenario,
        Func<TData, Task<TDataNext>> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Allows for additional given steps
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function transforming test data into test data</param>
    /// <typeparam name="TData">initial scenario data</typeparam>
    /// <typeparam name="TDataNext">next scenario data</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static ScenarioBuilder.Arranged<TDataNext> And<TData, TDataNext>(
        this ScenarioBuilder.Arranged<TData> scenario,
        Func<TData, TDataNext> fn
    ) => Shared.And(scenario, fn);
}
