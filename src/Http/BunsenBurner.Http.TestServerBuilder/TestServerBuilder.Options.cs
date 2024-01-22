using BunsenBurner.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.Http;

public static partial class TestServerBuilder
{
    /// <summary>
    /// Options used in <see cref="TestServerBuilder.Create"/> to build a <see cref="TestServer"/> instance
    /// </summary>
    public readonly record struct Options
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Options()
        {
            Startup = null;
            Environment = TestServerConstants.EnvironmentName;
            ConfigureServices = null;
            ConfigureConfiguration = null;
            ConfigureHost = null;
            Issuer = TestServerConstants.Issuer;
            SigningKey = TestServerConstants.SigningKey;
            LogMessageStore = LogMessageStore.New();
            Sink = null;
        }

        /// <summary>
        /// Optional startup class to use to build the <see cref="TestServer"/>
        /// </summary>
        public Type? Startup { get; init; }

        /// <summary>
        /// Optional environment name to use with the <see cref="TestServer"/>; defaults to <see cref="TestServerConstants.EnvironmentName"/>
        /// </summary>
        public string Environment { get; init; }

        /// <summary>
        /// Optional configuration for the <see cref="IServiceProvider"/> used by the <see cref="TestServer"/>
        /// </summary>
        public Action<IServiceCollection>? ConfigureServices { get; init; }

        /// <summary>
        /// Optional configuration for the <see cref="IConfigurationBuilder"/> used by the <see cref="TestServer"/>
        /// </summary>
        public Action<
            WebHostBuilderContext,
            IConfigurationBuilder
        >? ConfigureConfiguration { get; init; }

        /// <summary>
        /// Optional configuration for the <see cref="IWebHost"/> used by the <see cref="TestServer"/>
        /// </summary>
        public Action<IWebHostBuilder>? ConfigureHost { get; init; }

        /// <summary>
        /// Issuer for the test JWT tokens accepted by the <see cref="TestServer"/>; defaults to <see cref="TestServerConstants.Issuer"/>
        /// </summary>
        public string Issuer { get; init; }

        /// <summary>
        /// Test signing key to use with test JWT tokens accepted by the <see cref="TestServer"/>; defaults to <see cref="TestServerConstants.SigningKey"/>
        /// </summary>
        public string SigningKey { get; init; }

        /// <summary>
        /// Optional <see cref="LogMessageStore"/> to use with the <see cref="TestServer"/>
        /// </summary>
        public LogMessageStore LogMessageStore { get; init; }

        /// <summary>
        /// Optional log messages <see cref="Sink"/> to log to from the running <see cref="TestServer"/>
        /// </summary>
        public Sink? Sink { get; init; }

        /// <summary>
        /// Builds the <see cref="TestServerBuilder.Options"/> into a <see cref="TestServer"/>
        /// </summary>
        /// <returns><see cref="TestServer"/></returns>
        public TestServer Build() => Create(this);
    }
}
