# :fire: Bunsen Burner :fire:
[![Coverage Status](https://coveralls.io/repos/github/bmazzarol/Bunsen-Burner/badge.svg?branch=main)](https://coveralls.io/github/bmazzarol/Bunsen-Burner?branch=main)
[![CodeQL](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/codeql.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/codeql.yml)

> Set :fire: to your old unit tests!
---

A better way to write tests :test_tube: in C#.

* Test framework agnostic
* Zero dependencies
* Easy to use and extend
* More maintainable

```c#
// Arrange act assert style

using static BunsenBurner.Aaa;

namespace SomeNamespace;

public static class Tests
{
    [Fact(DisplayName = "Example AAA test!!!")]
    public static async Task SomeTest() =>
        await Arrange(() => new SUT(...))
             .Act(async sut => await sut.SomeMethod(...))
             .Assert(result => Assert.Equal("should be this", result));
}

// Given when then style

using static BunsenBurner.Bdd;

namespace SomeNamespace;

public static class Tests
{
    [Fact(DisplayName = "Example BDD test!!!")]
    public static async Task SomeTest() =>
        await Given(() => new SUT(...))
             .When(async sut => await sut.SomeMethod(...))
             .Then(result => Assert.Equal("should be this", result));
}
```

## Getting Started

To use this library, simply include `BunsenBurner.dll` in your project or grab
it from NuGet (Coming Soon), and add this to the top of each test `.cs` file
that needs it:

```C#
using static BunsenBurner.Aaa; // For Arrange act assert
```

or

```C#
using static BunsenBurner.Bdd; // For Given when then
```

Click through to the links bellow for further details.

| Library                                               | Description                                                                                                          | Nu-Get      |
|-------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------|-------------|
| [Core](./BunsenBurner/README.md)                      | Core test abstraction that makes it all possible. This is all that is required to get started!                       | Coming Soon |
| [Logging](./BunsenBurner.Logging/README.md)           | Core logging abstractions. Used to assert against logged messages, useful for cases like testing background services | Coming Soon |
| [AutoFixture](./BunsenBurner.AutoFixture/README.md)   | Integration with [AutoFixture](https://github.com/AutoFixture) to simplify arrange steps                             | Coming Soon |
| [Bogus](./BunsenBurner.Bogus/README.md)               | Integration with [Bogus](https://github.com/bchavez/Bogus) to simplify arrange steps                                 | Coming Soon |
| [Hedgehog](./BunsenBurner.Hedgehog/README.md)         | Integration with [Hedgehog](https://github.com/hedgehogqa/fsharp-hedgehog) to write property based tests             | Coming Soon |
| [Verify.Xunit](./BunsenBurner.Verify.Xunit/README.md) | Integration with [Verify.Xunit](https://github.com/VerifyTests/Verify) to simplify assert steps                      | Coming Soon |
| [Verify.Xunit](./BunsenBurner.Verify.NUnit/README.md) | Integration with [Verify.NUnit](https://github.com/VerifyTests/Verify) to simplify assert steps                      | Coming Soon |
| [Http](./BunsenBurner.Http/README.md)                 | Extension methods for testing Http servers                                                                           | Coming Soon |
| [FunctionApp](./BunsenBurner.FunctionApp/README.md)   | Extension methods for testing Function apps                                                                          | Coming Soon |
| [Background](./BunsenBurner.Background/README.md)     | Extension methods for testing Background services                                                                    | Coming Soon |

## Why?

Most tests in the C# are written in an arrange, act, assert style, like so,

```c#
using Xunit;

namespace SomeNamespace;

public static class Tests
{
    [Fact]
    public static async Task SomeTest()
    {
        // Arrange
        var sut = new SUT(...);
        
        // Act
        var result = await sut.SomeMethod(...);
        
        // Assert
        Assert.Equal("should be this", result);
    }
}
```

This library aims to formalize this structure in the following ways,

* Enforces that all tests must be arranged before acting and acted upon before
  assertions can occur
* Converts tests to data, which can be composed and built up then executed
    * Works well wth theories
* Because tests are just data, functions can be used to extend them and compose
  them together
    * Works will with extension methods and other test libraries, use cases

```c#
// can use implicit usings
using Xunit;
using static BunsenBurner.Aaa;

namespace SomeNamespace;

public static class Tests
{
    [Fact(DisplayName = "Example AAA test!!!")]
    public static async Task SomeTest() =>
              // arrange starts a new test, whatever type it returns can be used when acting 
        await Arrange(() => new SUT(...))
              // act on the arranged data, async is supported in all test steps
             .Act(async sut => await sut.SomeMethod(...))
              // assert against the result of acting
             .Assert(result => Assert.Equal("should be this", result));
}
```

For more details/information have a look the test projects or create an issue.
