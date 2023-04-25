using System.Linq.Expressions;
using BunsenBurner.Logging;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static BunsenBurner.Shared;

namespace BunsenBurner.Http;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<HttpRequestMessage> ArrangeRequest<TSyntax>(
        this HttpRequestMessage request
    )
        where TSyntax : struct, Syntax => Arrange<HttpRequestMessage, TSyntax>(request);

    [Pure]
    internal static Scenario<TSyntax>.Arranged<HttpRequestMessage> ArrangeRequest<TSyntax>(
        this string name,
        HttpRequestMessage request
    )
        where TSyntax : struct, Syntax => name.Arrange<HttpRequestMessage, TSyntax>(request);

    private static async Task<HttpResponseMessage> InternalCall(
        HttpRequestMessage request,
        HttpClient client,
        ILogger? logger = null
    )
    {
        var requestToSend = await request.CloneRequest();
        logger?.LogInformation("{Request}", await request.AsCurlString());
        var httpResp = await client.SendAsync(requestToSend);
        logger?.LogInformation("{Response}", await httpResp.AsCurlString());
        return httpResp;
    }

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, ResponseContext> ActAndCall<TData, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, HttpRequestMessage> fn,
        Func<TData, TestServer> serverFn
    )
        where TSyntax : struct, Syntax =>
        scenario.Act(async data =>
        {
            var server = serverFn(data);
            var store = server.Host.Services.GetService<LogMessageStore>();
            var logger = server.Host.Services.GetService<ILogger<object>>();
            var resp = await InternalCall(fn(data), server.CreateClient(), logger);
            return new ResponseContext(resp, store ?? LogMessageStore.New());
        });

    [Pure]
    internal static Scenario<TSyntax>.Acted<
        HttpRequestMessage,
        ResponseContext
    > ActAndCall<TSyntax>(
        this Scenario<TSyntax>.Arranged<HttpRequestMessage> scenario,
        TestServer server
    )
        where TSyntax : struct, Syntax => scenario.ActAndCall(static _ => _, _ => server);

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, HttpResponseMessage> ActAndCall<TData, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, HttpRequestMessage> fn,
        Func<HttpClient>? clientFactory = default
    )
        where TSyntax : struct, Syntax =>
        scenario.Act(data => InternalCall(fn(data), clientFactory?.Invoke() ?? new HttpClient()));

    [Pure]
    internal static Scenario<TSyntax>.Acted<
        HttpRequestMessage,
        HttpResponseMessage
    > ActAndCall<TSyntax>(
        this Scenario<TSyntax>.Arranged<HttpRequestMessage> scenario,
        Func<HttpClient>? clientFactory = default
    )
        where TSyntax : struct, Syntax => scenario.ActAndCall(static _ => _, clientFactory);

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, HttpResponseMessage> ActAndCallUntil<
        TData,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, HttpRequestMessage> fn,
        Schedule schedule,
        Expression<Func<HttpResponseMessage, bool>> predicate,
        Func<HttpClient>? clientFactory = default
    )
        where TSyntax : struct, Syntax =>
        scenario.Act(async data =>
        {
            var compiledPred = predicate.Compile();
            foreach (var duration in schedule.Run())
            {
                var resp = await InternalCall(
                    fn(data),
                    clientFactory?.Invoke() ?? new HttpClient()
                );
                if (compiledPred(resp))
                    return resp;
                await Task.Delay((TimeSpan)duration);
            }

            throw new InvalidOperationException(
                $"Condition {predicate} was not met before the schedule completed"
            );
        });

    [Pure]
    internal static Scenario<TSyntax>.Acted<
        HttpRequestMessage,
        HttpResponseMessage
    > ActAndCallUntil<TSyntax>(
        this Scenario<TSyntax>.Arranged<HttpRequestMessage> scenario,
        Schedule schedule,
        Expression<Func<HttpResponseMessage, bool>> predicate,
        Func<HttpClient>? clientFactory = default
    )
        where TSyntax : struct, Syntax =>
        scenario.ActAndCallUntil(static _ => _, schedule, predicate, clientFactory);
}
