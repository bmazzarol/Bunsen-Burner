using BunsenBurner.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BunsenBurner.Xunit;

/// <summary>
/// Extension methods for working with DI and the dummy logger
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the dummy logger to the list of log providers
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="store">log messages store</param>
    /// <param name="outputHelper">test output helper to log against</param>
    /// <param name="formatter">optional formatter for the log messages</param>
    /// <returns>services</returns>
    public static IServiceCollection AddDummyLogger(
        this IServiceCollection services,
        LogMessageStore store,
        ITestOutputHelper outputHelper,
        Func<LogMessage, string>? formatter = null
    ) =>
        services.AddLogging(
            options =>
                options.AddProvider(
                    new DummyLoggerProvider(store, Sink.New(outputHelper.WriteLine, formatter))
                )
        );
}
