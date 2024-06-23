# Syntax

Bunsen Burner supports 2 DLS flavours via <xref:BunsenBurner.ISyntax`1>,

1. <xref:BunsenBurner.ArrangeActAssertSyntax> - [Arrange Act Assert](xref:BunsenBurner.ArrangeActAssert)

   [!code-csharp[Example1](../../BunsenBurner.Tests/Examples/AaaExample.cs#Example1)]

2. <xref:BunsenBurner.GivenWhenThenSyntax> - [Given When Then (BDD)](xref:BunsenBurner.GivenWhenThen)

   [!code-csharp[Example1](../../BunsenBurner.Tests/Examples/BddExample.cs#Example1)]

It does this because both are valid, and it depends on the teams
enforced practises as to which is used.

Based on how the [DSL](<xref:BunsenBurner.TestBuilder`1>) is designed, the
flavours can never mix.
