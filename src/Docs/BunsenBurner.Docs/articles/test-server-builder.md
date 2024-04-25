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
