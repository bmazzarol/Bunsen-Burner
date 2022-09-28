using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the dummy logger to the list of log providers
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="store">log messages store</param>
    /// <returns>services</returns>
    public static IServiceCollection AddDummyLogger(
        this IServiceCollection services,
        LogMessageStore store
    ) => services.AddLogging(options => options.AddProvider(new DummyLoggerProvider(store)));

    /// <summary>
    /// Clears existing logging providers
    /// </summary>
    /// <param name="services">services</param>
    /// <returns>services</returns>
    public static IServiceCollection ClearLoggingProviders(this IServiceCollection services) =>
        services.AddLogging(options => options.ClearProviders());
}
