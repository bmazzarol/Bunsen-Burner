using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Store of <see cref="LogMessage"/>, can be shared between <see cref="DummyLogger{T}"/> instances
/// </summary>
public sealed record LogMessageStore : IEnumerable<LogMessage>
{
    private ConcurrentQueue<LogMessage> _logMessages;

    private LogMessageStore() => _logMessages = new ConcurrentQueue<LogMessage>();

    /// <summary>
    /// Creates a new <see cref="LogMessageStore"/>
    /// </summary>
    /// <returns>log message store</returns>
    [Pure]
    public static LogMessageStore New() => new();

    internal void Log(LogMessage message) => _logMessages.Enqueue(message);

    /// <inheritdoc />
    public IEnumerator<LogMessage> GetEnumerator() => _logMessages.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private readonly object _lock = new();

    /// <summary>
    /// Clears the <see cref="LogMessageStore"/>
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _logMessages = new ConcurrentQueue<LogMessage>();
        }
    }

    /// <summary>
    /// Converts the <see cref="LogMessageStore"/> to a <see cref="ILoggerFactory"/>
    /// </summary>
    /// <param name="sink">optional sink to use for the <see cref="ILoggerFactory"/></param>
    /// <returns><see cref="ILoggerFactory"/></returns>
    public ILoggerFactory ToLoggerFactory(Sink? sink = default) =>
        new LoggerFactory(new[] { new DummyLoggerProvider(store: this, sink) });
}
