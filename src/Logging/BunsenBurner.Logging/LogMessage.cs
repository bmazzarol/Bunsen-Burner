using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Log message
/// </summary>
public sealed record LogMessage
{
    /// <summary>
    /// Category name
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Log level
    /// </summary>
    public LogLevel Level { get; }

    /// <summary>
    /// Log event id
    /// </summary>
    public EventId EventId { get; }

    /// <summary>
    /// Optional exception
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Log message
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// 0 or more scopes
    /// </summary>
    public IEnumerable<object> Scopes { get; }

    private LogMessage(
        string category,
        LogLevel level,
        EventId eventId,
        Exception? exception,
        string message,
        IEnumerable<object> scopes
    )
    {
        Category = category;
        Level = level;
        EventId = eventId;
        Exception = exception;
        Message = message;
        Scopes = scopes;
    }

    internal static LogMessage New(
        string category,
        LogLevel level,
        EventId eventId,
        Exception? exception,
        string message,
        IEnumerable<object> scopes
    ) => new(category, level, eventId, exception, message, scopes);
}
