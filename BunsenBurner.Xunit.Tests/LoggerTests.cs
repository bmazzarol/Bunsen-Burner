using BunsenBurner.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BunsenBurner.Xunit.Tests;

public sealed class LoggerTests
{
    private readonly ITestOutputHelper _outputHelper;

    public LoggerTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

    [Fact(
        DisplayName = "Logger provider creates loggers from service collections with test output helper"
    )]
    public void Case1()
    {
        var store = LogMessageStore.New();
        var services = new ServiceCollection();
        services.ClearLoggingProviders().AddDummyLogger(store, _outputHelper);
        var sp = services.BuildServiceProvider();
        var logger1 = sp.GetRequiredService<ILogger<int>>();
        var logger2 = sp.GetRequiredService<ILogger<string>>();
        logger1.LogInformation("logger 1");
        logger2.LogInformation("logger 2");
        Assert.Equal(2, store.Count());
        Assert.Contains(store, message => message.Message == "logger 1");
        Assert.Contains(store, message => message.Message == "logger 2");
    }

    [Fact(
        DisplayName = "Logger provider creates loggers from service collections with test output helper and custom format"
    )]
    public void Case2()
    {
        var store = LogMessageStore.New();
        var services = new ServiceCollection();
        services
            .ClearLoggingProviders()
            .AddDummyLogger(
                store,
                _outputHelper,
                message => $"[{message.Level}]: {message.Message}"
            );
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
