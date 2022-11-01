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

    /// <summary>
    /// Creates a new instance of the background service and log message store from the provided startup class
    /// </summary>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TBackgroundService">background service</typeparam>
    /// <returns>instance of the background service and log message store</returns>
    public static BackgroundServiceContext<TBackgroundService> Create<
        TStartup,
        TBackgroundService
    >()
        where TStartup : new()
        where TBackgroundService : IHostedService
    {
        var startupType = typeof(TStartup);
        var sp = ServiceProviderCache.Get(
            startupType.FullName ?? startupType.Name,
            static _ =>
            {
                var store = LogMessageStore.New();
                var configureServicesMethod = typeof(TStartup).GetMethod("ConfigureServices");
                var sc = new ServiceCollection();
                var startup = new TStartup();
                configureServicesMethod?.Invoke(startup, new object[] { sc });
                sc.ClearLoggingProviders().AddDummyLogger(store).AddSingleton(store);
                return sc.BuildServiceProvider();
            }
        );
        var services = sp.GetRequiredService<IEnumerable<IHostedService>>();
        var store = sp.GetRequiredService<LogMessageStore>();
        store.Clear();
        return new BackgroundServiceContext<TBackgroundService>(
            services.OfType<TBackgroundService>().First(),
            store
        );
    }
}
