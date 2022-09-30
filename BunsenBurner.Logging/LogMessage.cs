using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Log message
/// </summary>
public sealed record LogMessage
{
    public Type ClassType { get; }
    public LogLevel Level { get; }
    public EventId EventId { get; }
    public Exception? Exception { get; }
    public string Message { get; }

    private LogMessage(
        Type classType,
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

    internal static LogMessage New<T>(
        LogLevel level,
        EventId eventId,
        Exception? exception,
        string message
    ) => new(typeof(T), level, eventId, exception, message);
}
