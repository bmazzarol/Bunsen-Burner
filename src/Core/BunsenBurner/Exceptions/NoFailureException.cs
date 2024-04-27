using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Exceptions;

/// <summary>
/// Thrown when an <see cref="TestBuilder{TSyntax}"/> is expected to fail and it succeeds
/// </summary>
[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors")]
public sealed class NoFailureException : Exception
{
    internal NoFailureException()
        : base("Test was expected to fail, but completed without issue") { }
}
