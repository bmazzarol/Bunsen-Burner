using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace FluentTests;

/// <summary>
/// Thrown when a scenario is expected to fail and it succeeds
/// </summary>
[Serializable]
public sealed class NoFailureException : Exception
{
    internal NoFailureException() : base("Test was expected to fail, but completed without issue")
    { }

    [ExcludeFromCodeCoverage]
    private NoFailureException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
