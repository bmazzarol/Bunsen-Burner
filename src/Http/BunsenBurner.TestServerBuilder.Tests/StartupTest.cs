using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Tests;

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
