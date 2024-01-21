using BunsenBurner.Logging;
using BunsenBurner.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace BunsenBurner.Tests;

public class TestServerBuilderOptionsTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TestServerBuilderOptionsTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact(DisplayName = "Tests for the build methods")]
    public void Case1()
    {
        #region Example1

        // create an options instance to configure
        var options = new TestServerBuilder.Options
        {
            // issuer for all JWT tokens sent to the test server
            // defaults to `https://localhost/dev/`
            Issuer = TestServerConstants.Issuer,
            // name of the environment
            // defaults to `testing`
            Environment = TestServerConstants.EnvironmentName,
            // provided signing key
            // defaults to `SECRET_SIGNING_KEY`
            SigningKey = TestServerConstants.SigningKey,
            // some logging sink, such as ITestOutputHelper
            Sink = Sink.New(_testOutputHelper.WriteLine),
            // start-up class to use
            Startup = typeof(Startup),
            // configure the configuration used
            // it will automatically include a `appsettings.{environment}.json` file
            ConfigureConfiguration = (
                WebHostBuilderContext context,
                IConfigurationBuilder builder
            ) =>
            {
                builder.Properties.Add("b", 2);
                builder.AddInMemoryCollection(
                    new[] { KeyValuePair.Create<string, string?>("MySetting", "1") }
                );
            },
            // services can be added and replaced here
            ConfigureServices = (IServiceCollection services) =>
            {
                services.AddSingleton(string.Empty);
            },
            // any host settings that you have can be set here
            ConfigureHost = (IWebHostBuilder builder) => {
                // something to do on a IWebHostBuilder
            }
        };
        // build the options into a test server
        TestServer testServer = options.Build();
        // or pass the options to the builder
        TestServer testServer2 = TestServerBuilder.Create(options);

        #endregion

        Assert.NotNull(testServer);
        Assert.NotNull(testServer2);
    }

    #region Example2

    // shared instance of the test server can be stored in the once type
    private readonly Once<TestServer> _sharedTestServer = Once.New(
        () => new TestServerBuilder.Options { Startup = typeof(Startup) }.Build()
    );

    [Fact(DisplayName = "Once can be used to create a test server once and share it")]
    public async Task Case2()
    {
        // get the server, we create it only once the first time
        TestServer server = await _sharedTestServer;
        // second call gets the same instance
        TestServer server2 = await _sharedTestServer;
        Assert.Same(server, server2);
    }

    #endregion

    #region Example3

    // shared named instances of the test server can be stored in the cache type
    private readonly Cache<TestServer> _sharedTestServerCache = Cache.New<TestServer>();

    [Fact(DisplayName = "Cache can be used to create a named test server once and share it")]
    public void Case3()
    {
        // get the server, we create it only once the first time with a given name
        TestServer server = _sharedTestServerCache.Get(
            nameof(server),
            _ => new TestServerBuilder.Options { Startup = typeof(Startup) }.Build()
        );
        // second call gets the same instance
        TestServer server2 = _sharedTestServerCache.Get(
            nameof(server),
            _ => new TestServerBuilder.Options { Startup = typeof(Startup) }.Build()
        );
        Assert.Same(server, server2);
        // but another call with a new key gets a new instance
        // second call gets the same instance
        TestServer server3 = _sharedTestServerCache.Get(
            nameof(server3),
            _ => new TestServerBuilder.Options { Startup = typeof(Startup) }.Build()
        );
        Assert.NotSame(server, server3);
        Assert.NotSame(server2, server3);
    }

    #endregion
}
