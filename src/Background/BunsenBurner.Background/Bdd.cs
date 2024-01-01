using BunsenBurner.Logging;
using Microsoft.Extensions.Hosting;

namespace BunsenBurner.Background;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Given a background service context to run
    /// </summary>
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > GivenABackgroundService<TStartup, TBackgroundService>(Sink? sink = default)
        where TStartup : new()
        where TBackgroundService : IHostedService =>
        Shared.ArrangeBackgroundService<TStartup, TBackgroundService, Syntax.Bdd>(sink);

    /// <summary>
    /// Given a background service context to run
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > GivenABackgroundService<TStartup, TBackgroundService>(this string name, Sink? sink = default)
        where TStartup : new()
        where TBackgroundService : IHostedService =>
        name.ArrangeBackgroundService<TStartup, TBackgroundService, Syntax.Bdd>(sink);

    /// <summary>
    /// Given a background service context to run along with existing given data
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TData">current arranged data</typeparam>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<(
        TData Data,
        BackgroundServiceContext<TBackgroundService> BackgroundServiceContext
    )> AndABackgroundService<TData, TStartup, TBackgroundService>(
        this BddScenario.Arranged<TData> scenario,
        Sink? sink = default
    )
        where TStartup : new()
        where TBackgroundService : IHostedService =>
        scenario.AndABackgroundService<TData, TStartup, TBackgroundService, Syntax.Bdd>(sink);

    /// <summary>
    /// Runs the background service until the predicate returns true, or the schedule ends, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function to setup and return the background service and store</param>
    /// <param name="schedule">custom schedule to supply waits</param>
    /// <param name="pred">predicate that indicates the job is complete, if this returns false, delay will be applied based on the schedule</param>
    /// <param name="maxRunDuration">maximum time the operation will run for before failing; defaults to 1 minute</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, LogMessageStore> WhenRunUntil<TData, TBackgroundService>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, BackgroundServiceContext<TBackgroundService>> fn,
        Schedule schedule,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred,
        TimeSpan? maxRunDuration = default
    )
        where TBackgroundService : IHostedService =>
        scenario.ActAndRunUntil(fn, schedule, pred, maxRunDuration);

    /// <summary>
    /// Runs the background service until the predicate returns true, or the schedule ends, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="schedule">custom schedule to supply waits</param>
    /// <param name="pred">predicate that indicates the job is complete, if this returns false, delay will be applied based on the schedule</param>
    /// <param name="maxRunDuration">maximum time the operation will run for before failing; defaults to 1 minute</param>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<
        BackgroundServiceContext<TBackgroundService>,
        LogMessageStore
    > WhenRunUntil<TBackgroundService>(
        this BddScenario.Arranged<BackgroundServiceContext<TBackgroundService>> scenario,
        Schedule schedule,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred,
        TimeSpan? maxRunDuration = default
    )
        where TBackgroundService : IHostedService =>
        scenario.ActAndRunUntil(schedule, pred, maxRunDuration);

    /// <summary>
    /// Runs the background service for the given time, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function to setup and return the background service and store</param>
    /// <param name="maxCumulativeDelay">max cumulative delay to apply before cancelling the job, will stop early if the predicate returns true</param>
    /// <param name="pred">predicate that indicates the job is complete, if this returns false, 1 millisecond will be awaited and the predicate is rerun, stopping after maxCumulativeDelay</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, LogMessageStore> WhenRunFor<TData, TBackgroundService>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, BackgroundServiceContext<TBackgroundService>> fn,
        Duration maxCumulativeDelay,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    )
        where TBackgroundService : IHostedService =>
        scenario.ActAndRunFor(fn, maxCumulativeDelay, pred);

    /// <summary>
    /// Runs the background service for the given time, returning any log messages
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="maxCumulativeDelay">max cumulative delay to apply before cancelling the job, will stop early if the predicate returns true</param>
    /// <param name="pred">predicate that indicates the job is complete, if this returns false, 1 millisecond will be awaited and the predicate is rerun, stopping after maxCumulativeDelay</param>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<
        BackgroundServiceContext<TBackgroundService>,
        LogMessageStore
    > WhenRunFor<TBackgroundService>(
        this BddScenario.Arranged<BackgroundServiceContext<TBackgroundService>> scenario,
        Duration maxCumulativeDelay,
        Func<BackgroundServiceContext<TBackgroundService>, bool> pred
    )
        where TBackgroundService : IHostedService =>
        scenario.ActAndRunFor(maxCumulativeDelay, pred);
}
