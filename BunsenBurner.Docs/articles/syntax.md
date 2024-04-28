# Syntax

Bunsen Burner supports 2 DLS flavours via <xref:BunsenBurner.Syntax>,

1. <xref:BunsenBurner.Syntax.Aaa> - [Arrange Act Assert](xref:BunsenBurner.AaaSyntax)

   [!code-csharp[Example1](../../BunsenBurner.Tests/Examples/AaaExample.cs#Example1)]

2. <xref:BunsenBurner.Syntax.Bdd> - [Given When Then (BDD)](xref:BunsenBurner.BddSyntax)

   [!code-csharp[Example1](../../BunsenBurner.Tests/Examples/BddExample.cs#Example1)]

It does this because both are valid, and it depends on the teams
enforced practises as to which is used.

Based on how the [DSL](<xref:BunsenBurner.TestBuilder`1>) is designed, the
flavours can never mix.
