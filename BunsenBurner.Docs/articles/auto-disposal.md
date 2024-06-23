# Auto Disposal

Any type that implements <xref:System.IDisposable> or <xref:System.IAsyncDisposable>
are auto disposed by the [DSL](xref:BunsenBurner.TestBuilder`1).

This means that you can just return whatever type without needing to worry about
wrapping the DSL in `using` blocks.

[!code-csharp[Example1](../../BunsenBurner.Tests/Examples/AutoDisposal.cs#Example1)]

If you want to opt out of this behaviour call
<xref:BunsenBurner.TestBuilder`1.Asserted`2.NoDisposal*> which will stop disposal.

[!code-csharp[Example2](../../BunsenBurner.Tests/Examples/AutoDisposal.cs#Example2)]
