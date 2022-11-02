using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Http.Tests;

using static BunsenBurner.Aaa;

internal sealed class MyHealthCheck : IHealthCheck
{
    private readonly ILogger<MyHealthCheck> _logger;

    public MyHealthCheck(ILogger<MyHealthCheck> logger) => _logger = logger;

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new()
    )
    {
        _logger.LogInformation("Health checked");
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}

internal sealed class Startup : IStartup
{
    public void Configure(IApplicationBuilder app) => app.UseHealthChecks("/health");

    IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks().AddCheck<MyHealthCheck>("test");
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
            .ActAndCall(TestServerBuilderOptions.New<Startup>().Build())
            .IsOk()
            .And(ctx => Assert.Contains(ctx.Store, x => x.Message == "Health checked"));
}
