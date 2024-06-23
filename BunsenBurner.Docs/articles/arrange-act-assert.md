# Arrange Act Assert Pattern

The [Arrange, Act and Assert Pattern](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
is a way of structuring tests so that
they are easy to read and understand. This pattern is also known as the Given,
When, Then pattern (one of the reasons both <xref:BunsenBurner.ISyntax`1> is
supported).

By breaking down your tests into these three distinct stages, you can more
easily identify what needs to be tested and ensure that your tests are
comprehensive.

Bunsen Burner aims to empower this by co-opting the compilers help.

It does this in a few ways,

* The [DSL](xref:BunsenBurner.TestBuilder`1) enforces the correct flow, you need
  to arrange, act and assert in the correct order, otherwise it will not compile
* Uses types to drive what each stage in the DSL works on, this allows IDEs to
  lift code out of the DSL and into utility functions that can then be shared

  [!code-csharp[Example1c](../../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1c)]

* Provides additional `And` stages to promote short stages,
  particularly helpful in the assertion stage

  [!code-csharp[Example1](../../BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example1)]
