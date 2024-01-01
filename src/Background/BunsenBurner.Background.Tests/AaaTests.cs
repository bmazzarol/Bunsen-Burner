using System.Diagnostics.CodeAnalysis;
using BunsenBurner.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static System.StringComparison;

namespace BunsenBurner.Background.Tests;

using static Aaa;
using static BunsenBurner.Aaa;

internal interface ITestService
{
    int Result();
}

internal sealed class TestService(int result) : ITestService
{
    public int Result() => result;
}

internal sealed class Background(ILogger<Background> logger, IServiceProvider provider)
    : BackgroundService
{
    [ExcludeFromCodeCoverage]
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var service = provider.GetService<ITestService>();
        logger.LogInformation("Starting work...");
        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = TimeSpan.FromMilliseconds(1);
            logger.LogInformation("Doing work for {Delay} duration", delay);
            await Task.Delay(delay, stoppingToken);
            if (service != null)
                logger.LogInformation("Value was {Result}", service.Result());
            logger.LogInformation("Work complete");
        }
    }
}

internal sealed class Startup
{
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddHostedService<Background>();
    }
}

public sealed class AaaTests(ITestOutputHelper testOutput)
{
    [Fact(DisplayName = "A background service can be started and run for a period")]
    public async Task Case1() =>
        await ArrangeBackgroundService<Startup, Background>(Sink.New(testOutput.WriteLine))
            .ActAndRunFor(
                TimeSpan.FromMinutes(5),
                context =>
                    context.Store.Any(x => string.Equals(x.Message, "Work complete", Ordinal))
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => string.Equals(x.Message, "Work complete", Ordinal));
            });

    [Fact(
        DisplayName = "A background service can be started and run for a period with a description"
    )]
    public async Task Case2() =>
        await "Some description"
            .ArrangeBackgroundService<Startup, Background>(Sink.New(testOutput.WriteLine))
            .ActAndRunFor(
                TimeSpan.FromMinutes(5),
                context =>
                    context.Store.Any(x => string.Equals(x.Message, "Work complete", Ordinal))
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => string.Equals(x.Message, "Work complete", Ordinal));
            });

    [Fact(
        DisplayName = "A background service can be started and run for a period with existing arranged data"
    )]
    public async Task Case3() =>
        await Arrange(() => 1)
            .AndABackgroundService<int, Startup, Background>(Sink.New(testOutput.WriteLine))
            .ActAndRunFor(
                x => x.BackgroundServiceContext,
                TimeSpan.FromMinutes(5),
                context =>
                    context.Store.Any(x => string.Equals(x.Message, "Work complete", Ordinal))
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => string.Equals(x.Message, "Work complete", Ordinal));
            });

    [Fact(DisplayName = "A background service can be started and run against a schedule")]
    public async Task Case4() =>
        await ArrangeBackgroundService<Startup, Background>(Sink.New(testOutput.WriteLine))
            .ActAndRunUntil(
                Schedule.Spaced(TimeSpan.FromMilliseconds(1))
                    & Schedule.MaxCumulativeDelay(TimeSpan.FromMinutes(5)),
                context =>
                    context.Store.Any(x => string.Equals(x.Message, "Work complete", Ordinal))
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => string.Equals(x.Message, "Work complete", Ordinal));
            });

    [Fact(
        DisplayName = "A background service can be started and run against a schedule with a description"
    )]
    public async Task Case5() =>
        await "Some description"
            .ArrangeBackgroundService<Startup, Background>(Sink.New(testOutput.WriteLine))
            .ActAndRunUntil(
                Schedule.Spaced(TimeSpan.FromMilliseconds(1))
                    & Schedule.MaxCumulativeDelay(TimeSpan.FromMinutes(5)),
                context =>
                    context.Store.Any(x => string.Equals(x.Message, "Work complete", Ordinal))
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => string.Equals(x.Message, "Work complete", Ordinal));
            });

    [Fact(
        DisplayName = "A background service can be started and run against a schedule with existing arranged data"
    )]
    public async Task Case6() =>
        await Arrange(() => 1)
            .AndABackgroundService<int, Startup, Background>(Sink.New(testOutput.WriteLine))
            .ActAndRunUntil(
                x => x.BackgroundServiceContext,
                Schedule.Spaced(TimeSpan.FromMilliseconds(1))
                    & Schedule.MaxCumulativeDelay(TimeSpan.FromMinutes(5)),
                context =>
                    context.Store.Any(x => string.Equals(x.Message, "Work complete", Ordinal))
            )
            .Assert(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => string.Equals(x.Message, "Work complete", Ordinal));
            });

    [Fact(
        DisplayName = "A background service can be started and run against a schedule with existing arranged data and no caching"
    )]
    public async Task Case7() =>
        await Arrange(() => 99)
            .ActAndRunUntil(
                i =>
                    BackgroundServiceBuilder.Create<Startup, Background>(
                        collection => collection.AddSingleton<ITestService>(new TestService(i)),
                        Sink.New(testOutput.WriteLine)
                    ),
                Schedule.Spaced(TimeSpan.FromMilliseconds(1))
                    & Schedule.MaxCumulativeDelay(TimeSpan.FromMinutes(5)),
                context =>
                    context.Store.Any(x => string.Equals(x.Message, "Work complete", Ordinal))
            )
            .Assert(
                (i, store) =>
                {
                    Assert.NotEmpty(store);
                    Assert.Contains(store, x => string.Equals(x.Message, "Work complete", Ordinal));
                    Assert.Contains(
                        store,
                        x => string.Equals(x.Message, $"Value was {i}", Ordinal)
                    );
                }
            );
}
