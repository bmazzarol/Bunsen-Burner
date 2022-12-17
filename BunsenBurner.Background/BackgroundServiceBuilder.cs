using BunsenBurner.Logging;
using BunsenBurner.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BunsenBurner.Background;

/// <summary>
/// Provides a cache/builder for background services
/// </summary>
public static class BackgroundServiceBuilder
{
    private static Cache<IServiceProvider> ServiceProviderCache => Cache.New<IServiceProvider>();

    private static IServiceProvider BuildServiceProvider<TStartup>(
        Func<TStartup> startupBuilder,
        Action<IServiceCollection>? config = default,
        Sink? sink = default
    )
    {
        var store = LogMessageStore.New();
        var configureServicesMethod = typeof(TStartup).GetMethod("ConfigureServices");
        var sc = new ServiceCollection();
        var startup = startupBuilder();
        configureServicesMethod?.Invoke(startup, new object[] { sc });
        config?.Invoke(sc);
        sc.ClearLoggingProviders().AddDummyLogger(store, sink).AddSingleton(store);
        return sc.BuildServiceProvider();
    }

    /// <summary>
    /// Creates a new instance of the background service and log message store from the provided startup class
    /// </summary>
    /// <param name="startupBuilder">builder for the startup class</param>
    /// <param name="config">optional service collection configuration</param>
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service</typeparam>
    /// <returns>instance of the background service and log message store</returns>
    public static BackgroundServiceContext<TBackgroundService> Create<TStartup, TBackgroundService>(
        Func<TStartup> startupBuilder,
        Action<IServiceCollection>? config = default,
        Sink? sink = default
    ) where TBackgroundService : IHostedService
    {
        var sp = BuildServiceProvider(startupBuilder, config, sink);
        var services = sp.GetRequiredService<IEnumerable<IHostedService>>();
        var store = sp.GetRequiredService<LogMessageStore>();
        store.Clear();
        return new BackgroundServiceContext<TBackgroundService>(
            services.OfType<TBackgroundService>().First(),
            store
        );
    }

    /// <summary>
    /// Creates a new instance of the background service and log message store from the provided startup class
    /// </summary>
    /// <param name="config">optional service collection configuration</param>
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service</typeparam>
    /// <returns>instance of the background service and log message store</returns>
    public static BackgroundServiceContext<TBackgroundService> Create<TStartup, TBackgroundService>(
        Action<IServiceCollection>? config = default,
        Sink? sink = default
    )
        where TStartup : new()
        where TBackgroundService : IHostedService =>
        Create<TStartup, TBackgroundService>(() => new TStartup(), config, sink);

    /// <summary>
    /// Creates a new instance of the background service and log message store from the provided startup class
    /// </summary>
    /// <param name="startupBuilder">builder for the startup class</param>
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service</typeparam>
    /// <returns>instance of the background service and log message store</returns>
    public static BackgroundServiceContext<TBackgroundService> CreateAndCache<
        TStartup,
        TBackgroundService
    >(Func<TStartup> startupBuilder, Sink? sink = default) where TBackgroundService : IHostedService
    {
        var startupType = typeof(TStartup);
        var sp = ServiceProviderCache.Get(
            startupType.FullName ?? startupType.Name,
            _ => BuildServiceProvider(startupBuilder, sink: sink)
        );
        var services = sp.GetRequiredService<IEnumerable<IHostedService>>();
        var store = sp.GetRequiredService<LogMessageStore>();
        store.Clear();
        return new BackgroundServiceContext<TBackgroundService>(
            services.OfType<TBackgroundService>().First(),
            store
        );
    }

    /// <summary>
    /// Creates a new instance of the background service and log message store from the provided startup class
    /// </summary>
    /// <param name="sink">optional messages sink</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service</typeparam>
    /// <returns>instance of the background service and log message store</returns>
    public static BackgroundServiceContext<TBackgroundService> CreateAndCache<
        TStartup,
        TBackgroundService
    >(Sink? sink = default)
        where TStartup : new()
        where TBackgroundService : IHostedService =>
        CreateAndCache<TStartup, TBackgroundService>(() => new TStartup(), sink);
}
