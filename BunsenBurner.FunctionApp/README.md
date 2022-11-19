# ![](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Function App

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.FunctionApp)](https://www.nuget.org/packages/BunsenBurner.FunctionApp/)

## Getting Started

To use this library, simply include `BunsenBurner.FunctionApp.dll` in your
project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.FunctionApp/), and
add this to the top of each test `.cs` file
that needs it:

```C#
using BunsenBurner.FunctionApp;
```

## What?

This provides a set of extensions and types to make testing function apps
easy!

Arrange the required request data the function app accepts,

``` c#
Arrange(() => 2) // some data required to call the function app
     // this builds the function app from the Startup class
    .AndFunctionApp<int, Startup, Function>()
     // now the function instance is in scope along with the params
     // just call it and return the result
    .ActAndExecute(
        x => x.FunctionApp,
        async (i, function) =>
        {
            // example HTTP trigger, the Bunsen Burner HTTP library can be used here
            var result = await function.SomeFunctionTrigger(
                Request.GET($"/some-path/{i.Data}".SetQueryParam("noBody")).AsHttpRequest()
            );
            // extension methed to get the ObjectResult back into a HTTP Response
            return result.AsResponse();
        }
    )
    // assertions as normal
    .Assert(resp =>
    {
        Assert.Equal(HttpStatusCode.OK, resp.Code);
        Assert.Empty(resp.Content ?? string.Empty);
    });
```

That's it! Just compose parameters and assert against results.

For more examples check out the test project, create an issue or start a
discussion.
