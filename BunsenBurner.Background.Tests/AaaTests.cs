using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Background.Tests;

using static Aaa;
using static BunsenBurner.Aaa;

internal sealed class Background : BackgroundService
{
    private readonly ILogger<Background> _logger;

    public Background(ILogger<Background> logger) => _logger = logger;

    [ExcludeFromCodeCoverage]
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting work...");
        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = TimeSpan.FromMilliseconds(10);
            _logger.LogInformation("Doing work for {Delay} duration", delay);
            await Task.Delay(delay, stoppingToken);
            _logger.LogInformation("Work complete");
        }
    }
}

internal sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddHostedService<Background>();
    }
}

public static class AaaTests
{
    [Fact(DisplayName = "A background service can be started and run for a period")]
    public static async Task Case1() =>
        await ArrangeBackgroundService<Startup, Background>()
            .ActAndRunFor(TimeSpan.FromMilliseconds(40))
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });

    [Fact(
        DisplayName = "A background service can be started and run for a period with a description"
    )]
    public static async Task Case2() =>
        await "Some description"
            .ArrangeBackgroundService<Startup, Background>()
            .ActAndRunFor(TimeSpan.FromMilliseconds(40))
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });

    [Fact(
        DisplayName = "A background service can be started and run for a period with existing arranged data"
    )]
    public static async Task Case3() =>
        await Arrange(() => 1)
            .AndABackgroundService<int, Startup, Background>()
            .ActAndRunFor(x => x.BackgroundServiceContext, TimeSpan.FromMilliseconds(40))
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });
}
