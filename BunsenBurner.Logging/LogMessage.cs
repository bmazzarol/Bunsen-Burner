using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Log message
/// </summary>
public sealed record LogMessage
{
    /// <summary>
    /// Parent class name
    /// </summary>
    public string ClassType { get; }

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

    private LogMessage(
        string classType,
        LogLevel level,
        EventId eventId,
        Exception? exception,
        string message
    )
    {
        ClassType = classType;
        Level = level;
        EventId = eventId;
        Exception = exception;
        Message = message;
    }

    internal static LogMessage New(
        string classType,
        LogLevel level,
        EventId eventId,
        Exception? exception,
        string message
    ) => new(classType, level, eventId, exception, message);
}
