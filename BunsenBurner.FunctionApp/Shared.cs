using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace BunsenBurner.FunctionApp;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    private static ConcurrentDictionary<string, Lazy<IHost>> HostCache => new();

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResult> ActAndExecute<
        TData,
        TResult,
        TStartup,
        TFunction,
        TSyntax
    >(this Scenario<TSyntax>.Arranged<TData> scenario, Func<TData, TFunction, Task<TResult>> fn)
        where TStartup : FunctionsStartup, new()
        where TSyntax : struct, Syntax
        where TFunction : class =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data =>
            {
                var startupType = typeof(TStartup);
                var functionType = typeof(TFunction);
                return await fn(
                    data,
                    HostCache
                        .GetOrAdd(
                            $"{startupType.FullName ?? startupType.Name}_{functionType.FullName ?? functionType.Name}",
                            _ =>
                                new Lazy<IHost>(
                                    () =>
                                        new HostBuilder()
                                            .ConfigureWebJobs(new TStartup().Configure)
                                            .ConfigureServices(
                                                services => services.TryAddSingleton<TFunction>()
                                            )
                                            .Build()
                                )
                        )
                        .Value.Services.GetRequiredService<TFunction>()
                );
            }
        );
}
