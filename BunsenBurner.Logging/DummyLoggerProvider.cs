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

    internal DummyLoggerProvider(LogMessageStore store)
    {
        _store = store;
        _loggers = new LoggerCache(StringComparer.Ordinal);
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers
            .GetOrAdd(
                categoryName,
                static (ownerClassName, store) =>
                    new Lazy<DummyLogger<object>>(() => DummyLogger.New(ownerClassName, store)),
                _store
            )
            .Value;

    public void Dispose()
    {
        // Method intentionally left empty.
    }
}
