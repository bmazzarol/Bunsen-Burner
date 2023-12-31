using BunsenBurner.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.NUnit.Tests;

public sealed class LoggerTests
{
    [Test(
        Description = "Logger provider creates loggers from service collections with test output helper"
    )]
    public void Case1()
    {
        var store = LogMessageStore.New();
        var services = new ServiceCollection();
        services.ClearLoggingProviders().AddDummyLogger(store);
        var sp = services.BuildServiceProvider();
        var logger1 = sp.GetRequiredService<ILogger<int>>();
        var logger2 = sp.GetRequiredService<ILogger<string>>();
        logger1.LogInformation("logger 1");
        logger2.LogInformation("logger 2");
        Assert.That(store.Any());
    }

    [Test(
        Description = "Logger provider creates loggers from service collections with test output helper and custom format"
    )]
    public void Case2()
    {
        var store = LogMessageStore.New();
        var services = new ServiceCollection();
        services
            .ClearLoggingProviders()
            .AddDummyLogger(store, message => $"[{message.Level}]: {message.Message}");
        var sp = services.BuildServiceProvider();
        var logger1 = sp.GetRequiredService<ILogger<int>>();
        var logger2 = sp.GetRequiredService<ILogger<string>>();
        logger1.LogInformation("logger 1");
        logger2.LogInformation("logger 2");
        Assert.That(store.Any());
    }
}
