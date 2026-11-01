![Bunsen Burner](https://raw.githubusercontent.com/bmazzarol/Bunsen-Burner/main/fire-icon-small.png)

# Bunsen Burner

**Arrange, act and assert, enforced by the compiler.**

A test DSL for C# that turns arrange-act-assert into a composable pipeline, and disposes everything the arrange and act stages create.

[![Nuget](https://img.shields.io/nuget/v/BunsenBurner)](https://www.nuget.org/packages/BunsenBurner/)

```shell
dotnet add package BunsenBurner
```

[Getting started](https://bmazzarol.github.io/Bunsen-Burner/articles/getting-started.html) - [Full documentation](https://bmazzarol.github.io/Bunsen-Burner/)

## Getting started

Add the package, then import the DSL in each test file that needs it:

```csharp
// for AAA style tests
using static BunsenBurner.ArrangeActAssert;
```

or

```csharp
// for BDD style tests
using static BunsenBurner.GivenWhenThen;
```

> [!NOTE]
> Tests must return a `Task` to be compatible with the DSL.

Take a test you already have,

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

and write it as a pipeline,

```csharp
[Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
public Task ExampleTest() =>
    Arrange(() =>
    {
        var widget = new { Name = "Widget1", Cost = 12.50 };
        var ms = new MemoryStream();
        return (widget, ms);
    })
    .Act(async data =>
    {
        await JsonSerializer.SerializeAsync(data.ms, data.widget);
        return Encoding.UTF8.GetString(data.ms.ToArray());
    })
    .Assert(result =>
        Assert.Equal(expected: "{\"Name\":\"Widget1\",\"Cost\":12.5}", actual: result)
    );
```

The `MemoryStream` is disposed by the test, whether the assertion passes or not.

Because each stage is a delegate, shared code lifts out of the test into a
reusable helper:

```csharp
public Task ExampleTest() =>
    Arrange(() => new { Name = "Widget1", Cost = 12.50 })
        .Act(CallSerializeAsync)     // a shared stage, used by many tests
        .Assert(result => result == "{\"Name\":\"Widget1\",\"Cost\":12.5}");

private static async Task<string> CallSerializeAsync<T>((T, MemoryStream) data)
{
    var (widget, ms) = data;
    await JsonSerializer.SerializeAsync(ms, widget);
    return Encoding.UTF8.GetString(ms.ToArray());
}
```

Arranging, acting and asserting in the wrong order does not compile. That is the
point of the DSL.

## What it does not do

- It is not a test runner. Tests are still `xUnit` `Fact`s, so the runner,
  reporting and CI stay the same
- It does not execute anything. Each stage is a delegate the test itself calls
- Tests that do not return a `Task` are not compatible with the DSL
- It does not keep disposables alive. Anything disposable created in the arrange
  or act stage is disposed, unless a stage calls `NoDisposal`

## See also

- [Getting started](https://bmazzarol.github.io/Bunsen-Burner/articles/getting-started.html)
- [Arrange act assert](https://bmazzarol.github.io/Bunsen-Burner/articles/arrange-act-assert.html)
- [Auto disposal](https://bmazzarol.github.io/Bunsen-Burner/articles/auto-disposal.html)
