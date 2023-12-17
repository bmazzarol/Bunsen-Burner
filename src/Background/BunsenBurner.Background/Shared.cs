using BunsenBurner.Logging;
using Microsoft.Extensions.Hosting;
using static BunsenBurner.Shared;

namespace BunsenBurner.Background;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > ArrangeBackgroundService<TStartup, TBackgroundService, TSyntax>(Sink? sink = default)
        where TStartup : new()
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax =>
        Arrange<BackgroundServiceContext<TBackgroundService>, TSyntax>(
            () =>
                Task.FromResult(
                    BackgroundServiceBuilder.CreateAndCache<TStartup, TBackgroundService>(sink)
                )
        );

    [Pure]
    internal static Scenario<TSyntax>.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > ArrangeBackgroundService<TStartup, TBackgroundService, TSyntax>(
        this string name,
        Sink? sink = default
    )
        where TStartup : new()
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax =>
        name.Arrange<BackgroundServiceContext<TBackgroundService>, TSyntax>(
            () =>
                Task.FromResult(
                    BackgroundServiceBuilder.CreateAndCache<TStartup, TBackgroundService>(sink)
                )
        );

    [Pure]
    internal static Scenario<TSyntax>.Arranged<(
        TData Data,
        BackgroundServiceContext<TBackgroundService> BackgroundServiceContext
    )> AndABackgroundService<TData, TStartup, TBackgroundService, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Sink? sink = default
    )
        where TStartup : new()
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax =>
        scenario.And(data =>
        {
            var ctx = BackgroundServiceBuilder.CreateAndCache<TStartup, TBackgroundService>(sink);
            return (Data: data, BackgroundServiceContext: ctx);
        });

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, LogMessageStore> ActAndRunUntil<
        TData,
        TBackgroundService,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, BackgroundServiceContext<TBackgroundService>> selector,
        Schedule schedule,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    )
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax =>
        scenario.Act(async data =>
        {
            var (service, store) = selector(data);
            await service.StartAsync(CancellationToken.None);
            foreach (var duration in schedule)
            {
                if (pred(new BackgroundServiceContext<TBackgroundService>(service, store)))
                {
                    break;
                }
                await Task.Delay((TimeSpan)duration);
            }
            await service.StopAsync(CancellationToken.None);
            return store;
        });

    [Pure]
    internal static Scenario<TSyntax>.Acted<
        BackgroundServiceContext<TBackgroundService>,
        LogMessageStore
    > ActAndRunUntil<TBackgroundService, TSyntax>(
        this Scenario<TSyntax>.Arranged<BackgroundServiceContext<TBackgroundService>> scenario,
        Schedule schedule,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    )
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax => scenario.ActAndRunUntil(_ => _, schedule, pred);

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, LogMessageStore> ActAndRunFor<
        TData,
        TBackgroundService,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, BackgroundServiceContext<TBackgroundService>> fn,
        Duration maxCumulativeDelay,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    )
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax =>
        scenario.ActAndRunUntil(
            fn,
            Schedule.Fixed(TimeSpan.FromMilliseconds(1))
                & Schedule.MaxCumulativeDelay(maxCumulativeDelay),
            pred
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<
        BackgroundServiceContext<TBackgroundService>,
        LogMessageStore
    > ActAndRunFor<TBackgroundService, TSyntax>(
        this Scenario<TSyntax>.Arranged<BackgroundServiceContext<TBackgroundService>> scenario,
        Duration maxCumulativeDelay,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    )
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax => scenario.ActAndRunFor(_ => _, maxCumulativeDelay, pred);
}
