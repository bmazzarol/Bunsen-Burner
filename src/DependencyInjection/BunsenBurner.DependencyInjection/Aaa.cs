namespace BunsenBurner.DependencyInjection;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Builds a service provider, then requests instances for all matching service descriptors
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="servicesGetter">services getter</param>
    /// <param name="pred">predicate for matching descriptors</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TData> ActAndAssertServicesAreConfigured<TData>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, IServiceCollection> servicesGetter,
        Func<ServiceDescriptor, bool> pred
    ) => scenario.ActAndAssertServicesAreConfigured<Syntax.Aaa, TData>(servicesGetter, pred);

    /// <summary>
    /// Builds a service provider, then requests instances for all matching service descriptors
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="servicesGetter">services getter</param>
    /// <param name="assemblies">assemblies to request instances from</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TData> ActAndAssertServicesAreConfigured<TData>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, IServiceCollection> servicesGetter,
        params Assembly[] assemblies
    ) => scenario.ActAndAssertServicesAreConfigured<Syntax.Aaa, TData>(servicesGetter, assemblies);
}
