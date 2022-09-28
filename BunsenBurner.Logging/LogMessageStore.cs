using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Logging;

/// <summary>
/// Store of log messages, can be shared between DummyLogger instances
/// </summary>
public sealed record LogMessageStore : IEnumerable<LogMessage>
{
    private readonly List<LogMessage> _logMessages;

    private LogMessageStore() => _logMessages = new List<LogMessage>();

    [Pure]
    public static LogMessageStore New() => new();

    internal LogMessageStore Log(LogMessage message)
    {
        _logMessages.Add(message);
        return this;
    }

    public IEnumerator<LogMessage> GetEnumerator() => _logMessages.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
