using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Dummy logger that can be used to create assertions against
/// </summary>
/// <typeparam name="T">Some T</typeparam>
public sealed record DummyLogger<T> : ILogger<T>, IEnumerable<LogMessage>, IDisposable
{
    private readonly string _ownerClassName;
    private readonly LogMessageStore _store;
    private readonly Sink? _sink;
    private readonly ConcurrentDictionary<Scope, Scope> _scopes = new();

    internal DummyLogger(LogMessageStore store, string ownerClassName, Sink? sink)
    {
        _store = store;
        _ownerClassName = ownerClassName;
        _sink = sink;
    }

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        var message = LogMessage.New(
            _ownerClassName,
            logLevel,
            eventId,
            exception,
            formatter(state, exception),
            _scopes.Keys.Select(x => x.State)
        );
        _store.Log(message);
        _sink?.Write(message);
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        var scope = new Scope(state, this);
        _scopes.TryAdd(scope, scope);
        return scope;
    }

    private sealed record Scope(object State, DummyLogger<T> Parent) : IDisposable
    {
        [ExcludeFromCodeCoverage]
        public void Dispose() => Parent._scopes.TryRemove(this, out _);
    }

    /// <inheritdoc />
    public IEnumerator<LogMessage> GetEnumerator() => _store.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var scope in _scopes.Keys)
            scope.Dispose();
        _scopes.Clear();
    }
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
    /// <param name="sink">optional message sink to log through to</param>
    /// <typeparam name="T">some parent T</typeparam>
    /// <returns>dummy logger T</returns>
    [Pure]
    public static DummyLogger<T> New<T>(LogMessageStore? store = default, Sink? sink = default) =>
        new(store ?? LogMessageStore.New(), typeof(T).FullName ?? string.Empty, sink);

    /// <summary>
    /// Creates a new un-typed dummy logger
    /// </summary>
    /// <param name="ownerClassName">some parent class name</param>
    /// <param name="store">log message store</param>
    /// <param name="sink">optional message sink to log through to</param>
    /// <returns>dummy logger</returns>
    [Pure]
    public static DummyLogger<object> New(
        string ownerClassName,
        LogMessageStore? store = default,
        Sink? sink = default
    ) => new(store ?? LogMessageStore.New(), ownerClassName, sink);
}
