using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.DependencyInjection.Tests;

[SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
public static class BddTests
{
    [Fact(DisplayName = "Service providers can be validated")]
    public static async Task Case1() =>
        await new ServiceCollection()
            .GivenData()
            .WhenConfiguredThenServicesAreValid(
                x =>
                {
                    var startup = new Startup(s => s.AddScoped<IA, A>().AddSingleton<IB, B>());
                    startup.Configure(x);
                    return x;
                },
                descriptor => descriptor.ServiceType == typeof(IA)
            );

    [Fact(DisplayName = "Service providers can be validated by assembly")]
    public static async Task Case2() =>
        await new ServiceCollection()
            .GivenData()
            .WhenConfiguredThenServicesAreValid(
                x =>
                {
                    var startup = new Startup(s => s.AddScoped<IA, A>().AddSingleton<IB, B>());
                    startup.Configure(x);
                    return x;
                },
                typeof(A).Assembly
            );
}
