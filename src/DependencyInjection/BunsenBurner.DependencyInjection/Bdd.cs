namespace BunsenBurner.DependencyInjection;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Builds a service provider, then requests instances for all matching service descriptors
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="servicesGetter">services getter</param>
    /// <param name="pred">predicate for matching descriptors</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TData> WhenConfiguredThenServicesAreValid<TData>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, IServiceCollection> servicesGetter,
        Func<ServiceDescriptor, bool> pred
    ) => scenario.ActAndAssertServicesAreConfigured(servicesGetter, pred);

    /// <summary>
    /// Builds a service provider, then requests instances for all matching service descriptors
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="servicesGetter">services getter</param>
    /// <param name="assemblies">assemblies to request instances from</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TData> WhenConfiguredThenServicesAreValid<TData>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, IServiceCollection> servicesGetter,
        params Assembly[] assemblies
    ) => scenario.ActAndAssertServicesAreConfigured(servicesGetter, assemblies);
}
