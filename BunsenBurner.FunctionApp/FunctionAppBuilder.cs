using BunsenBurner.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace BunsenBurner.FunctionApp;

/// <summary>
/// Provides a cache/builder for function app classes
/// </summary>
public static class FunctionAppBuilder
{
    private static readonly Cache<IHost> HostCache = Cache.New<IHost>();

    /// <summary>
    /// Creates a new instance of the function app from the provided startup class
    /// </summary>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TFunction">function app</typeparam>
    /// <returns>instance of the function app</returns>
    public static TFunction Create<TStartup, TFunction>()
        where TStartup : FunctionsStartup, new()
        where TFunction : class
    {
        var startupType = typeof(TStartup);
        var functionType = typeof(TFunction);
        return HostCache
            .Get(
                $"{startupType.FullName ?? startupType.Name}_{functionType.FullName ?? functionType.Name}",
                _ =>
                    new HostBuilder()
                        .ConfigureWebJobs(new TStartup().Configure)
                        .ConfigureServices(services => services.TryAddSingleton<TFunction>())
                        .Build()
            )
            .Services.GetRequiredService<TFunction>();
    }
}
