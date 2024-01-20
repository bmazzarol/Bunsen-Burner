using static System.DateTimeOffset;

namespace BunsenBurner.Logging;

using LogFormatter = Func<LogMessage, string>;

/// <summary>
/// Provides a simple sink for rendered <see cref="LogMessage"/> strings
/// </summary>
public readonly struct Sink
{
    private static readonly LogFormatter DefaultFormatter = message =>
        $"{message.Level} ({Now:s}) [{message.EventId}]: {message.Message}";

    private readonly LogFormatter? _formatter;
    private readonly Action<string> _logSink;

    private Sink(Action<string> logSink, Func<LogMessage, string>? formatter)
    {
        _logSink = logSink;
        _formatter = formatter;
    }

    /// <summary>
    /// Creates a new sink
    /// </summary>
    /// <param name="logSink">log sink action</param>
    /// <param name="formatter">formatter</param>
    /// <returns>sink</returns>
    public static Sink New(Action<string> logSink, Func<LogMessage, string>? formatter = null) =>
        new(logSink, formatter);

    /// <summary>
    /// Write a messages to the sink
    /// </summary>
    /// <param name="message">message</param>
    public void Write(LogMessage message) =>
        _logSink((_formatter ?? DefaultFormatter).Invoke(message));
}
