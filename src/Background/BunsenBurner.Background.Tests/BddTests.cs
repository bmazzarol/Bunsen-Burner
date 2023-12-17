using Microsoft.Extensions.Logging;

namespace BunsenBurner.Background.Tests;

using static Bdd;
using static BunsenBurner.Bdd;

public static class BddTests
{
    [Fact(DisplayName = "A background service can be started and run for a period")]
    public static async Task Case1() =>
        await GivenABackgroundService<Startup, Background>()
            .WhenRunFor(
                TimeSpan.FromMinutes(5),
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Then(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(
                    store,
                    x =>
                        x.Message == "Work complete"
                        && x.Level == LogLevel.Information
                        && x.ClassType == typeof(Background).FullName
                        && x.Exception == null
                        && x.EventId.Id == 0
                );
            });

    [Fact(
        DisplayName = "A background service can be started and run for a period with a description"
    )]
    public static async Task Case2() =>
        await "Some description"
            .GivenABackgroundService<Startup, Background>()
            .WhenRunFor(
                TimeSpan.FromMinutes(5),
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Then(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });

    [Fact(
        DisplayName = "A background service can be started and run for a period with existing arranged data"
    )]
    public static async Task Case3() =>
        await Given(() => 1)
            .AndABackgroundService<int, Startup, Background>()
            .WhenRunFor(
                x => x.BackgroundServiceContext,
                TimeSpan.FromMinutes(5),
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Then(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });

    [Fact(DisplayName = "A background service can be started and run against a schedule")]
    public static async Task Case4() =>
        await GivenABackgroundService<Startup, Background>()
            .WhenRunUntil(
                Schedule.Spaced(TimeSpan.FromMilliseconds(1))
                    & Schedule.MaxCumulativeDelay(TimeSpan.FromMinutes(5)),
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Then(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });

    [Fact(
        DisplayName = "A background service can be started and run against a schedule with a description"
    )]
    public static async Task Case5() =>
        await "Some description"
            .GivenABackgroundService<Startup, Background>()
            .WhenRunUntil(
                Schedule.Spaced(TimeSpan.FromMilliseconds(1))
                    & Schedule.MaxCumulativeDelay(TimeSpan.FromMinutes(5)),
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Then(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });

    [Fact(
        DisplayName = "A background service can be started and run against a schedule with existing arranged data"
    )]
    public static async Task Case6() =>
        await Given(() => 1)
            .AndABackgroundService<int, Startup, Background>()
            .WhenRunUntil(
                x => x.BackgroundServiceContext,
                Schedule.Spaced(TimeSpan.FromMilliseconds(1))
                    & Schedule.MaxCumulativeDelay(TimeSpan.FromMinutes(5)),
                context => context.Store.Any(x => x.Message == "Work complete")
            )
            .Then(store =>
            {
                Assert.NotEmpty(store);
                Assert.Contains(store, x => x.Message == "Work complete");
            });
}
