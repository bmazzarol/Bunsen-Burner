# Act

An [act](xref:BunsenBurner.TestBuilder`1.Acted`2) step must follow at least one
[arrange](xref:BunsenBurner.TestBuilder`1.Arranged`1) step.

Both sync and async is supported as well as additional
<xref:BunsenBurner.TestBuilder`1.Acted`2.And*> steps.

[!code-csharp[Example4](../../BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example4)]

<xref:BunsenBurner.TestBuilder`1.Acted`2.And*> steps get passed the arranged
data and result of the last
[act](xref:BunsenBurner.TestBuilder`1.Acted`2) step
and transform that to a new result.
