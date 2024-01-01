<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Http

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Http)](https://www.nuget.org/packages/BunsenBurner.Http/)

## Getting Started

To use this library, simply include `BunsenBurner.Http.dll` in your project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.Http/), and add this
to the top of each test `.cs` file
that needs it:

```C#
using BunsenBurner.Http;
// allows for fluent building of requests and responses
using HttpBuildR;
using Req = System.Net.Http.HttpMethod;
using Resp = System.Net.HttpStatusCode;
```

## What?

This provides a set of extensions and types to make testing HTTP based services
easy!

Compose a request like so,

```c#
using HttpBuildR;
using Req = System.Net.Http.HttpMethod;
...
var req = 
    // start with a HTTP method
    Req.Get
    // flurl can be used for the URL composition
    .To("/hello-world".SetQueryParam("a", 1)) // all http verbs as covered
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
   .Assert(ctx => ctx.Response.StatusCode == HttpStatusCode.OK);
```

And HttpClient can be mocked out via a HttpMessageStore

```c#
using HttpBuildR;
using static BunsenBurner.Http.HttpMessageMatchers;
using Req = System.Net.Http.HttpMethod;
using Resp = System.Net.HttpStatusCode;

var store = HttpMessageStore.New();
store.Setup(
    // for a given named client
    "PersonService",
    // matchers can be used and composed to match incomming requests
    HasMethod(HttpMethod.Put).And(HasJsonContent((Person p) => p.Age > 19))),
    // response builder can be provided
    req => Resp.OK.Result(request: req)
                  .WithJsonContent(new { LastUpdatedDate = DateTime.Now })
...
// now a store can be converted to a client, or passed to a DummyFactory
var client = store.CreateClient("PersonService");
// now call the client
var result = await client.SendAsync(Req.Put.To("some-endpoint")
                                           .WithBearerToken(...)
                                           .WithJsonContent(new Person(25)));
// the store records all requests and responses made against it
Assert.True(store.Any(m => m.ClientName == "PersonService"
                        && m.Request.Method == HttpMethod.Put
                        && m.Response.StatusCode == Resp.OK))
```

That's it! Just compose requests and assert against responses.

For more examples check out the test project, create an issue or start a
discussion.
