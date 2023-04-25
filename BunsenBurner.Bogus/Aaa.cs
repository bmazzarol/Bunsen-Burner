using Bogus;

namespace BunsenBurner.Bogus;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// Provides support for bogus in constructing arrange steps
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Auto arrange the scenario using bogus
    /// </summary>
    /// <param name="fn">async arrange function</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(Func<Faker<TData>, Task<TData>> fn)
        where TData : class => Shared.AutoArrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Auto arrange the scenario using bogus
    /// </summary>
    /// <param name="fn">arrange function</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(Func<Faker<TData>, TData> fn)
        where TData : class => Shared.AutoArrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Auto arrange the scenario using bogus
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">async arrange function</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(
        this string name,
        Func<Faker<TData>, Task<TData>> fn
    )
        where TData : class => name.AutoArrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Auto arrange the scenario using bogus
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">arrange function</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(
        this string name,
        Func<Faker<TData>, TData> fn
    )
        where TData : class => name.AutoArrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Auto arrange the scenario using bogus
    /// </summary>
    /// <param name="fn">async arrange function</param>
    /// <param name="locale">locale</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(
        Func<Faker, Task<TData>> fn,
        string locale = "en"
    ) => Shared.AutoArrange<TData, Syntax.Aaa>(fn, locale);

    /// <summary>
    /// Auto arrange the scenario using bogus
    /// </summary>
    /// <param name="fn">arrange function</param>
    /// <param name="locale">locale</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(
        Func<Faker, TData> fn,
        string locale = "en"
    ) => Shared.AutoArrange<TData, Syntax.Aaa>(fn, locale);

    /// <summary>
    /// Auto arrange the scenario using bogus
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">async arrange function</param>
    /// <param name="locale">locale</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(
        this string name,
        Func<Faker, Task<TData>> fn,
        string locale = "en"
    ) => name.AutoArrange<TData, Syntax.Aaa>(fn, locale);

    /// <summary>
    /// Auto arrange the scenario using bogus
    /// </summary>
    /// <param name="name">name/description for the test</param>
    /// <param name="fn">arrange function</param>
    /// <param name="locale">locale</param>
    /// <typeparam name="TData">auto arranged data to act on</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<TData> AutoArrange<TData>(
        this string name,
        Func<Faker, TData> fn,
        string locale = "en"
    ) => name.AutoArrange<TData, Syntax.Aaa>(fn, locale);
}
