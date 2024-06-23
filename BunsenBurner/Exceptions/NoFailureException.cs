using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Exceptions;

/// <summary>
/// Thrown when an <see cref="TestBuilder{TSyntax}"/> is expected to fail and it succeeds
/// </summary>
[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors")]
public sealed class NoFailureException : Exception
{
    internal NoFailureException(string? name)
        : base(BuildErrorMessage(name)) { }

    private static string BuildErrorMessage(string? name)
    {
        if (name is null)
        {
            return "Test did not fail as expected";
        }

        return $"Test '{name}' did not fail as expected";
    }
}
