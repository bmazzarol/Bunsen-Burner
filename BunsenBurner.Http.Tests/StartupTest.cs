using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.Http.Tests;

using static BunsenBurner.Aaa;

internal sealed class Startup : IStartup
{
    public void Configure(IApplicationBuilder app) => app.UseHealthChecks("/health");

    IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks();
        return services.BuildServiceProvider();
    }
}

public static class StartupTest
{
    [Fact(DisplayName = "Using a startup class in the test service builder works")]
    public static async Task Case1() =>
        await Request
            .GET("/health")
            .ArrangeRequest()
            .ActAndCall(TestServerBuilder.Create<Startup>())
            .IsOk();
}
