![FluentTests](logo.jpg?raw=true "FluentTests.NET")

Fluent test builder for DotNET.

* Test framework agnostic
* Zero dependencies
* Easy to use and extend

```c#
// can use implicit usings
using Xunit;
using static Dsl.Shared;
using static Dsl.Aaa;

namespace SomeNamespace;

public static class Tests
{
    [Fact(DisplayName = "Example AAA test!!!")]
    public static async Task SomeTest() =>
        await Arrange(() => new new SUT(...))
             .Act(async sut => await sut.SomeMethod(...))
             .Assert(result => Assert.Equal("should be this", result));
}
```

## Getting Started

Coming Soon.

## Why?

Most tests in the DotNET ecosystem are written in an arrange, act, assert style, like so,

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

* Enforces that all tests must be arranged before acting and acted upon before assertions can occur
* Converts tests to data, which can be composed and built up then executed
  * Works well wth theories
* Because the fluent tests are just data, functions can be used to extend them and compose them together
  * Works will with dotnet extension methods and other test libraries

```c#
// can use implicit usings
using Xunit;
using static Dsl.Shared;
using static Dsl.Aaa;

namespace SomeNamespace;

public static class Tests
{
    [Fact(DisplayName = "Example AAA test!!!")]
    public static async Task SomeTest() =>
              // arrange starts a new test, whatever type it returns can be used when acting 
        await Arrange(() => new new SUT(...))
              // act on the arranged data, async is supported in all test steps
             .Act(async sut => await sut.SomeMethod(...))
              // assert against the result of acting
             .Assert(result => Assert.Equal("should be this", result));
}
```

More details to come...
