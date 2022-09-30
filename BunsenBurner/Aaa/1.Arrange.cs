namespace BunsenBurner;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static partial class Aaa
{
    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="fn">async function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> Arrange<TData>(Func<Task<TData>> fn) =>
        Shared.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="fn">function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> Arrange<TData>(Func<TData> fn) =>
        Shared.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">async function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> Arrange<TData>(
        this string name,
        Func<Task<TData>> fn
    ) => name.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> Arrange<TData>(this string name, Func<TData> fn) =>
        name.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Allows for additional arranging of test data
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async function transforming test data into test data</param>
    /// <typeparam name="TData">initial data required to act on the test</typeparam>
    /// <typeparam name="TDataNext">next data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TDataNext> And<TData, TDataNext>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, Task<TDataNext>> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Allows for additional arranging of test data
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function transforming test data into test data</param>
    /// <typeparam name="TData">initial data required to act on the test</typeparam>
    /// <typeparam name="TDataNext">next data required to act on the test</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TDataNext> And<TData, TDataNext>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, TDataNext> fn
    ) => Shared.And(scenario, fn);
}
