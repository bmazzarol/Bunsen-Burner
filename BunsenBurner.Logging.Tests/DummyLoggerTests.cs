using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging.Tests;

public static class DummyLoggerTests
{
    [Fact(DisplayName = "Can log messages and get them back")]
    public static void Case1()
    {
        var logger = DummyLogger.New<object>();
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
        Assert.True(DummyLogger.New<object>().IsEnabled(level));

    [Fact(DisplayName = "Logger provider creates loggers from service collections")]
    public static void Case3()
    {
        var store = LogMessageStore.New();
        var services = new ServiceCollection();
        services.ClearLoggingProviders().AddDummyLogger(store);
        var sp = services.BuildServiceProvider();
        var logger1 = sp.GetRequiredService<ILogger<int>>();
        var logger2 = sp.GetRequiredService<ILogger<string>>();
        logger1.LogInformation("logger 1");
        logger2.LogInformation("logger 2");
        Assert.Equal(2, store.Count());
        Assert.Contains(store, message => message.Message == "logger 1");
        Assert.Contains(store, message => message.Message == "logger 2");
    }
}
