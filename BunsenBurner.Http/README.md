# ![](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Http

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Http)](https://www.nuget.org/packages/BunsenBurner.Http/)

## Getting Started

To use this library, simply include `BunsenBurner.Http.dll` in your project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.Http/), and add this
to the top of each test `.cs` file
that needs it:

```C#
using BunsenBurner.Http;
```

## What?

This provides a set of extensions and types to make testing HTTP based services
easy!

Compose a request like so,

``` c#
var req = Request
    // flurl can be used for the URL composition
    .GET("/hello-world".SetQueryParam("a", 1)) // all http verbs as covered
    // the non-url based parts are covered by methods
    // including JWT based auth token construction
    .WithHeader("b", 123, x => x.ToString())
```

Then convert the request to a scenario that can be run against a test or
real server.

The test server builder provides an opinionated way to build test servers.

```c#
req.ArrangeRequest() // convert to a scenario
    // run the request against the test server defined by the Startup class
   .ActAndCall(TestServerBuilderOptions.New<Startup>().Build())
    // a response context contains the http response and all log messages produced
    // while handling the request
   .Assert(ctx => ctx.Response.Code == HttpStatusCode.OK);
```

That's it! Just compose requests and assert against responses.

For more examples check out the test project, create an issue or start a
discussion.
