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

    private static IHost BuildHost<TStartup, TFunction>(
        Action<IServiceCollection>? config = default
    )
        where TStartup : FunctionsStartup, new()
        where TFunction : class =>
        new HostBuilder()
            .ConfigureWebJobs(new TStartup().Configure)
            .ConfigureServices(services =>
            {
                services.TryAddSingleton<TFunction>();
                config?.Invoke(services);
            })
            .Build();

    /// <summary>
    /// Creates a new instance of the function app from the provided startup class
    /// </summary>
    /// <param name="config">optional configuration</param>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TFunction">function app</typeparam>
    /// <returns>instance of the function app</returns>
    public static TFunction Create<TStartup, TFunction>(
        Action<IServiceCollection>? config = default
    )
        where TStartup : FunctionsStartup, new()
        where TFunction : class =>
        BuildHost<TStartup, TFunction>(config).Services.GetRequiredService<TFunction>();

    /// <summary>
    /// Creates a new instance of the function app from the provided startup class
    /// </summary>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <typeparam name="TFunction">function app</typeparam>
    /// <returns>instance of the function app</returns>
    public static TFunction CreateAndCache<TStartup, TFunction>()
        where TStartup : FunctionsStartup, new()
        where TFunction : class
    {
        var startupType = typeof(TStartup);
        var functionType = typeof(TFunction);
        return HostCache
            .Get(
                $"{startupType.FullName ?? startupType.Name}_{functionType.FullName ?? functionType.Name}",
                _ => BuildHost<TStartup, TFunction>()
            )
            .Services.GetRequiredService<TFunction>();
    }
}
