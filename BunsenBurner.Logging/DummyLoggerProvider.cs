using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace BunsenBurner.Logging;

/// <summary>
/// Provides and dummy logger provider which can use a shared backing store for messages
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed record DummyLoggerProvider : ILoggerProvider
{
    private readonly LogMessageStore _store;

    internal DummyLoggerProvider(LogMessageStore store) => _store = store;

    public ILogger CreateLogger(string categoryName)
    {
        var ownerType = Type.GetType(categoryName);
        var loggerType = typeof(DummyLogger<>).MakeGenericType(ownerType ?? typeof(object));
        var logger = Activator.CreateInstance(loggerType, _store);
        return logger as ILogger ?? throw new InvalidOperationException();
    }

    public void Dispose() { }
}
