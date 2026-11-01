# Getting Started

Add the package, then use `BunsenBurner` in each test file that needs it,

```shell
dotnet add package BunsenBurner
```

[!code-csharp[AAAUsing](../../BunsenBurner.Tests/Examples/ArrangeActAssert.cs#ArrangeActAssertUsing)]

or the Given/When/Then spelling,

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

The idea is to use the DSL to make the shape of the test explicit, then use
that shape to find code that can be shared.
