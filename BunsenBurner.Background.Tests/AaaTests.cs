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
            var delay = 1 * ms;
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
            .ActAndRunFor(
                5 * minutes,
                context => context.Store.Any(x => x.Message == "Work complete")
            )
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
            .ActAndRunFor(
                5 * minutes,
                context => context.Store.Any(x => x.Message == "Work complete")
            )
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
            .ActAndRunFor(
                x => x.BackgroundServiceContext,
                5 * minutes,
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });

    [Fact(DisplayName = "A background service can be started and run against a schedule")]
    public static async Task Case4() =>
        await ArrangeBackgroundService<Startup, Background>()
            .ActAndRunUntil(
                Schedule.spaced(1 * ms) & Schedule.maxCumulativeDelay(5 * minutes),
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });

    [Fact(
        DisplayName = "A background service can be started and run against a schedule with a description"
    )]
    public static async Task Case5() =>
        await "Some description"
            .ArrangeBackgroundService<Startup, Background>()
            .ActAndRunUntil(
                Schedule.spaced(1 * ms) & Schedule.maxCumulativeDelay(5 * minutes),
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });

    [Fact(
        DisplayName = "A background service can be started and run against a schedule with existing arranged data"
    )]
    public static async Task Case6() =>
        await Arrange(() => 1)
            .AndABackgroundService<int, Startup, Background>()
            .ActAndRunUntil(
                x => x.BackgroundServiceContext,
                Schedule.spaced(1 * ms) & Schedule.maxCumulativeDelay(5 * minutes),
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });
}
