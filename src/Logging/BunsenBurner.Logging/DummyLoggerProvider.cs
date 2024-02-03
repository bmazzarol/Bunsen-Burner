using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

using LoggerCache = ConcurrentDictionary<string, Lazy<DummyLogger<object>>>;

/// <summary>
/// Provides and <see cref="DummyLogger{T}"/> provider which can use a shared
/// <see cref="LogMessageStore"/> for messages
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
                category => new Lazy<DummyLogger<object>>(
                    () => DummyLogger.New(category, _store, _sink)
                )
            )
            .Value;

    public void Dispose()
    {
        foreach (var kvp in _loggers)
        {
            if (
                _loggers.TryRemove(kvp.Key, out var logger)
                && logger is { IsValueCreated: true, Value: { } dl }
            )
            {
#pragma warning disable S3966
                dl.Dispose();
#pragma warning restore S3966
            }
        }

        _loggers.Clear();
    }
}
