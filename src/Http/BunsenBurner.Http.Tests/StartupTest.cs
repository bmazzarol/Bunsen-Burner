using System.Diagnostics.CodeAnalysis;
using BunsenBurner.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using static BunsenBurner.Http.Tests.Shared;
using static System.StringComparison;

namespace BunsenBurner.Http.Tests;

using static BunsenBurner.Aaa;

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
            .WithHeader("a", "1", "2", "3")
            .WithHeader("b", "4")
            .ArrangeRequest()
            .ActAndCall(
                TestServerBuilderOptions
                    .New<Startup>()
                    .WithLogMessageSink(Sink.New(_outputHelper.WriteLine))
                    .Build()
            )
            .Assert(ResponseCodeIsOk)
            .And(
                ctx =>
                    Assert.Contains(
                        ctx.Store,
                        x => string.Equals(x.Message, "Health checked", Ordinal)
                    )
            )
            .And(
                ctx =>
                    Assert.Contains(
                        ctx.Store,
                        x => string.Equals(x.Message, "Result is 1", Ordinal)
                    )
            );

    [Fact(
        DisplayName = "Using a startup class in the test service builder works with replacements"
    )]
    public async Task Case2() =>
        await Req.Get.To("/health")
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
                    .WithLogMessageSink(Sink.New(_outputHelper.WriteLine))
                    .Build()
            )
            .Assert(ResponseCodeIsOk)
            .And(
                ctx =>
                    Assert.Contains(
                        ctx.Store,
                        x => string.Equals(x.Message, "Health checked", Ordinal)
                    )
            )
            .And(
                ctx =>
                    Assert.Contains(
                        ctx.Store,
                        x => string.Equals(x.Message, "Result is 2", Ordinal)
                    )
            );

    [Fact(DisplayName = "Test sending a body in the request to test server and logging it")]
    public async Task Case3() =>
        await Req.Post.To("/health")
            .WithJsonContent(new { A = 1 })
            .ArrangeRequest()
            .ActAndCall(
                TestServerBuilderOptions
                    .New<Startup>()
                    .WithLogMessageSink(Sink.New(_outputHelper.WriteLine))
                    .BuildAndCache()
            )
            .Assert(ResponseCodeIsOk)
            .And(
                ctx =>
                    Assert.Equal(
                        "no-store, no-cache",
                        ctx.Response.Headers.GetAsString("Cache-Control")
                    )
            );
}
