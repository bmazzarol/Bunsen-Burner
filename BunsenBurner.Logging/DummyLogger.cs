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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => new NoopScope();

    private sealed record NoopScope : IDisposable
    {
        public void Dispose()
        {
            // Method intentionally left empty.
        }
    }

    /// <inheritdoc />
    public IEnumerator<LogMessage> GetEnumerator() => _store.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/// <summary>
/// Dummy logger constructors
/// </summary>
public static class DummyLogger
{
    /// <summary>
    /// Creates a new typed dummy logger
    /// </summary>
    /// <param name="store">log message store</param>
    /// <typeparam name="T">some parent T</typeparam>
    /// <returns>dummy logger T</returns>
    [Pure]
    public static DummyLogger<T> New<T>(LogMessageStore? store = default) =>
        new(store ?? LogMessageStore.New(), typeof(T).FullName ?? string.Empty);

    /// <summary>
    /// Creates a new un-typed dummy logger
    /// </summary>
    /// <param name="ownerClassName">some parent class name</param>
    /// <param name="store">log message store</param>
    /// <returns>dummy logger</returns>
    [Pure]
    public static DummyLogger<object> New(
        string ownerClassName,
        LogMessageStore? store = default
    ) => new(store ?? LogMessageStore.New(), ownerClassName);
}
