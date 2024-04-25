using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging.Tests;

public static class DummyLoggerTests
{
    [Fact(DisplayName = "Can log messages and get them back")]
    public static async Task Case1() =>
        await DummyLogger
            .New<object>()
            .Arrange()
            .Act(logger =>
            {
                logger.LogInformation("information");
                logger.LogWarning("warning");
                logger.LogError("error");
                logger.LogCritical("critical");
                logger.LogDebug("debug");
                using var scope = logger.BeginScope("test");
                logger.LogTrace("trace");
                logger.Dispose();
                return logger;
            })
            .Assert(logger =>
            {
                Assert.Equal(6, logger.Count());
                Assert.Contains(
                    logger,
                    message => string.Equals(message.Message, "warning", StringComparison.Ordinal)
                );
                Assert.Contains(
                    logger,
                    message =>
                        string.Equals(message.Message, "trace", StringComparison.Ordinal)
                        && message.Scopes.Contains("test")
                );
            });

    [Fact(DisplayName = "Nested scopes work and provide the correct logged message")]
    public static async Task Case2() =>
        await DummyLogger
            .New<object>()
            .Arrange()
            .Act(logger =>
            {
                logger.LogInformation("information");
                logger.LogWarning("warning");
                logger.LogError("error");
                logger.LogCritical("critical");
                using var scope1 = logger.BeginScope("test");
                logger.LogDebug("debug");
                using var scope2 = logger.BeginScope("test2");
                logger.LogTrace("trace");
                logger.Dispose();
                return logger;
            })
            .Assert(logger =>
            {
                Assert.Equal(6, logger.Count());
                Assert.Contains(
                    logger,
                    message => string.Equals(message.Message, "warning", StringComparison.Ordinal)
                );
                Assert.Contains(
                    logger,
                    message =>
                        string.Equals(message.Message, "debug", StringComparison.Ordinal)
                        && message.Scopes.Contains("test")
                        && !message.Scopes.Contains("test2")
                );
                Assert.Contains(
                    logger,
                    message =>
                        string.Equals(message.Message, "trace", StringComparison.Ordinal)
                        && message.Scopes.Contains("test")
                        && message.Scopes.Contains("test2")
                );
            });

    [Theory(DisplayName = "Is logging enabled should be true")]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Critical)]
    [InlineData(LogLevel.Debug)]
    [InlineData(LogLevel.Error)]
    [InlineData(LogLevel.Trace)]
    [InlineData(LogLevel.Warning)]
    [InlineData(LogLevel.None)]
    public static void Case3(LogLevel level) =>
        Assert.True(DummyLogger.New<object>().IsEnabled(level));

    [Fact(DisplayName = "Logger provider creates loggers from service collections")]
    public static async Task Case4() =>
        await Arrange(() =>
            {
                var store = LogMessageStore.New();
                var services = new ServiceCollection();
                services.ClearLoggingProviders().AddDummyLogger(store);
                return (store, serviceProvider: services.BuildServiceProvider());
            })
            .Act(t =>
            {
                var logger1 = t.serviceProvider.GetRequiredService<ILogger<int>>();
                var logger2 = t.serviceProvider.GetRequiredService<ILogger<string>>();
                logger1.LogInformation("logger 1");
                logger2.LogInformation("logger 2");
                return (t.store, logger1, logger2);
            })
            .Assert(t =>
            {
                Assert.Equal(2, t.store.Count());
                Assert.Contains(
                    t.store,
                    message => string.Equals(message.Message, "logger 1", StringComparison.Ordinal)
                );
                Assert.Contains(
                    t.store,
                    message => string.Equals(message.Message, "logger 2", StringComparison.Ordinal)
                );
                t.store.Clear();
            });

    [Fact(DisplayName = "Log message store can be converted to a logger factory")]
    public static async Task Case5() =>
        await LogMessageStore
            .New()
            .Arrange()
            .Act(store =>
            {
                var lf = store.ToLoggerFactory();
                var logger1 = lf.CreateLogger("test");
                logger1.LogWarning("test 1");
                var logger2 = lf.CreateLogger("test2");
                logger2.LogWarning("test 2");
                return store;
            })
            .Assert(store =>
            {
                Assert.Equal(2, store.Count());
                Assert.Contains(
                    store,
                    message => string.Equals(message.Message, "test 1", StringComparison.Ordinal)
                );
                Assert.Contains(
                    store,
                    message => string.Equals(message.Message, "test 2", StringComparison.Ordinal)
                );
            });
}
