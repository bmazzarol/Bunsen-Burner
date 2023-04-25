using BunsenBurner.Logging;
using Microsoft.Extensions.Hosting;

namespace BunsenBurner.Background;

/// <summary>
/// Context used to test a background service
/// </summary>
/// <param name="Service">background service</param>
/// <param name="Store">log message store</param>
/// <typeparam name="TBackgroundService">background service type</typeparam>
public sealed record BackgroundServiceContext<TBackgroundService>(
    TBackgroundService Service,
    LogMessageStore Store
)
    where TBackgroundService : IHostedService;
