using BunsenBurner.Logging;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.Http;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<TRequest> ArrangeRequest<TRequest, TSyntax>(
        this TRequest request
    )
        where TRequest : Request
        where TSyntax : struct, Syntax => new(default, () => Task.FromResult(request));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TRequest> ArrangeRequest<TRequest, TSyntax>(
        this string name,
        TRequest request
    )
        where TRequest : Request
        where TSyntax : struct, Syntax => new(name, () => Task.FromResult(request));

    private static async Task<Response> InternalCall<TRequest>(TRequest request, HttpClient client)
        where TRequest : Request
    {
        var httpResp = await client.SendAsync(request.HttpRequestMessage()).ConfigureAwait(false);
        return await Response.New(httpResp).ConfigureAwait(false);
    }

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, ResponseContext> ActAndCall<
        TData,
        TRequest,
        TSyntax
    >(this Scenario<TSyntax>.Arranged<TData> scenario, Func<TData, TRequest> fn, TestServer server)
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data =>
            {
                var resp = await InternalCall(fn(data), server.CreateClient())
                    .ConfigureAwait(false);
                return new ResponseContext(
                    resp,
                    server.Services.GetService<LogMessageStore>() ?? LogMessageStore.New()
                );
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<TRequest, ResponseContext> ActAndCall<
        TRequest,
        TSyntax
    >(this Scenario<TSyntax>.Arranged<TRequest> scenario, TestServer server)
        where TRequest : Request
        where TSyntax : struct, Syntax => scenario.ActAndCall(static _ => _, server);

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, Response> ActAndCall<TData, TRequest, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TRequest> fn
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            data => InternalCall(fn(data), new HttpClient())
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<TRequest, Response> ActAndCall<TSyntax, TRequest>(
        this Scenario<TSyntax>.Arranged<TRequest> scenario
    )
        where TRequest : Request
        where TSyntax : struct, Syntax => scenario.ActAndCall(static _ => _);
}
