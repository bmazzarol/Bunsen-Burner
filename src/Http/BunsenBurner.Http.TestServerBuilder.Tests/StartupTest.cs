using System.Diagnostics.CodeAnalysis;
using BunsenBurner.Http.Extensions;
using BunsenBurner.Logging;
using HttpBuildR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using Req = System.Net.Http.HttpMethod;

namespace BunsenBurner.Http.Tests;

internal interface ITestService
{
    int Result();
}

internal sealed class TestService : ITestService
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

[SuppressMessage("Performance", "CA1822:Mark members as static")]
internal sealed class Startup
{
    public void Configure(IApplicationBuilder app) => app.UseHealthChecks("/health");

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks().AddCheck<MyHealthCheck>("test");
        services.AddScoped<ITestService>(_ => new TestService(1));
    }
}

public sealed class StartupTest
{
    private readonly ITestOutputHelper _outputHelper;

    public StartupTest(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

    [Fact(DisplayName = "Using a startup class in the test service builder works")]
    public async Task Case1() =>
        await Req.Get.To("/health")
            .ArrangeData()
            .Act(
                new TestServerBuilder.Options
                {
                    Startup = typeof(Startup),
                    Sink = Sink.New(_outputHelper.WriteLine)
                }
                    .Build()
                    .CallTestServer()
            )
            .Assert(ctx => ctx.Response.IsSuccessStatusCode)
            .And(ctx =>
            {
                Assert.Contains(
                    ctx.Store,
                    x => string.Equals(x.Message, "Result is 1", StringComparison.Ordinal)
                );
                Assert.Contains(
                    ctx.Store,
                    x => string.Equals(x.Message, "Health checked", StringComparison.Ordinal)
                );
            });
}
