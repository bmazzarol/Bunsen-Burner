# Auto Disposal

Any type that implements <xref:System.IDisposable> or <xref:System.IAsyncDisposable>
are auto disposed by the [DSL](xref:BunsenBurner.TestBuilder`1).

Return the type directly; no `using` block is needed around the DSL.

[!code-csharp[Example1](../../BunsenBurner.Tests/Examples/AutoDisposal.cs#Example1)]

If you want to opt out of this behaviour call
<xref:BunsenBurner.TestBuilder`1.Asserted`2.NoDisposal*> which will stop disposal.

[!code-csharp[Example2](../../BunsenBurner.Tests/Examples/AutoDisposal.cs#Example2)]
