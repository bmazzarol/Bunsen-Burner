using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Log message
/// </summary>
public sealed record LogMessage
{
    public readonly Type ClassType;
    public readonly LogLevel Level;
    public readonly EventId EventId;
    public readonly Exception? Exception;
    public readonly string Message;

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
