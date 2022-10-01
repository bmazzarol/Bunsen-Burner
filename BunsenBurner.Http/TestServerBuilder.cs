using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using BunsenBurner.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Http;

/// <summary>
/// Provides a opinionated builder for test servers
/// </summary>
public static class TestServerBuilder
{
#pragma warning disable S3963
    [ExcludeFromCodeCoverage]
    static TestServerBuilder() =>
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            foreach (var (_, server) in TestServerCache)
            {
                if (server.IsValueCreated)
                    server.Value.Dispose();
            }

            TestServerCache.Clear();
        };
#pragma warning restore S3963

    private static readonly ConcurrentDictionary<string, Lazy<TestServer>> TestServerCache =
        new(StringComparer.Ordinal);

    /// <summary>
    /// Creates a new test service instance
    /// </summary>
    /// <remarks>
    /// Does the following,
    ///
    /// * Sets the environment name
    /// * Removes all background services
    /// * Replaces all loggers with the dummy logger
    /// * Enables both Synchronous IO and Preserve execution context
    /// * Wires up an appsettings.{testing-env-name}.json files
    /// * Runs all delegates and replacements
    /// </remarks>
    /// <param name="name">optional name to post-append to the startup class name, used as the cache key</param>
    /// <param name="environmentName">optional environment name to use, default Constants.TestEnvironmentName</param>
    /// <param name="configureServices">optional action to configure services</param>
    /// <param name="configureAppConfiguration">optional action to configure configuration</param>
    /// <param name="configureHost">optional action to configure host</param>
    /// <param name="appSettingsToOverride">optional app settings to override</param>
    /// <typeparam name="TStartup">valid startup class</typeparam>
    /// <returns>test server</returns>
    public static TestServer Create<TStartup>(
        string? name = default,
        string environmentName = Constants.TestEnvironmentName,
        Action<WebHostBuilderContext, IServiceCollection>? configureServices = default,
        Action<WebHostBuilderContext, IConfigurationBuilder>? configureAppConfiguration = default,
        Action<IWebHostBuilder>? configureHost = default,
        IDictionary<string, string>? appSettingsToOverride = default
    ) where TStartup : IStartup
    {
        var type = typeof(TStartup);
        return Create(
            $"{type.FullName ?? type.Name}{name}",
            type,
            environmentName,
            configureServices,
            configureAppConfiguration,
            configureHost,
            appSettingsToOverride
        );
    }

    /// <summary>
    /// Creates a new test service instance
    /// </summary>
    /// <remarks>
    /// Does the following,
    ///
    /// * Sets the environment name
    /// * Removes all background services
    /// * Replaces all loggers with the dummy logger
    /// * Enables both Synchronous IO and Preserve execution context
    /// * Wires up an appsettings.{testing-env-name}.json files
    /// * Runs all delegates and replacements
    /// </remarks>
    /// <param name="name">name to cache against</param>
    /// <param name="startupClass">optional startup class to use</param>
    /// <param name="environmentName">optional environment name to use, default Constants.TestEnvironmentName</param>
    /// <param name="configureServices">optional action to configure services</param>
    /// <param name="configureAppConfiguration">optional action to configure configuration</param>
    /// <param name="configureHost">optional action to configure host</param>
    /// <param name="appSettingsToOverride">optional app settings to override</param>
    /// <returns>test server</returns>
    public static TestServer Create(
        string name,
        Type? startupClass = default,
        string environmentName = Constants.TestEnvironmentName,
        Action<WebHostBuilderContext, IServiceCollection>? configureServices = default,
        Action<WebHostBuilderContext, IConfigurationBuilder>? configureAppConfiguration = default,
        Action<IWebHostBuilder>? configureHost = default,
        IDictionary<string, string>? appSettingsToOverride = default
    ) =>
        TestServerCache
            .GetOrAdd(
                name,
                static (_, ctx) =>
                    new Lazy<TestServer>(() =>
                    {
                        var builder = new WebHostBuilder();
                        builder.UseEnvironment(ctx.environmentName);
                        if (ctx.startupClass != default)
                            builder.UseStartup(ctx.startupClass);
                        builder
                            .ConfigureServices(
                                (context, services) =>
                                {
                                    ctx.configureServices?.Invoke(context, services);
                                    var store = LogMessageStore.New();
                                    services
                                        // remove any logger factories
                                        .RemoveAll(typeof(ILoggerFactory))
                                        // use dummy logger
                                        .AddSingleton(store)
                                        .ClearLoggingProviders()
                                        .AddDummyLogger(store)
                                        // remove hosted services
                                        .RemoveAll(typeof(IHostedService));
                                }
                            )
                            .ConfigureAppConfiguration(
                                (context, configBuilder) =>
                                {
                                    ctx.configureAppConfiguration?.Invoke(context, configBuilder);
                                    configBuilder
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile(
                                            $"appsettings.{ctx.environmentName}.json",
                                            optional: true,
                                            reloadOnChange: false
                                        )
                                        .AddInMemoryCollection(ctx.appSettingsToOverride);
                                }
                            );
                        ctx.configureHost?.Invoke(builder);
                        var server = new TestServer(builder);
                        // might be required, no harm enabling it for testing
                        server.AllowSynchronousIO = true;
                        // required for all thread local access, such flurl test client
                        server.PreserveExecutionContext = true;
                        return server;
                    }),
                (
                    startupClass,
                    environmentName,
                    configureServices,
                    configureAppConfiguration,
                    configureHost,
                    appSettingsToOverride
                )
            )
            .Value;
}
