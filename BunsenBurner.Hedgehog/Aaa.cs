namespace BunsenBurner.Hedgehog;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Arranges a generator
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="generator">generator</param>
    /// <typeparam name="TData">data to generate</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Acted<Gen<TData>, Property<TData>> ArrangeGenerator<TData>(
        this string name,
        Gen<TData> generator
    ) => name.ArrangeGenerator<TData, Syntax.Aaa>(generator);

    /// <summary>
    /// Arranges a generator
    /// </summary>
    /// <param name="generator">generator</param>
    /// <typeparam name="TData">data to generate</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Acted<Gen<TData>, Property<TData>> ArrangeGenerator<TData>(
        this Gen<TData> generator
    ) => generator.ArrangeGenerator<TData, Syntax.Aaa>();

    /// <summary>
    /// Asserts that the property holds for all generated data
    /// </summary>
    /// <param name="scenario">scenario with generator</param>
    /// <param name="fn">property</param>
    /// <param name="config">optional config</param>
    /// <typeparam name="TData">some data</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<Gen<TData>, Property<TData>> AssertPropertyHolds<TData>(
        this AaaScenario.Acted<Gen<TData>, Property<TData>> scenario,
        Func<TData, bool> fn,
        PropertyConfig? config = default
    ) => scenario.AssertPropertyHolds<TData, Syntax.Aaa>(fn, config);

    /// <summary>
    /// Asserts that the property holds for all generated data
    /// </summary>
    /// <param name="scenario">scenario with generator</param>
    /// <param name="fn">property</param>
    /// <param name="config">optional config</param>
    /// <typeparam name="TData">some data</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<Gen<TData>, Property<TData>> AssertPropertyHolds<TData>(
        this AaaScenario.Acted<Gen<TData>, Property<TData>> scenario,
        Action<TData> fn,
        PropertyConfig? config = default
    ) => scenario.AssertPropertyHolds<TData, Syntax.Aaa>(fn, config);
}
