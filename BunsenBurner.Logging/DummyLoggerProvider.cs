using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

using LoggerCache = ConcurrentDictionary<string, Lazy<DummyLogger<object>>>;

/// <summary>
/// Provides and dummy logger provider which can use a shared backing store for messages
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed record DummyLoggerProvider : ILoggerProvider
{
    private readonly LogMessageStore _store;
    private readonly LoggerCache _loggers;
    private readonly Sink? _sink;

    internal DummyLoggerProvider(LogMessageStore store, Sink? sink = default)
    {
        _store = store;
        _sink = sink;
        _loggers = new LoggerCache(StringComparer.Ordinal);
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers
            .GetOrAdd(
                categoryName,
                ownerClassName =>
                    new Lazy<DummyLogger<object>>(
                        () => DummyLogger.New(ownerClassName, _store, _sink)
                    )
            )
            .Value;

    public void Dispose()
    {
        foreach (
            var logger in _loggers.Where(x => x.Value.IsValueCreated).Select(x => x.Value.Value)
        )
        {
            logger.Dispose();
        }

        _loggers.Clear();
    }
}
