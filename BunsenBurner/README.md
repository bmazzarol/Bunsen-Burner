<!-- markdownlint-disable MD013 -->

# ![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png) Bunsen Burner

<!-- markdownlint-enable MD013 -->

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner)](https://www.nuget.org/packages/BunsenBurner/)

Test framework-agnostic, zero dependencies, easy to use DSL, more maintainable,
easier to refactor. Set :fire: to your old tests! A better way to write tests
:test_tube: in C#.

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

Take an existing test,

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

and convert it to this,

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

This formalizes the structure and then opens up refactoring possibilities
such as pulling out a shared act method,

```csharp
[Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
public Task ExampleTestUsingAaaBuilder2() =>
    Arrange(() =>
    {
        var widget = new { Name = "Widget1", Cost = 12.50 };
        var ms = new MemoryStream();
        return (widget, ms);
    })
    // pull out the shared test code
    .Act(CallSerializeAsync)
    .Assert(result =>
        Assert.Equal(
            expected: "{\"Name\":\"Widget1\",\"Cost\":12.5}", 
            actual: result)
    );

// this is now a shared stage and can be used in many tests
private static async Task<string> CallSerializeAsync<T>((T, MemoryStream) data)
{
    var (widget, ms) = data;
    await JsonSerializer.SerializeAsync(ms, widget);
    return Encoding.UTF8.GetString(ms.ToArray());
}
```

The principle is simple, refine the test structure using a DSL
then use the new structure to identify common code and DRY that code up.

For more information,
see [Bunsen Burner](https://bmazzarol.github.io/Bunsen-Burner).
