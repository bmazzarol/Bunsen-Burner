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
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > ArrangeBackgroundService<TStartup, TBackgroundService>(Sink? sink = default)
        where TStartup : new()
        where TBackgroundService : IHostedService =>
        Shared.ArrangeBackgroundService<TStartup, TBackgroundService, Syntax.Aaa>(sink);

    /// <summary>
    /// Arranges a background service context to run
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > ArrangeBackgroundService<TStartup, TBackgroundService>(this string name, Sink? sink = default)
        where TStartup : new()
        where TBackgroundService : IHostedService =>
        name.ArrangeBackgroundService<TStartup, TBackgroundService, Syntax.Aaa>(sink);

    /// <summary>
    /// Arranges a background service context to run along with existing arranged data
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TData">current arranged data</typeparam>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>arranged scenario</returns>
    [Pure]
    public static AaaScenario.Arranged<(TData Data, BackgroundServiceContext<TBackgroundService> BackgroundServiceContext)> AndABackgroundService<
        TData,
        TStartup,
        TBackgroundService
    >(this AaaScenario.Arranged<TData> scenario, Sink? sink = default)
        where TStartup : new()
        where TBackgroundService : IHostedService =>
        scenario.AndABackgroundService<TData, TStartup, TBackgroundService, Syntax.Aaa>(sink);

    /// <summary>
    /// Runs the background service until the predicate returns true, or the schedule ends, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function to setup and return the background service and store</param>
    /// <param name="schedule">custom schedule to supply waits</param>
    /// <param name="pred">predicate that indicates the job is complete, if this returns false, delay will be applied based on the schedule</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, LogMessageStore> ActAndRunUntil<
        TData,
        TBackgroundService
    >(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, BackgroundServiceContext<TBackgroundService>> fn,
        Schedule schedule,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    ) where TBackgroundService : IHostedService =>
        Shared.ActAndRunUntil(scenario, fn, schedule, pred);

    /// <summary>
    /// Runs the background service until the predicate returns true, or the schedule ends, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="schedule">custom schedule to supply waits</param>
    /// <param name="pred">predicate that indicates the job is complete, if this returns false, delay will be applied based on the schedule</param>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<
        BackgroundServiceContext<TBackgroundService>,
        LogMessageStore
    > ActAndRunUntil<TBackgroundService>(
        this AaaScenario.Arranged<BackgroundServiceContext<TBackgroundService>> scenario,
        Schedule schedule,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    ) where TBackgroundService : IHostedService => Shared.ActAndRunUntil(scenario, schedule, pred);

    /// <summary>
    /// Runs the background service for the given time, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function to setup and return the background service and store</param>
    /// <param name="maxCumulativeDelay">max cumulative delay to apply before cancelling the job, will stop early if the predicate returns true</param>
    /// <param name="pred">predicate that indicates the job is complete, if this returns false, 1 millisecond will be awaited and the predicate is rerun, stopping after maxCumulativeDelay</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, LogMessageStore> ActAndRunFor<TData, TBackgroundService>(
        this AaaScenario.Arranged<TData> scenario,
        Func<TData, BackgroundServiceContext<TBackgroundService>> fn,
        Duration maxCumulativeDelay,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    ) where TBackgroundService : IHostedService =>
        Shared.ActAndRunFor(scenario, fn, maxCumulativeDelay, pred);

    /// <summary>
    /// Runs the background service for the given time, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="maxCumulativeDelay">max cumulative delay to apply before cancelling the job, will stop early if the predicate returns true</param>
    /// <param name="pred">predicate that indicates the job is complete, if this returns false, 1 millisecond will be awaited and the predicate is rerun, stopping after maxCumulativeDelay</param>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<
        BackgroundServiceContext<TBackgroundService>,
        LogMessageStore
    > ActAndRunFor<TBackgroundService>(
        this AaaScenario.Arranged<BackgroundServiceContext<TBackgroundService>> scenario,
        Duration maxCumulativeDelay,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    ) where TBackgroundService : IHostedService =>
        Shared.ActAndRunFor(scenario, maxCumulativeDelay, pred);
}
