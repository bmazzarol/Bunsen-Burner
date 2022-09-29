using BunsenBurner.Logging;
using Microsoft.Extensions.Hosting;

namespace BunsenBurner.Background;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > ArrangeBackgroundService<TStartup, TBackgroundService, TSyntax>()
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax
        where TStartup : new() =>
        new(
            default,
            () => Task.FromResult(BackgroundServiceBuilder.Create<TStartup, TBackgroundService>())
        );

    [Pure]
    internal static Scenario<TSyntax>.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > ArrangeBackgroundService<TStartup, TBackgroundService, TSyntax>(this string name)
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax
        where TStartup : new() =>
        new(
            name,
            () => Task.FromResult(BackgroundServiceBuilder.Create<TStartup, TBackgroundService>())
        );

    [Pure]
    internal static Scenario<TSyntax>.Arranged<(TData Data, BackgroundServiceContext<TBackgroundService> BackgroundServiceContext)> AndABackgroundService<
        TData,
        TStartup,
        TBackgroundService,
        TSyntax
    >(this Scenario<TSyntax>.Arranged<TData> scenario)
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax
        where TStartup : new() =>
        new(
            scenario.Name,
            async () =>
            {
                var data = await scenario.ArrangeScenario();
                var ctx = BackgroundServiceBuilder.Create<TStartup, TBackgroundService>();
                return (Data: data, BackgroundServiceContext: ctx);
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, LogMessageStore> ActAndRunFor<
        TData,
        TBackgroundService,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, BackgroundServiceContext<TBackgroundService>> fn,
        TimeSpan runDuration
    )
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data =>
            {
                var (service, store) = fn(data);
                await service.StartAsync(CancellationToken.None);
                await Task.Delay(runDuration);
                await service.StopAsync(CancellationToken.None);
                return store;
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<
        BackgroundServiceContext<TBackgroundService>,
        LogMessageStore
    > ActAndRunFor<TBackgroundService, TSyntax>(
        this Scenario<TSyntax>.Arranged<BackgroundServiceContext<TBackgroundService>> scenario,
        TimeSpan runDuration
    )
        where TBackgroundService : IHostedService
        where TSyntax : struct, Syntax => scenario.ActAndRunFor(_ => _, runDuration);
}
