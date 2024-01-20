<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner TestServer Builder

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.TestServerBuilder)](https://www.nuget.org/packages/BunsenBurner.TestServerBuilder/)

## Getting Started

To use this library, simply include `BunsenBurner.TestServerBuilder.dll` in your
project or grab it
from [NuGet](https://www.nuget.org/packages/BunsenBurner.TestServerBuilder/),
and add this to the top of each test `.cs` file that needs it:

```C#
using BunsenBurner;
```

## What?

This provides a set of utility functions exposed by `TestServerBuilder` to
create `TestServer` instances for use in testings APIs.

Its harder than you think to get a working TestServer; this takes the pain
out of it and lets you get back to writing tests.

```c#
TestServerBuilderOptions options = 
    TestServerBuilderOptions
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
```

For more examples check out the test project, create an issue or start a
discussion.
