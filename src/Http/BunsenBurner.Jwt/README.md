<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner JWT

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Jwt)](https://www.nuget.org/packages/BunsenBurner.Jwt/)

## Getting Started

To use this library, simply include `BunsenBurner.Jwt.dll` in your
project or grab it
from [NuGet](https://www.nuget.org/packages/BunsenBurner.Jwt/),
and add this to the top of each test `.cs` file that needs it:

```C#
using BunsenBurner.Jwt;
```

## What?

This provides a set of way to build test 
[JWT tokens](https://en.wikipedia.org/wiki/JSON_Web_Token)

```c#
[Fact(DisplayName = "An authorized endpoint can be called with a test token")]
public static async Task Case1() =>
    await
    // create a request 
    // (using Http-BuildR https://github.com/bmazzarol/Http-BuildR)
    Req.Get.To("/api/test")
        // now add the test token
        .WithTestBearerToken(
            // pass it a shared signing key, some shared string
            Token.New(SigningKey)
                // now headers and claims can be added
                .WithClaim(ClaimName.Issuer, Issuer)
                .WithHeader(
                    HeaderName.ContentType, 
                    MediaTypeNames.Application.Json)
        )
        // rest is standard...
        .ArrangeData()
        .Act(
            new TestServerBuilder.Options { 
                    Startup = typeof(TestStartupWithAuth) 
                }
                .Build()
                .CallTestServer()
        )
        .Assert(ctx => ctx.Response.IsSuccessStatusCode);
```

Token can also be used standalone with `HttpRequestMessage`,

```c#
// build the token as standard
Token token = Token
    .New("SECRET")
    .WithClaim(ClaimName.Issuer, "https://some-test/");
// now encode it to a string
string rawJwt = token.Encode();
// and it can be used with anything, such has a HttpRequestMessage
HttpRequestMessage request = new HttpRequestMessage();
request.Headers.Add("Authorization", $"Bearer {token.Encode()}");
```

For more examples check out the test project, create an issue or start a
discussion.
