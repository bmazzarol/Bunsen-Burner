# Act

An act step must follow at least one arrange step.

Both sync and async is supported as well as additional `And` steps.

[!code-csharp[Example4](../../../Core/BunsenBurner.Tests/Examples/ArrangeActAssert.cs#Example4)]

`And` steps get passed the arranged data and result of the last `Act`
and transform that to a new result.
