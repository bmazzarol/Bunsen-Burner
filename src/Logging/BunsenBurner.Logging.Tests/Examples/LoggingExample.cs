using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BunsenBurner.Logging.Tests.Examples;

#region Example1

public class LoggingExample(ITestOutputHelper testOutputHelper)
{
    [Fact(DisplayName = "Example using test logger")]
    public async Task ExampleTest1() =>
        await Arrange(() =>
            {
                // create an instance of a log message store
                LogMessageStore store = LogMessageStore.New();
                // this can be used by n number of logger instances
                // in-fact it can be converted to a logger factory for this
                ILoggerFactory factory = store.ToLoggerFactory(
                    // we can also pass an optional sink in to get the output in our
                    // test explorer
                    sink: Sink.New(testOutputHelper.WriteLine)
                );
                return (store, factory);
            })
            .Act(data =>
            {
                // now we can create loggers
                var logger1 = DummyLogger.New<LoggingExample>(
                    data.store,
                    // a sink can also be passed to the logger directly
                    sink: Sink.New(testOutputHelper.WriteLine)
                );
                logger1.LogInformation("Some info log message");
                // or create via the factory
                var logger2 = data.factory.CreateLogger(nameof(ExampleTest1));
                logger2.LogWarning("Some warning log message");
                return data.store;
            })
            .Assert(store =>
            {
                // now we have full access to assert against the store
                Assert.Collection(
                    store,
                    message1 =>
                    {
                        Assert.Equal(expected: "Some info log message", actual: message1.Message);
                        Assert.Equal(
                            expected: typeof(LoggingExample).FullName,
                            actual: message1.Category
                        );
                        Assert.Equal(expected: LogLevel.Information, actual: message1.Level);
                    },
                    message2 =>
                    {
                        Assert.Equal(
                            expected: "Some warning log message",
                            actual: message2.Message
                        );
                        Assert.Equal(expected: nameof(ExampleTest1), actual: message2.Category);
                        Assert.Equal(expected: LogLevel.Warning, actual: message2.Level);
                    }
                );
            });
}

#endregion
