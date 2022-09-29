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
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > GivenABackgroundService<TStartup, TBackgroundService>()
        where TBackgroundService : IHostedService
        where TStartup : new() =>
        Shared.ArrangeBackgroundService<TStartup, TBackgroundService, Syntax.Bdd>();

    /// <summary>
    /// Given a background service context to run
    /// </summary>
    /// <param name="name">name/description</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<
        BackgroundServiceContext<TBackgroundService>
    > GivenABackgroundService<TStartup, TBackgroundService>(this string name)
        where TBackgroundService : IHostedService
        where TStartup : new() =>
        name.ArrangeBackgroundService<TStartup, TBackgroundService, Syntax.Bdd>();

    /// <summary>
    /// Given a background service context to run along with existing given data
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <typeparam name="TData">current arranged data</typeparam>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service class</typeparam>
    /// <returns>given scenario</returns>
    [Pure]
    public static BddScenario.Arranged<(TData Data, BackgroundServiceContext<TBackgroundService> BackgroundServiceContext)> AndABackgroundService<
        TData,
        TStartup,
        TBackgroundService
    >(this BddScenario.Arranged<TData> scenario)
        where TBackgroundService : IHostedService
        where TStartup : new() =>
        scenario.AndABackgroundService<TData, TStartup, TBackgroundService, Syntax.Bdd>();

    /// <summary>
    /// Runs the background service for the given time, returning any log messages
    /// </summary>
    /// <param name="scenario">given scenario</param>
    /// <param name="fn">function to setup and return the background service and store</param>
    /// <param name="runDuration">duration the background service should run for</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, LogMessageStore> WhenRunFor<TData, TBackgroundService>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, BackgroundServiceContext<TBackgroundService>> fn,
        TimeSpan runDuration
    ) where TBackgroundService : IHostedService => scenario.ActAndRunFor(fn, runDuration);

    /// <summary>
    /// Runs the background service for the given time, returning any log messages
    /// </summary>
    /// <param name="scenario">given scenario</param>
    /// <param name="runDuration">duration the background service should run for</param>
    /// <typeparam name="TBackgroundService">background service to test</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<
        BackgroundServiceContext<TBackgroundService>,
        LogMessageStore
    > WhenRunFor<TBackgroundService>(
        this BddScenario.Arranged<BackgroundServiceContext<TBackgroundService>> scenario,
        TimeSpan runDuration
    ) where TBackgroundService : IHostedService => scenario.ActAndRunFor(runDuration);
}
