using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Logging;

/// <summary>
/// Store of log messages, can be shared between DummyLogger instances
/// </summary>
public sealed record LogMessageStore : IEnumerable<LogMessage>
{
    private readonly ConcurrentBag<LogMessage> _logMessages;

    private LogMessageStore() => _logMessages = new ConcurrentBag<LogMessage>();

    [Pure]
    public static LogMessageStore New() => new();

    internal void Log(LogMessage message) => _logMessages.Add(message);

    public IEnumerator<LogMessage> GetEnumerator() => _logMessages.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Clear() => _logMessages.Clear();
}
