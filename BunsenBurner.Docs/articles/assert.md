# Assert

An [assert](xref:BunsenBurner.TestBuilder`1.Asserted`2) step must follow at
least one
[act](xref:BunsenBurner.TestBuilder`1.Acted`2) step.

Both sync and async is supported as well as
additional <xref:BunsenBurner.TestBuilder`1.Asserted`2.And*> steps.

<xref:BunsenBurner.TestBuilder`1.Asserted`2.And*> steps are executed in
parallel and will return an aggregate exception if more than one assertion
fails.

[!code-csharp[Example5](../../BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example5)]

The standard [assert](xref:BunsenBurner.TestBuilder`1.Asserted`2) step is a
simple Action and
succeeds as long as nothing throws.

## Expression Based Asserts

Another style of assertion is using expressions.

[!code-csharp[Example6](../../BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example6)]

## Asserting against exceptions

On the unhappy path [throw](xref:BunsenBurner.TestBuilder`1.Acted`2.Throw*) can be
used to assert against exceptions,

[!code-csharp[Example7](../../BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example7)]

If the [act](xref:BunsenBurner.TestBuilder`1.Acted`2) step does not fail the
test will fail.
