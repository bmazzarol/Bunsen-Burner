using System.Text;
using BunsenBurner.Logging;
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

namespace BunsenBurner;

/// <summary>
/// Provides a opinionated builder for <see cref="TestServer"/> instances
/// </summary>
public static partial class TestServerBuilder
{
    /// <summary>
    /// Creates a <see cref="SymmetricSecurityKey"/> from a given string.
    /// The minimum key size is 128 bytes therefore the provided string must be at least 16 characters.
    /// Smaller strings will be padded to the right with spaces.
    /// </summary>
    private static SymmetricSecurityKey CreateSymmetricSecurityKey(this string key) =>
        new(Encoding.ASCII.GetBytes(key.PadRight(totalWidth: 512 / 8, paddingChar: '\0')));

    /// <summary>
    /// Configures locally signed JWT authentication on the provided <see cref="IServiceCollection"/>.
    /// It uses a <see cref="StaticConfigurationManager{T}"/> for <see cref="OpenIdConnectConfiguration"/> with
    /// a generated signing key and provided issuer.
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="issuer">issue expected for all JWT</param>
    /// <param name="signingKey">seed for the generated signing key</param>
    /// <returns>services</returns>
    public static IServiceCollection ConfigureTestAuth(
        this IServiceCollection services,
        string issuer,
        string signingKey
    ) =>
        services.PostConfigure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            jwtOptions =>
            {
                var key = signingKey.CreateSymmetricSecurityKey();
                var config = new OpenIdConnectConfiguration { Issuer = issuer };
                config.SigningKeys.Add(key);
                jwtOptions.TokenValidationParameters.ValidIssuer = issuer;
                jwtOptions.TokenValidationParameters.IssuerSigningKey = key;
                jwtOptions.ConfigurationManager =
                    new StaticConfigurationManager<OpenIdConnectConfiguration>(config);
            }
        );

    /// <summary>
    /// Configures a test logger for the provided <see cref="IServiceCollection"/>.
    /// It uses the <see cref="DummyLoggerProvider"/> internally and replaces all other <see cref="ILoggerProvider"/>
    /// instances.
    /// An optional <see cref="Sink"/> can be used to direct logging output to, for example to <see cref="Console.WriteLine()"/>
    /// or a test output helper as provided by xunit.
    /// </summary>
    /// <param name="services">services</param>
    /// <param name="store"><see cref="LogMessageStore"/> to store all log messages in</param>
    /// <param name="sink">optional <see cref="Sink"/> to direct logging output</param>
    /// <returns>services</returns>
    public static IServiceCollection ConfigureTestLogging(
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

    /// <summary>
    /// Configures test settings via a `appsettings.{environmentName}.json` file
    /// </summary>
    /// <param name="builder">configuration builder</param>
    /// <param name="environmentName">environment name</param>
    /// <returns>builder</returns>
    public static IConfigurationBuilder ConfigureTestSettings(
        this IConfigurationBuilder builder,
        string environmentName
    ) =>
        builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                $"appsettings.{environmentName}.json",
                optional: true,
                reloadOnChange: false
            );

    /// <summary>
    /// Creates a new <see cref="TestServer"/> instance from the provided <see cref="TestServerBuilder.Options"/>
    /// </summary>
    /// <remarks>
    /// <para>Does the following,</para>
    /// <para>
    /// * Sets the environment name
    /// * Removes all <see cref="IHostedService"/> instances
    /// * Replaces all <see cref="ILogger{TCategoryName}"/> with <see cref="DummyLogger{T}"/>
    /// * Replaces the JWT token configuration to allow for test tokens
    /// * Enables both Synchronous IO and Preserve execution context for 3.1 (not 6+)
    /// * Wires up an appsettings.{testing-env-name}.json files
    /// * Runs all delegates and replacements
    /// </para>
    /// </remarks>
    /// <param name="options">options to use when building the <see cref="TestServer"/></param>
    /// <returns><see cref="TestServer"/></returns>
    public static TestServer Create(Options options)
    {
        var builder = new WebHostBuilder();
        builder.UseEnvironment(options.Environment);
        if (options.Startup != null)
            builder.UseStartup(options.Startup);
        builder
            .ConfigureTestServices(services =>
            {
                options.ConfigureServices?.Invoke(services);
                services
                    // setup test loggers
                    .ConfigureTestLogging(options.LogMessageStore, options.Sink)
                    // remove hosted services
                    .RemoveAll(typeof(IHostedService))
                    // setup test auth
                    .ConfigureTestAuth(options.Issuer, options.SigningKey);
            })
            .ConfigureAppConfiguration(
                (context, configBuilder) =>
                {
                    options.ConfigureConfiguration?.Invoke(context, configBuilder);
                    configBuilder.ConfigureTestSettings(options.Environment);
                }
            );
        options.ConfigureHost?.Invoke(builder);
        var server = new TestServer(builder);
#if NETCOREAPP3_1_OR_GREATER
        // might be required, no harm enabling it for testing
        server.AllowSynchronousIO = true;
        // required for all thread local access, such as flurl test client
        server.PreserveExecutionContext = true;
#endif
        return server;
    }
}
