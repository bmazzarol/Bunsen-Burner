using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.DependencyInjection.Tests;

public static class AaaTests
{
    [Fact(DisplayName = "Service providers can be validated")]
    public static async Task Case1() =>
        await new ServiceCollection()
            .ArrangeData()
            .ActAndAssertServicesAreConfigured(
                x =>
                {
                    var startup = new Startup(s => s.AddScoped<IA, A>().AddSingleton<IB, B>());
                    startup.Configure(x);
                    return x;
                },
                descriptor => descriptor.ServiceType == typeof(IA)
            );

    [Fact(DisplayName = "Invalid service providers can be validated")]
    public static async Task Case2()
    {
        var exception = await Assert.ThrowsAsync<AggregateException>(
            async () =>
                await new ServiceCollection()
                    .ArrangeData()
                    .ActAndAssertServicesAreConfigured(
                        x =>
                        {
                            var startup = new Startup(s => s.AddScoped<IA, A>());
                            startup.Configure(x);
                            return x;
                        },
                        descriptor => descriptor.ServiceType == typeof(IA)
                    )
        );
        Assert.Contains(
            exception.InnerExceptions,
            e =>
                e.Message.Contains(
                    "Unable to resolve service for type 'BunsenBurner.DependencyInjection.Tests.IB' while attempting to activate 'BunsenBurner.DependencyInjection.Tests.A'."
                )
        );
    }

    [Fact(DisplayName = "Service providers can be validated by assembly")]
    public static async Task Case3() =>
        await new ServiceCollection()
            .ArrangeData()
            .ActAndAssertServicesAreConfigured(
                x =>
                {
                    var startup = new Startup(s => s.AddScoped<IA, A>().AddSingleton<IB, B>());
                    startup.Configure(x);
                    return x;
                },
                typeof(A).Assembly
            );
}
