# Syntax

Bunsen Burner supports 2 DLS flavours via @BunsenBurner.Syntax,

1. @BunsenBurner.Syntax.Aaa - Arrange Act Assert
   
   [!code-csharp[Example1](../../../Core/BunsenBurner.Tests/Examples/AaaExample.cs#Example1)]
   
2. @BunsenBurner.Syntax.Bdd - Given When Then
   
   [!code-csharp[Example1](../../../Core/BunsenBurner.Tests/Examples/BddExample.cs#Example1)]

It does this because both are valid and it depends on the teams
enforced practises as to which is used.

Based on how the DSL is designed, the flavours can never mix.
