namespace BunsenBurner.Hedgehog;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Given a generator
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="generator">generator</param>
    /// <typeparam name="TData">data to generate</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<Gen<TData>> GivenGenerator<TData>(
        this string name,
        Gen<TData> generator
    ) => name.ArrangeGenerator<TData, Syntax.Bdd>(generator);

    /// <summary>
    /// Given a generator
    /// </summary>
    /// <param name="generator">generator</param>
    /// <typeparam name="TData">data to generate</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<Gen<TData>> GivenGenerator<TData>(
        this Gen<TData> generator
    ) => generator.ArrangeGenerator<TData, Syntax.Bdd>();

    /// <summary>
    /// Then that the property holds for all generated data
    /// </summary>
    /// <param name="scenario">scenario with generator</param>
    /// <param name="fn">property</param>
    /// <param name="config">optional config</param>
    /// <typeparam name="TData">some data</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Asserted<Gen<TData>, bool> ThenPropertyHolds<TData>(
        this BddScenario.Arranged<Gen<TData>> scenario,
        Func<TData, bool> fn,
        PropertyConfig? config = default
    ) => scenario.AssertPropertyHolds(fn, config);

    /// <summary>
    /// Then that the property holds for all generated data
    /// </summary>
    /// <param name="scenario">scenario with generator</param>
    /// <param name="fn">property</param>
    /// <param name="config">optional config</param>
    /// <typeparam name="TData">some data</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Asserted<Gen<TData>, bool> ThenPropertyHolds<TData>(
        this BddScenario.Arranged<Gen<TData>> scenario,
        Action<TData> fn,
        PropertyConfig? config = default
    ) => scenario.AssertPropertyHolds(fn, config);
}
