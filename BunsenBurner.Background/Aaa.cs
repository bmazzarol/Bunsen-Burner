using BunsenBurner.Logging;
using Microsoft.Extensions.Hosting;

namespace BunsenBurner.Background;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Arranges a background service context to run
    /// </summary>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > ArrangeBackgroundService<TStartup, TBackgroundService>()
        where TBackgroundService : IHostedService
        where TStartup : new() =>
        Shared.ArrangeBackgroundService<TStartup, TBackgroundService, Syntax.Aaa>();

    /// <summary>
    /// Arranges a background service context to run
    /// </summary>
    /// <param name="name">name/description</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > ArrangeBackgroundService<TStartup, TBackgroundService>(this string name)
        where TBackgroundService : IHostedService
        where TStartup : new() =>
        name.ArrangeBackgroundService<TStartup, TBackgroundService, Syntax.Aaa>();

    /// <summary>
    /// Arranges a background service context to run along with existing arranged data
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TData">current arranged data</typeparam>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<(TData Data, BackgroundServiceContext<TBackgroundService> BackgroundServiceContext)> AndABackgroundService<
        TData,
        TStartup,
        TBackgroundService
    >(this AaaScenario.Arranged<TData> scenario)
        where TBackgroundService : IHostedService
        where TStartup : new() =>
        scenario.AndABackgroundService<TData, TStartup, TBackgroundService, Syntax.Aaa>();

    /// <summary>
    /// Runs the background service for the given time, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function to setup and return the background service and store</param>
    /// <param name="runDuration">duration the background service should run for</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, LogMessageStore> ActAndRunFor<TData, TBackgroundService>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, BackgroundServiceContext<TBackgroundService>> fn,
        TimeSpan runDuration
    ) where TBackgroundService : IHostedService =>
        scenario.ActAndRunFor<TData, TBackgroundService, Syntax.Aaa>(fn, runDuration);

    /// <summary>
    /// Runs the background service for the given time, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="runDuration">duration the background service should run for</param>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<
        BackgroundServiceContext<TBackgroundService>,
        LogMessageStore
    > ActAndRunFor<TBackgroundService>(
        this AaaScenario.Arranged<BackgroundServiceContext<TBackgroundService>> scenario,
        TimeSpan runDuration
    ) where TBackgroundService : IHostedService =>
        scenario.ActAndRunFor<TBackgroundService, Syntax.Aaa>(runDuration);
}
