using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace BunsenBurner;

/// <summary>
/// Thrown when a <see cref="Scenario{TSyntax}"/> is expected to fail and it succeeds
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public sealed class NoFailureException : Exception
{
    internal NoFailureException()
        : base("Test was expected to fail, but completed without issue") { }

    private NoFailureException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

    internal NoFailureException(string message)
        : base(message) { }

    internal NoFailureException(string message, Exception innerException)
        : base(message, innerException) { }
}
