<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner Dependency Injection

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner.DependencyInjection)](https://www.nuget.org/packages/BunsenBurner.DependencyInjection/)

This provides extension methods to test that a standard DI container is
configured correctly.

## Getting Started

To use this library, simply include `BunsenBurner.DependencyInjection.dll` in
your projector grab it
from [NuGet](https://www.nuget.org/packages/BunsenBurner.DependencyInjection/),
and add this to the top of each test `.cs` file that needs it:

```C#
using static BunsenBurner.DependencyInjection.Aaa;
```

or

```C#
using static BunsenBurner.DependencyInjection.Bdd;
```

## What?

The standard Dependency Injection container in dotnet is not typesafe and
requires runtime verification to ensure that its configured correctly.

This is a very simple test that builds all services that match a predicate.

There are default predicates that operate on an assembly(s). This will in most
cases do the job.

## How to use

To start using it import the syntax that your test project is using.

```c#
using static BunsenBurner.DependencyInjection.Aaa;

public static class DITests
{
    [Fact(DisplayName = "All services are resolvable in the project assembly")]
    public static async Task Case1() =>
        await new Startup()
            .ArrangeData()
            .ActAndAssertServicesAreConfigured(typeof(Startup).Assembly);
}
```

For more information on the added arrange functions check out the code and the
tests.
