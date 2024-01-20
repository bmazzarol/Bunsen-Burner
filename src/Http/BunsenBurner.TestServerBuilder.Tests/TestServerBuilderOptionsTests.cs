using BunsenBurner.Logging;
using Microsoft.AspNetCore.TestHost;
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

        TestServerBuilderOptions options = TestServerBuilderOptions
            .New()
            // issuer for all JWT tokens sent to the test server
            .WithIssuer(
                // defaults to `https://localhost/dev/`
                TestServerConstants.Issuer
            )
            // name of the environment
            .WithEnvironmentName(
                // defaults to `testing`
                TestServerConstants.EnvironmentName
            )
            // provided signing key
            .WithSigningKey(
                // defaults to `SECRET_SIGNING_KEY`
                TestServerConstants.SigningKey
            )
            // some logging sink, such as ITestOutputHelper
            .WithLogMessageSink(Sink.New(_testOutputHelper.WriteLine))
            // some ad-hoc setting
            .WithSetting("MySetting", "1")
            // a start up can be added using generics
            .WithStartup<Startup>()
            // or provided Type
            .WithStartup(typeof(Startup))
            // customize the configuration builder
            // it will automatically include a `appsettings.{environment}.json` file
            .WithConfig(c => c.Properties.Add("b", 2))
            // services can be added and replaced here
            .WithServices(collection => collection.AddSingleton(string.Empty))
            // any host settings that you have can be set here
            .WithHost(c =>
            {
                // something to do on a IWebHostBuilder
            });
        // build the options into a test server
        TestServer testServer = options.Build();
        // or pass the options to the builder
        TestServer testServer2 = TestServerBuilder.Create(options);

        #endregion

        Assert.NotNull(testServer);
        Assert.NotNull(testServer2);
    }
}
