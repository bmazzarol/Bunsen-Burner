using System.Linq.Expressions;
using BunsenBurner.Logging;
using LanguageExt;
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
        var httpResp = await client.SendAsync(request.HttpRequestMessage());
        return await Response.New(httpResp);
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
                var resp = await InternalCall(fn(data), server.CreateClient());
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
        Func<TData, TRequest> fn,
        Func<HttpClient>? clientFactory = default
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            data => InternalCall(fn(data), clientFactory?.Invoke() ?? new HttpClient())
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<TRequest, Response> ActAndCall<TSyntax, TRequest>(
        this Scenario<TSyntax>.Arranged<TRequest> scenario,
        Func<HttpClient>? clientFactory = default
    )
        where TRequest : Request
        where TSyntax : struct, Syntax => scenario.ActAndCall(static _ => _, clientFactory);

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, Response> ActAndCallUntil<
        TData,
        TRequest,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TRequest> fn,
        Schedule schedule,
        Expression<Func<Response, bool>> predicate,
        Func<HttpClient>? clientFactory = default
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            async data =>
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
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<TRequest, Response> ActAndCallUntil<TSyntax, TRequest>(
        this Scenario<TSyntax>.Arranged<TRequest> scenario,
        Schedule schedule,
        Expression<Func<Response, bool>> predicate,
        Func<HttpClient>? clientFactory = default
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        scenario.ActAndCallUntil(static _ => _, schedule, predicate, clientFactory);
}
