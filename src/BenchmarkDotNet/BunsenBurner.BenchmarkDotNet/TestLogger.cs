using BenchmarkDotNet.Loggers;

namespace BunsenBurner.BenchmarkDotNet;

/// <summary>
/// Logger that supports the test frameworks
/// </summary>
internal sealed class TestLogger : ILogger
{
    private readonly Action<string> _sink;

    public TestLogger(Action<string> sink) => _sink = sink;

    public void Write(LogKind logKind, string text) => _sink(text);

    public void WriteLine() => _sink(Environment.NewLine);

    public void WriteLine(LogKind logKind, string text)
    {
        Write(logKind, text);
        WriteLine();
    }

    public void Flush()
    {
        // flushing is not supported
    }

    public string Id => nameof(TestLogger);

    public int Priority => 0;
}
