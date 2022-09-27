using Bogus;

namespace BunsenBurner.Bogus;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// Provides support for bogus in constructing given steps
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Auto complete the given step in the scenario using bogus
    /// </summary>
    /// <param name="fn">async given function</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> AutoGiven<TData>(Func<Faker<TData>, Task<TData>> fn)
        where TData : class => Shared.AutoArrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Auto complete the given step in the scenario using bogus
    /// </summary>
    /// <param name="fn">given function</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> AutoGiven<TData>(Func<Faker<TData>, TData> fn)
        where TData : class => Shared.AutoArrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Auto complete the given step in the scenario using bogus
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">async given function</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> AutoGiven<TData>(
        this string name,
        Func<Faker<TData>, Task<TData>> fn
    ) where TData : class => name.AutoArrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Auto complete the given step in the scenario using bogus
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">given function</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> AutoGiven<TData>(
        this string name,
        Func<Faker<TData>, TData> fn
    ) where TData : class => name.AutoArrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Auto complete the given step in the scenario using bogus
    /// </summary>
    /// <param name="fn">async given function</param>
    /// <param name="locale">locale</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> AutoGiven<TData>(
        Func<Faker, Task<TData>> fn,
        string locale = "en"
    ) => Shared.AutoArrange<TData, Syntax.Bdd>(fn, locale);

    /// <summary>
    /// Auto complete the given step in the scenario using bogus
    /// </summary>
    /// <param name="fn">given function</param>
    /// <param name="locale">locale</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> AutoGiven<TData>(
        Func<Faker, TData> fn,
        string locale = "en"
    ) => Shared.AutoArrange<TData, Syntax.Bdd>(fn, locale);

    /// <summary>
    /// Auto complete the given step in the scenario using bogus
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">async given function</param>
    /// <param name="locale">locale</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> AutoGiven<TData>(
        this string name,
        Func<Faker, Task<TData>> fn,
        string locale = "en"
    ) => name.AutoArrange<TData, Syntax.Bdd>(fn, locale);

    /// <summary>
    /// Auto complete the given step in the scenario using bogus
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">given function</param>
    /// <param name="locale">locale</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> AutoGiven<TData>(
        this string name,
        Func<Faker, TData> fn,
        string locale = "en"
    ) => name.AutoArrange<TData, Syntax.Bdd>(fn, locale);
}
