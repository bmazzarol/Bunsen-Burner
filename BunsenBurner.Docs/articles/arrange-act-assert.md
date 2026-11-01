# Arrange Act Assert Pattern

The [Arrange, Act and Assert Pattern](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
structures a test so that it reads in one direction. It is also known as
Given, When, Then, which is why both <xref:BunsenBurner.ISyntax`1> spellings are
supported.

Splitting a test into these three stages makes it obvious what is being tested
and where the setup can be shared.

Bunsen Burner enforces the pattern with the compiler. It does that in a few
ways,

* The [DSL](xref:BunsenBurner.TestBuilder`1) enforces the correct flow, you need
  to arrange, act and assert in the correct order, otherwise it will not compile
* Uses types to drive what each stage in the DSL works on, this allows IDEs to
  lift code out of the DSL and into utility functions that can then be shared

  [!code-csharp[Example1c](../../BunsenBurner.Tests/Examples/GettingStarted.cs#Example1c)]

* Provides additional `And` stages to promote short stages,
  particularly helpful in the assertion stage

  [!code-csharp[Example1](../../BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example1)]
