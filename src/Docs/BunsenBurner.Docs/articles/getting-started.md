# Getting Started

To use this library, simply include `BunsenBurner.dll` in your project or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner/), and add this to
the top of each test `.cs` file
that needs it:

```C#
using static BunsenBurner.Aaa; // For Arrange act assert
```

or

```C#
using static BunsenBurner.Bdd; // For Given when then
```

Take an existing test,

[!code-csharp[Example1a](../../../Core/BunsenBurner.Tests/Examples/GettingStarted.cs#Example1a)]

This can be converted to use the arrange act assert builder like so,

[!code-csharp[Example1b](../../../Core/BunsenBurner.Tests/Examples/GettingStarted.cs#Example1b)]

This formalizes the structure and then opens up refactoring possibilities
such as pulling out a shared act method,

[!code-csharp[Example1c](../../../Core/BunsenBurner.Tests/Examples/GettingStarted.cs#Example1c)]

The principle is simple, refine the test structure using a DSL
then use the new structure to identify common code and DRY that code up.