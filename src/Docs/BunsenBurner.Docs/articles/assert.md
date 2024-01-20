# Assert

An assert step must follow at least on act step.

Both sync and async is supported as well as additional `And` steps.

[!code-csharp[Example5](../../../Core/BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example5)]

The standard assert step is a simple Action and succeeds as long as nothing
throws.

## Expression Based Asserts

Another style of assertion is using expressions.

[!code-csharp[Example6](../../../Core/BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example6)]

## Failure Based Asserts

On the unhappy path the `AssertFailsWith` can be used to assert that act will
fail with some exception,

[!code-csharp[Example7](../../../Core/BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example7)]

If the act step does not fail the test will fail.
