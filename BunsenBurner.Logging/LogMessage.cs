using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Log message
/// </summary>
public sealed record LogMessage
{
    public string ClassType { get; }
    public LogLevel Level { get; }
    public EventId EventId { get; }
    public Exception? Exception { get; }
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
