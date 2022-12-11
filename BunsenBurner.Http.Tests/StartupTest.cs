using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Http.Tests;

using static BunsenBurner.Aaa;

internal interface ITestService
{
    int Result();
}

internal class TestService : ITestService
{
    private readonly int _result;

    public TestService(int result) => _result = result;

    public int Result() => _result;
}

internal sealed class MyHealthCheck : IHealthCheck
{
    private readonly ILogger<MyHealthCheck> _logger;
    private readonly ITestService _service;

    public MyHealthCheck(ILogger<MyHealthCheck> logger, ITestService service)
    {
        _logger = logger;
        _service = service;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new()
    )
    {
        _logger.LogInformation("Health checked");
        _logger.LogInformation("Result is {Result}", _service.Result());
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}

internal sealed class Startup
{
    public void Configure(IApplicationBuilder app) => app.UseHealthChecks("/health");

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks().AddCheck<MyHealthCheck>("test");
        services.AddScoped<ITestService>(_ => new TestService(1));
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
            .And(ctx => Assert.Contains(ctx.Store, x => x.Message == "Health checked"))
            .And(ctx => Assert.Contains(ctx.Store, x => x.Message == "Result is 1"));

    [Fact(
        DisplayName = "Using a startup class in the test service builder works with replacements"
    )]
    public static async Task Case2() =>
        await Request
            .GET("/health")
            .ArrangeRequest()
            .ActAndCall(
                TestServerBuilderOptions
                    .New<Startup>()
                    .WithServices(
                        services =>
                            services.Replace(
                                ServiceDescriptor.Scoped<ITestService>(_ => new TestService(2))
                            )
                    )
                    .Build(false)
            )
            .IsOk()
            .And(ctx => Assert.Contains(ctx.Store, x => x.Message == "Health checked"))
            .And(ctx => Assert.Contains(ctx.Store, x => x.Message == "Result is 2"));
}
