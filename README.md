<!-- markdownlint-disable MD033 MD041 -->
<div align="center">

<img src="fire-icon.png" alt="Bunsen Burner" width="150px"/>

# Bunsen Burner

[:running: **_Getting Started_**](https://bmazzarol.github.io/Bunsen-Burner/articles/getting-started.html)
[:books: **_Documentation_**](https://bmazzarol.github.io/Bunsen-Burner)

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner)](https://www.nuget.org/packages/BunsenBurner/)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=bmazzarol_Bunsen-Burner&metric=coverage)](https://sonarcloud.io/summary/new_code?id=bmazzarol_Bunsen-Burner)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=bmazzarol_Bunsen-Burner&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=bmazzarol_Bunsen-Burner)
[![CD Build](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/cd-build.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/cd-build.yml)
[![Check Markdown](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/check-markdown.yml/badge.svg)](https://github.com/bmazzarol/Bunsen-Burner/actions/workflows/check-markdown.yml)

Set :fire: to your old tests!
A better way to write tests :test_tube: in C#.

</div>

## Features

* Test framework-agnostic
* Zero dependencies
* Easy to use DSL
* More maintainable
* Easier to refactor

Convert semantically equivalent tests from this,

```csharp
[Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
public async Task ExampleTest()
{
    // Arrange
    var widget = new { Name = "Widget1", Cost = 12.50 };
    var ms = new MemoryStream();

    // Act
    await JsonSerializer.SerializeAsync(ms, widget);

    // Assert
    Assert.Equal(
        expected: "{\"Name\":\"Widget1\",\"Cost\":12.5}",
        actual: Encoding.UTF8.GetString(ms.ToArray())
    );
}
```

to this,

```csharp
[Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
public Task ExampleTest() =>
     Arrange(() => new { Name = "Widget1", Cost = 12.50 })
    .Act(async widget =>
    {
        var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, widget);
        return Encoding.UTF8.GetString(ms.ToArray());
    })
    .Assert(result => result == "{\"Name\":\"Widget1\",\"Cost\":12.5}");
```

## Getting Started

To use this library, simply include `BunsenBurner.dll` in your project or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner/), and add this to
the top of each test `.cs` file
that needs it:

```C#
// for AAA style tests
using static BunsenBurner.ArrangeActAssert;
```

or

```C#
// for BDD style tests
using static BunsenBurner.GivenWhenThen;
```

> [!NOTE]
> Tests must always return a `Task` to be compatible with the DSL.

Then use the DSL to refactor your tests.

For more information, see
the [documentation](https://bmazzarol.github.io/Bunsen-Burner).

## Why?

Most tests in C# are written in an arrange, act, assert style.
This is a good pattern, but it is only a convention.

This library aims to formalize this structure in the following ways,

* Enforces that all tests must be arranged before acting and acted upon before
  assertions can occur. Making it a compile
  time error if you don't follow this pattern
* Scaffolding tests using a fluent API making them easier to read, write and
  refactor
* Encourages automatic refactoring of tests sections into helper methods, which
  is only possible if the test is structured using delegates
* Works with the developers IDE to provide a better experience when writing
  tests

For more information have a look the
[documentation](https://bmazzarol.github.io/Bunsen-Burner), or check out the
test project or create an issue.

## Attributions

[Fire icons created by juicy_fish](https://www.flaticon.com/free-icons/fire)
