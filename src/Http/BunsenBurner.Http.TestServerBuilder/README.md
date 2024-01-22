<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner TestServer Builder

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Http.TestServerBuilder)](https://www.nuget.org/packages/BunsenBurner.Http.TestServerBuilder/)

## Getting Started

To use this library, simply include `BunsenBurner.Http.TestServerBuilder.dll` in your
project or grab it
from [NuGet](https://www.nuget.org/packages/BunsenBurner.Http.TestServerBuilder/),
and add this to the top of each test `.cs` file that needs it:

```C#
using BunsenBurner.Http;
```

## What?

This provides a set of utility functions exposed by `TestServerBuilder` to
create `TestServer` instances for use in testings APIs.

Its harder than you think to get a working TestServer; this takes the pain
out of it and lets you get back to writing tests.

```c#
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
```

For more examples check out the test project, create an issue or start a
discussion.
