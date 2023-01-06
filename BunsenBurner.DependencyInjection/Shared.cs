namespace BunsenBurner.DependencyInjection;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TData> ActAndAssertServicesAreConfigured<
        TSyntax,
        TData
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, IServiceCollection> servicesGetter,
        Func<ServiceDescriptor, bool> pred
    ) where TSyntax : struct, Syntax =>
        scenario
            .Act(r =>
            {
                var services = servicesGetter(r);
                var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var exceptions = services
                    .Where(pred)
                    .Select(x =>
                    {
                        try
                        {
                            scope.ServiceProvider.GetService(x.ServiceType);
                            return null;
                        }
                        catch (InvalidOperationException e)
                        {
                            return e;
                        }
                    })
                    .OfType<InvalidOperationException>()
                    .ToList();

                if (exceptions.Any())
                {
                    throw new AggregateException(
                        "Service Provider is missing services",
                        exceptions
                    );
                }

                return r;
            })
            .Assert(_ => true);

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TData, TData> ActAndAssertServicesAreConfigured<
        TSyntax,
        TData
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, IServiceCollection> servicesGetter,
        params Assembly[] assemblies
    ) where TSyntax : struct, Syntax =>
        scenario.ActAndAssertServicesAreConfigured(
            servicesGetter,
            descriptor => assemblies.Contains(descriptor.ServiceType.Assembly)
        );
}
