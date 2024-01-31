# Auto Disposal

Any type that implements <xref:System.IDisposable> or <xref:System.IAsyncDisposable>
are auto disposed by the [DSL](xref:BunsenBurner.TestBuilder`1).

This means that you can just return whatever type without needing to worry about
wrapping the DSL in `using` blocks.

[!code-csharp[Example1](../../../Core/BunsenBurner.Tests/Examples/AutoDisposal.cs#Example1)]

## Manual Disposal

If you want to opt out of this behaviour you can wrap your type in a
<xref:BunsenBurner.Utility.ManualDisposal`1> which will stop disposal.

[!code-csharp[Example2](../../../Core/BunsenBurner.Tests/Examples/AutoDisposal.cs#Example2)]
