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
    /// Sets the name to the options
    /// </summary>
    /// <param name="name">name to set</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithName(string name) => this with { Name = name };

    /// <summary>
    /// Optional startup class to use to build the test server
    /// </summary>
    public Type? StartupClass { get; init; }

    /// <summary>
    /// Sets the startup class to the options
    /// </summary>
    /// <typeparam name="TStartup">startup class</typeparam>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithStartup<TStartup>() where TStartup : IStartup =>
        this with
        {
            StartupClass = typeof(TStartup)
        };

    /// <summary>
    /// Sets the startup class to the options
    /// </summary>
    /// <param name="startupType">startup class</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithStartup(Type startupType) =>
        this with
        {
            StartupClass = startupType
        };

    /// <summary>
    /// Optional environment name to use with the test server
    /// </summary>
    public string EnvironmentName { get; init; }

    /// <summary>
    /// Sets the environment name to the options
    /// </summary>
    /// <param name="name">environment name</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithEnvironmentName(string name) =>
        this with
        {
            EnvironmentName = name
        };

    /// <summary>
    /// Optional configuration for the services
    /// </summary>
    public Action<WebHostBuilderContext, IServiceCollection>? ConfigureServices { get; init; }

    /// <summary>
    /// Configures services in the options
    /// </summary>
    /// <param name="config">action</param>
    /// <param name="replace">flag indicates the config should be replaced, not extended</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithServices(
        Action<WebHostBuilderContext, IServiceCollection> config,
        bool replace = false
    ) =>
        this with
        {
            ConfigureServices = (context, collection) =>
            {
                if (!replace)
                    ConfigureServices?.Invoke(context, collection);
                config(context, collection);
            }
        };

    /// <summary>
    /// Configures services in the options
    /// </summary>
    /// <param name="config">action</param>
    /// <param name="replace">flag indicates the config should be replaced, not extended</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithServices(
        Action<IServiceCollection> config,
        bool replace = false
    ) => WithServices((_, collection) => config(collection), replace);

    /// <summary>
    /// Optional configuration for the app configurations
    /// </summary>
    public Action<
        WebHostBuilderContext,
        IConfigurationBuilder
    >? ConfigureAppConfiguration { get; init; }

    /// <summary>
    /// Configures config in the options
    /// </summary>
    /// <param name="config">action</param>
    /// <param name="replace">flag indicates the config should be replaced, not extended</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithConfig(
        Action<WebHostBuilderContext, IConfigurationBuilder> config,
        bool replace = false
    ) =>
        this with
        {
            ConfigureAppConfiguration = (context, collection) =>
            {
                if (!replace)
                    ConfigureAppConfiguration?.Invoke(context, collection);
                config(context, collection);
            }
        };

    /// <summary>
    /// Configures config in the options
    /// </summary>
    /// <param name="config">action</param>
    /// <param name="replace">flag indicates the config should be replaced, not extended</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithConfig(
        Action<IConfigurationBuilder> config,
        bool replace = false
    ) => WithConfig((_, builder) => config(builder), replace);

    /// <summary>
    /// Optional configuration for the host
    /// </summary>
    public Action<IWebHostBuilder>? ConfigureHost { get; init; }

    /// <summary>
    /// Configures host in the options
    /// </summary>
    /// <param name="config">action</param>
    /// <param name="replace">flag indicates the config should be replaced, not extended</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithHost(
        Action<IWebHostBuilder> config,
        bool replace = false
    ) =>
        this with
        {
            ConfigureHost = host =>
            {
                if (!replace)
                    ConfigureHost?.Invoke(host);
                config(host);
            }
        };

    /// <summary>
    /// Optional overrides for app settings
    /// </summary>
    public IDictionary<string, string?>? AppSettingsToOverride { get; init; }

    /// <summary>
    /// Adds a setting to the options
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="value">value</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithSetting(string name, string? value)
    {
        var settings =
            AppSettingsToOverride ?? new Dictionary<string, string?>(StringComparer.Ordinal);
        settings.Add(name, value);
        return this with { AppSettingsToOverride = settings };
    }

    /// <summary>
    /// Issuer for the test JWT tokens
    /// </summary>
    public string Issuer { get; init; }

    /// <summary>
    /// Sets the issuer in the options
    /// </summary>
    /// <param name="issuer">issuer</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithIssuer(string issuer) => this with { Issuer = issuer };

    /// <summary>
    /// Test signing key to use with test JWT tokens
    /// </summary>
    public string SigningKey { get; init; }

    /// <summary>
    /// Sets the signing key in the options
    /// </summary>
    /// <param name="key">key</param>
    /// <returns>options</returns>
    public TestServerBuilderOptions WithSigningKey(string key) => this with { SigningKey = key };

    internal TestServerBuilderOptions(
        string name,
        Type? startupClass = default,
        string environmentName = Constants.TestEnvironmentName,
        Action<WebHostBuilderContext, IServiceCollection>? configureServices = default,
        Action<WebHostBuilderContext, IConfigurationBuilder>? configureAppConfiguration = default,
        Action<IWebHostBuilder>? configureHost = default,
        IDictionary<string, string?>? appSettingsToOverride = default,
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
        string? name = default,
        Type? startupClass = default,
        string environmentName = Constants.TestEnvironmentName,
        Action<WebHostBuilderContext, IServiceCollection>? configureServices = default,
        Action<WebHostBuilderContext, IConfigurationBuilder>? configureAppConfiguration = default,
        Action<IWebHostBuilder>? configureHost = default,
        IDictionary<string, string?>? appSettingsToOverride = default,
        string issuer = Constants.TestIssuer,
        string signingKey = Constants.TestSigningKey
    ) =>
        new(
            name ?? string.Empty,
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
        IDictionary<string, string?>? appSettingsToOverride = default,
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
