# TestServer Builder

Getting a working <xref:Microsoft.AspNetCore.TestHost.TestServer> with,

* Proper [OIDC](https://openid.net/developers/how-connect-works/) based JWT
  authentication
* [Test logging](./test-logger.md)
* Replaced services
* No hosted processes

That just works is not so easy.

So Bunsen Burner provides a utility package to help via
<xref:BunsenBurner.Http.TestServerBuilder> so that you can focus on testing.

To use this library, simply include `BunsenBurner.Http.TestServerBuilder.dll` in your
project or grab it
from [NuGet](https://www.nuget.org/packages/BunsenBurner.Http.TestServerBuilder/),
and add this to the top of each test `.cs` file that needs it:

```C#
using BunsenBurner.Http;
```

To use it do the following,

[!code-csharp[Example1](../../../Http/BunsenBurner.Http.TestServerBuilder.Tests/TestServerBuilderOptionsTests.cs#Example1)]

## Sharing TestServer Instances

There are 2 ways that <xref:Microsoft.AspNetCore.TestHost.TestServer> can be shared
across test runs that is independent from the test framework,

1. <xref:BunsenBurner.Utility.Once`1> - provides a thread safe at most once factory

   [!code-csharp[Example2](../../../Http/BunsenBurner.Http.TestServerBuilder.Tests/TestServerBuilderOptionsTests.cs#Example2)]

2. <xref:BunsenBurner.Utility.Cache`1> - provides named at most once factory methods

   [!code-csharp[Example3](../../../Http/BunsenBurner.Http.TestServerBuilder.Tests/TestServerBuilderOptionsTests.cs#Example3)]
