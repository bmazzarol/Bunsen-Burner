namespace BunsenBurner.Logging;

/// <summary>
/// Provides a simple sink for rendered log message strings
/// </summary>
public readonly struct Sink
{
    private readonly Func<LogMessage, string>? _formatter;
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
        _logSink(_formatter?.Invoke(message) ?? message.Message);
}
