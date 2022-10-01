using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Dummy logger that can be used to create assertions against
/// </summary>
/// <typeparam name="T">Some T</typeparam>
public sealed record DummyLogger<T> : ILogger<T>, IEnumerable<LogMessage>
{
    private readonly string _ownerClassName;
    private readonly LogMessageStore _store;

    internal DummyLogger(LogMessageStore store, string ownerClassName)
    {
        _store = store;
        _ownerClassName = ownerClassName;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) =>
        _store.Log(
            LogMessage.New(
                _ownerClassName,
                logLevel,
                eventId,
                exception,
                formatter(state, exception)
            )
        );

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) => new NoopScope();

    private sealed record NoopScope : IDisposable
    {
        public void Dispose()
        {
            // Method intentionally left empty.
        }
    }

    public IEnumerator<LogMessage> GetEnumerator() => _store.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class DummyLogger
{
    [Pure]
    public static DummyLogger<T> New<T>(LogMessageStore? store = default) =>
        new(store ?? LogMessageStore.New(), typeof(T).FullName ?? string.Empty);

    [Pure]
    public static DummyLogger<object> New(
        string ownerClassName,
        LogMessageStore? store = default
    ) => new(store ?? LogMessageStore.New(), ownerClassName);
}
