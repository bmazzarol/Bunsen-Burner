using AutoFixture;

namespace BunsenBurner.AutoFixture;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// Provides support for auto fixture in constructing given steps
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Auto complete the given step in the scenario using auto fixture
    /// </summary>
    /// <param name="fn">async given function</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static BddScenario.Arranged<TData> AutoGiven<TData>(Func<Fixture, Task<TData>> fn) =>
        Shared.AutoArrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Auto complete the given step in the scenario using auto fixture
    /// </summary>
    /// <param name="fn">given function</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static BddScenario.Arranged<TData> AutoGiven<TData>(Func<Fixture, TData> fn) =>
        Shared.AutoArrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Auto complete the given step in the scenario using auto fixture
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">async given function</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static BddScenario.Arranged<TData> AutoGiven<TData>(
        this string name,
        Func<Fixture, Task<TData>> fn
    ) => name.AutoArrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Auto complete the given step in the scenario using auto fixture
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">given function</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    public static BddScenario.Arranged<TData> AutoGiven<TData>(
        this string name,
        Func<Fixture, TData> fn
    ) => name.AutoArrange<TData, Syntax.Bdd>(fn);
}
