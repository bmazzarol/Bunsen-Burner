using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging.Tests;

public static class DummyLoggerTests
{
    [Fact(DisplayName = "Can log messages and get them back")]
    public static void Case1()
    {
        var logger = DummyLogger<object>.New();
        logger.LogInformation("information");
        logger.LogWarning("warning");
        logger.LogError("error");
        logger.LogCritical("critical");
        logger.LogDebug("debug");
        using var scope = logger.BeginScope("test");
        logger.LogTrace("trace");
        Assert.Equal(6, logger.Count());
        Assert.Contains(logger, message => message.Message == "warning");
        Assert.Contains(logger, message => message.Message == "trace");
    }

    [Theory(DisplayName = "Is logging enabled should be true")]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Critical)]
    [InlineData(LogLevel.Debug)]
    [InlineData(LogLevel.Error)]
    [InlineData(LogLevel.Trace)]
    [InlineData(LogLevel.Warning)]
    [InlineData(LogLevel.None)]
    public static void Case2(LogLevel level) =>
        Assert.True(DummyLogger<object>.New().IsEnabled(level));
}
