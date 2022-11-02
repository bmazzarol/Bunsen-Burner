using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.Http;

/// <summary>
/// Options for building test servers
/// </summary>
public sealed record TestServerBuilderOptions
{
    /// <summary>
    /// Name of the test server
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Optional startup class to use to build the test server
    /// </summary>
    public Type? StartupClass { get; init; }

    /// <summary>
    /// Optional environment name to use with the test server
    /// </summary>
    public string EnvironmentName { get; init; }

    /// <summary>
    /// Optional configuration for the services
    /// </summary>
    public Action<WebHostBuilderContext, IServiceCollection>? ConfigureServices { get; init; }

    /// <summary>
    /// Optional configuration for the app configurations
    /// </summary>
    public Action<
        WebHostBuilderContext,
        IConfigurationBuilder
    >? ConfigureAppConfiguration { get; init; }

    /// <summary>
    /// Optional configuration for the host
    /// </summary>
    public Action<IWebHostBuilder>? ConfigureHost { get; init; }

    /// <summary>
    /// Optional overrides for app settings
    /// </summary>
    public IDictionary<string, string>? AppSettingsToOverride { get; init; }

    /// <summary>
    /// Issuer for the test JWT tokens
    /// </summary>
    public string Issuer { get; init; }

    /// <summary>
    /// Test signing key to use with test JWT tokens
    /// </summary>
    public string SigningKey { get; init; }

    internal TestServerBuilderOptions(
        string name,
        Type? startupClass = default,
        string environmentName = Constants.TestEnvironmentName,
        Action<WebHostBuilderContext, IServiceCollection>? configureServices = default,
        Action<WebHostBuilderContext, IConfigurationBuilder>? configureAppConfiguration = default,
        Action<IWebHostBuilder>? configureHost = default,
        IDictionary<string, string>? appSettingsToOverride = default,
        string issuer = Constants.TestIssuer,
        string signingKey = Constants.TestSigningKey
    )
    {
        Name = name;
        StartupClass = startupClass;
        EnvironmentName = environmentName;
        ConfigureServices = configureServices;
        ConfigureAppConfiguration = configureAppConfiguration;
        ConfigureHost = configureHost;
        AppSettingsToOverride = appSettingsToOverride;
        Issuer = issuer;
        SigningKey = signingKey;
    }

    /// <summary>
    /// Creates a new test server builder options
    /// </summary>
    /// <param name="name">name to cache against</param>
    /// <param name="startupClass">optional startup class to use</param>
    /// <param name="environmentName">optional environment name to use, default Constants.TestEnvironmentName</param>
    /// <param name="configureServices">optional action to configure services</param>
    /// <param name="configureAppConfiguration">optional action to configure configuration</param>
    /// <param name="configureHost">optional action to configure host</param>
    /// <param name="appSettingsToOverride">optional app settings to override</param>
    /// <param name="issuer">test issuer for the JWT tokens</param>
    /// <param name="signingKey">test signing key for the JWT tokens</param>
    /// <returns>options</returns>
    public static TestServerBuilderOptions New(
        string name,
        Type? startupClass = default,
        string environmentName = Constants.TestEnvironmentName,
        Action<WebHostBuilderContext, IServiceCollection>? configureServices = default,
        Action<WebHostBuilderContext, IConfigurationBuilder>? configureAppConfiguration = default,
        Action<IWebHostBuilder>? configureHost = default,
        IDictionary<string, string>? appSettingsToOverride = default,
        string issuer = Constants.TestIssuer,
        string signingKey = Constants.TestSigningKey
    ) =>
        new(
            name,
            startupClass,
            environmentName,
            configureServices,
            configureAppConfiguration,
            configureHost,
            appSettingsToOverride,
            issuer,
            signingKey
        );

    /// <summary>
    /// Creates a new test server builder options
    /// </summary>
    /// <param name="name">name to cache against</param>
    /// <param name="environmentName">optional environment name to use, default Constants.TestEnvironmentName</param>
    /// <param name="configureServices">optional action to configure services</param>
    /// <param name="configureAppConfiguration">optional action to configure configuration</param>
    /// <param name="configureHost">optional action to configure host</param>
    /// <param name="appSettingsToOverride">optional app settings to override</param>
    /// <param name="issuer">test issuer for the JWT tokens</param>
    /// <param name="signingKey">test signing key for the JWT tokens</param>
    /// <returns>options</returns>
    public static TestServerBuilderOptions New<TStartup>(
        string? name = default,
        string environmentName = Constants.TestEnvironmentName,
        Action<WebHostBuilderContext, IServiceCollection>? configureServices = default,
        Action<WebHostBuilderContext, IConfigurationBuilder>? configureAppConfiguration = default,
        Action<IWebHostBuilder>? configureHost = default,
        IDictionary<string, string>? appSettingsToOverride = default,
        string issuer = Constants.TestIssuer,
        string signingKey = Constants.TestSigningKey
    ) where TStartup : IStartup
    {
        var type = typeof(TStartup);
        return New(
            $"{type.FullName ?? type.Name}{name}",
            type,
            environmentName,
            configureServices,
            configureAppConfiguration,
            configureHost,
            appSettingsToOverride,
            issuer,
            signingKey
        );
    }
}
