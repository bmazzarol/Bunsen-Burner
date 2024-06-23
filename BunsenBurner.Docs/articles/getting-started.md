# Getting Started

To use this library, simply include `BunsenBurner.dll` in your project or grab
it from [NuGet](https://www.nuget.org/packages/BunsenBurner/), and add this to
the top of each test `.cs` file
that needs it:

[!code-csharp[AAAUsing](../../BunsenBurner.Tests/Examples/ArrangeActAssert.cs#ArrangeActAssertUsing)]

or

[!code-csharp[BDDUsing](../../BunsenBurner.Tests/BddTests.cs#BDDUsing)]

Take an existing test,

[!code-csharp[Example1a](../../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1a)]

This can be converted to use
the [test builder (DSL)](<xref:BunsenBurner.TestBuilder`1>) like so,

[!code-csharp[Example1b](../../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1b)]

> [!NOTE]
> Tests must always return a `Task` to be compatible with the DSL.

This formalizes the structure and then opens up refactoring possibilities
such as pulling out a shared act method,

[!code-csharp[Example1c](../../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1c)]

The principle is simple, refine the test structure using a DSL
then use the new structure to identify common code and DRY that code up.
