<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Background

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.Background)](https://www.nuget.org/packages/BunsenBurner.Background/)

## Getting Started

To use this library, simply include `BunsenBurner.Background.dll` in your
project
or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner.Background/), and
add this to the top of each test `.cs` file
that needs it:

```C#
using BunsenBurner.Background;
```

## What?

This provides a set of extensions and types to make testing background services
easy!

Arrange the background service and then provide a condition for completion,

``` c#
ArrangeBackgroundService<Startup, Background>() // some background service and Startup
     // run the background service for no longer than 5 minuets
     // stopping as soon as the log message "Work complete" is logged
    .ActAndRunFor(
        5 * minutes,
        context => context.Store.Any(x => x.Message == "Work complete")
    )
    // assertions against the log messages
    .Assert(store =>
    {
        Assert.NotEmpty(store);
        Assert.Contains(store, x => x.Message == "Work complete");
    });
```

That's it! Just compose the background service and exit condition, then assert
against log messages.

For more examples check out the test project, create an issue or start a
discussion.
