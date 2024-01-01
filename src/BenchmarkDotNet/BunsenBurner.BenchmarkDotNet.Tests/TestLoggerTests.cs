using BenchmarkDotNet.Loggers;

namespace BunsenBurner.BenchmarkDotNet.Tests;

using static BunsenBurner.Aaa;

public static class TestLoggerTests
{
    [Fact(DisplayName = "Write methods work")]
    public static async Task Case1() =>
        await Arrange(() =>
            {
                var output = new List<string>();
                var logger = new TestLogger(s => output.Add(s));
                return (output, logger);
            })
            .Act(t =>
            {
                t.logger.WriteLine(LogKind.Default, "test");
                t.logger.Flush();
                return t;
            })
            .Assert(
                r => r.output.Contains("test") && r.output.Count == 2 && r.logger.Priority == 0
            );
}
