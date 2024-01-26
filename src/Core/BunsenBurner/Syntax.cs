#pragma warning disable S101, CA1715

namespace BunsenBurner;

/// <summary>
/// Supported syntax for the <see cref="TestBuilder{TSyntax}"/>
/// </summary>
public interface Syntax
{
    /// <summary>
    /// Arrange, act, assert style tests
    /// </summary>
    public readonly struct Aaa : Syntax;

    /// <summary>
    /// Given, when, then style tests used in Behaviour Driven Development
    /// </summary>
    public readonly struct Bdd : Syntax;
}
