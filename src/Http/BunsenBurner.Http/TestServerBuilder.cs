using System.Text;
using BunsenBurner.Logging;
using BunsenBurner.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace BunsenBurner.Http;

/// <summary>
/// Provides a opinionated builder for test servers
/// </summary>
public static class TestServerBuilder
{
    private static readonly Cache<TestServer> TestServerCache = Cache.New<TestServer>();

    /// <summary>
    /// Creates a Symmetric Security Key from a given string.
    /// The minimum key size is 128 bytes therefore the provided string must be at least 16 characters.
    /// Smaller strings will be padded to the right with spaces.
    /// </summary>
    private static SymmetricSecurityKey AsSymmetricSecurityKey(this string key) =>
        new(Encoding.ASCII.GetBytes(key.PadRight(totalWidth: 512 / 8, paddingChar: '\0')));

    private static IServiceCollection ConfigureTestAuth(
        this IServiceCollection services,
        string issuer,
        string signingKey
    ) =>
        services.PostConfigure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            jwtOptions =>
            {
                var key = signingKey.AsSymmetricSecurityKey();
                var config = new OpenIdConnectConfiguration { Issuer = issuer };
                config.SigningKeys.Add(key);
                jwtOptions.TokenValidationParameters.ValidIssuer = issuer;
                jwtOptions.TokenValidationParameters.IssuerSigningKey = key;
                jwtOptions.ConfigurationManager =
                    new StaticConfigurationManager<OpenIdConnectConfiguration>(config);
            }
        );

    private static IServiceCollection ConfigureTestLogging(
        this IServiceCollection services,
        LogMessageStore store,
        Sink? sink
    ) =>
        services
            // remove any logger factories
            .RemoveAll(typeof(ILoggerFactory))
            // use dummy logger
            .AddSingleton(store)
            .ClearLoggingProviders()
            .AddDummyLogger(store, sink);

    private static IConfigurationBuilder ConfigureTestSettings(
        this IConfigurationBuilder builder,
        string environmentName,
        IDictionary<string, string?>? appSettingsToOverride
    ) =>
        builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                $"appsettings.{environmentName}.json",
                optional: true,
                reloadOnChange: false
            )
            .AddInMemoryCollection(
                appSettingsToOverride?.AsEnumerable()
                    ?? Array.Empty<KeyValuePair<string, string?>>()
            );

    /// <summary>
    /// Creates a new test service instance
    /// </summary>
    /// <remarks>
    /// <para>Does the following,</para>
    /// <para>
    /// * Sets the environment name
    /// * Removes all background services
    /// * Replaces all loggers with the dummy logger
    /// * Replaces the JWT token configuration to allow for test tokens
    /// * Enables both Synchronous IO and Preserve execution context
    /// * Wires up an appsettings.{testing-env-name}.json files
    /// * Runs all delegates and replacements
    /// </para>
    /// </remarks>
    /// <param name="options">options to use when building the test server</param>
    /// <returns>test server</returns>
    public static TestServer Create(TestServerBuilderOptions options)
    {
        var builder = new WebHostBuilder();
        builder.UseEnvironment(options.EnvironmentName);
        if (options.StartupClass != default)
            builder.UseStartup(options.StartupClass);
        builder
            .ConfigureTestServices(services =>
            {
                options.ConfigureServices?.Invoke(services);
                var store = LogMessageStore.New();
                services
                    // setup test loggers
                    .ConfigureTestLogging(store, options.Sink)
                    // remove hosted services
                    .RemoveAll(typeof(IHostedService))
                    // setup test auth
                    .ConfigureTestAuth(options.Issuer, options.SigningKey);
            })
            .ConfigureAppConfiguration(
                (context, configBuilder) =>
                {
                    options.ConfigureAppConfiguration?.Invoke(context, configBuilder);
                    configBuilder.ConfigureTestSettings(
                        options.EnvironmentName,
                        options.AppSettingsToOverride
                    );
                }
            );
        options.ConfigureHost?.Invoke(builder);
        var server = new TestServer(builder);
#if NETCOREAPP3_1_OR_GREATER
        // might be required, no harm enabling it for testing
        server.AllowSynchronousIO = true;
        // required for all thread local access, such flurl test client
        server.PreserveExecutionContext = true;
#endif
        return server;
    }

    /// <summary>
    /// Creates a new test service instance
    /// </summary>
    /// <remarks>
    /// <para>Does the following,</para>
    /// <para>
    /// * Sets the environment name
    /// * Removes all background services
    /// * Replaces all loggers with the dummy logger
    /// * Replaces the JWT token configuration to allow for test tokens
    /// * Enables both Synchronous IO and Preserve execution context
    /// * Wires up an appsettings.{testing-env-name}.json files
    /// * Runs all delegates and replacements
    /// </para>
    /// </remarks>
    /// <param name="options">options to use when building the test server</param>
    /// <returns>test server</returns>
    public static TestServer CreateAndCache(TestServerBuilderOptions options) =>
        TestServerCache.Get(options.Name, _ => Create(options));

    /// <summary>
    /// Creates a new test service instance
    /// </summary>
    /// <remarks>
    /// <para>Does the following,</para>
    /// <para>
    /// * Sets the environment name
    /// * Removes all background services
    /// * Replaces all loggers with the dummy logger
    /// * Replaces the JWT token configuration to allow for test tokens
    /// * Enables both Synchronous IO and Preserve execution context
    /// * Wires up an appsettings.{testing-env-name}.json files
    /// * Runs all delegates and replacements
    /// </para>
    /// </remarks>
    /// <param name="options">options to use when building the test server</param>
    /// <returns>test server</returns>
    public static TestServer Build(this TestServerBuilderOptions options) => Create(options);

    /// <summary>
    /// Creates and caches a new test service instance
    /// </summary>
    /// <remarks>
    /// <para>Does the following,</para>
    /// <para>
    /// * Sets the environment name
    /// * Removes all background services
    /// * Replaces all loggers with the dummy logger
    /// * Replaces the JWT token configuration to allow for test tokens
    /// * Enables both Synchronous IO and Preserve execution context
    /// * Wires up an appsettings.{testing-env-name}.json files
    /// * Runs all delegates and replacements
    /// </para>
    /// </remarks>
    /// <param name="options">options to use when building the test server</param>
    /// <returns>test server</returns>
    public static TestServer BuildAndCache(this TestServerBuilderOptions options) =>
        CreateAndCache(options);
}
