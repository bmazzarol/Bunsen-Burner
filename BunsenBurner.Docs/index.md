<!-- markdownlint-disable MD033 MD041 -->
<div align="center">

<img src="images/fire-icon.png" alt="Bunsen Burner" width="150px"/>

# Bunsen Burner

---

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner)](https://www.nuget.org/packages/BunsenBurner/)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=bmazzarol_Bunsen-Burner&metric=coverage)](https://sonarcloud.io/summary/new_code?id=bmazzarol_Bunsen-Burner)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=bmazzarol_Bunsen-Burner&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=bmazzarol_Bunsen-Burner)
[![CD Build](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/cd-build.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/cd-build.yml)
[![Check Markdown](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/check-markdown.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/check-markdown.yml)
[![CodeQL](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/codeql.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/codeql.yml)

Set :fire: to your old unit tests!
A better way to write tests in C#.

---

</div>

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
              // arrange starts a new test, 
              // whatever type it returns can be used when acting 
        await Arrange(() => new SUT(...))
              // act on the arranged data, async is supported in all test steps
             .Act(async sut => await sut.SomeMethod(...))
              // assert against the result of acting
             .Assert(result => Assert.Equal("should be this", result));
}
```
