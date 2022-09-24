using AutoFixture;

namespace BunsenBurner.AutoFixture;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// Provides support for auto fixture in constructing arrange steps
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Auto arrange the scenario using auto fixture
    /// </summary>
    /// <param name="fn">async arrange function</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(Func<Fixture, Task<TData>> fn) =>
        Shared.AutoArrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Auto arrange the scenario using auto fixture
    /// </summary>
    /// <param name="fn">arrange function</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(Func<Fixture, TData> fn) =>
        Shared.AutoArrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Auto arrange the scenario using auto fixture
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">async arrange function</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(
        this string name,
        Func<Fixture, Task<TData>> fn
    ) => name.AutoArrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Auto arrange the scenario using auto fixture
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">arrange function</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(
        this string name,
        Func<Fixture, TData> fn
    ) => name.AutoArrange<TData, Syntax.Aaa>(fn);
}
