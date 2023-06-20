using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Store of log messages, can be shared between DummyLogger instances
/// </summary>
public sealed record LogMessageStore : IEnumerable<LogMessage>
{
    private readonly ConcurrentStack<LogMessage> _logMessages;

    private LogMessageStore() => _logMessages = new ConcurrentStack<LogMessage>();

    /// <summary>
    /// Creates a new log messages store
    /// </summary>
    /// <returns>log message store</returns>
    [Pure]
    public static LogMessageStore New() => new();

    internal void Log(LogMessage message) => _logMessages.Push(message);

    /// <inheritdoc />
    public IEnumerator<LogMessage> GetEnumerator() => _logMessages.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Clears the log message store
    /// </summary>
    public void Clear() => _logMessages.Clear();

    /// <summary>
    /// Converts the log message store to a logger factory
    /// </summary>
    /// <returns>logger factory</returns>
    public ILoggerFactory AsLoggerFactory() =>
        new ServiceCollection()
            .AddDummyLogger(this)
            .BuildServiceProvider()
            .GetRequiredService<ILoggerFactory>();
}
