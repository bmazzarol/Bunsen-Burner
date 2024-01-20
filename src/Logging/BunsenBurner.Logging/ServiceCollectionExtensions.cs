using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Extension methods for working with <see cref="IServiceCollection"/> and the <see cref="DummyLogger{T}"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="DummyLoggerProvider"/> to the list of <see cref="ILoggerProvider"/>
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="store">log messages store</param>
    /// <param name="sink">custom sink</param>
    /// <returns>services</returns>
    public static IServiceCollection AddDummyLogger(
        this IServiceCollection services,
        LogMessageStore store,
        Sink? sink = default
    ) => services.AddLogging(options => options.AddProvider(new DummyLoggerProvider(store, sink)));

    /// <summary>
    /// Clears existing <see cref="ILoggerProvider"/> registrations
    /// </summary>
    /// <param name="services">services</param>
    /// <returns>services</returns>
    public static IServiceCollection ClearLoggingProviders(this IServiceCollection services) =>
        services.AddLogging(options => options.ClearProviders());
}
