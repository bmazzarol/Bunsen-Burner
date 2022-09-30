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
    private readonly LogMessageStore _store;

    public DummyLogger(LogMessageStore store) => _store = store;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) => _store.Log(LogMessage.New<T>(logLevel, eventId, exception, formatter(state, exception)));

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
        new(store ?? LogMessageStore.New());
}
