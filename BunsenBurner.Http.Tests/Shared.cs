using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using WireMock.Server;

namespace BunsenBurner.Http.Tests;

internal static class Shared
{
    public static TestServer SimpleResponse() =>
        CreateTestServer(
            nameof(SimpleResponse),
            async ctx =>
            {
                ctx.Response.Headers.Add("custom", "123");
                await ctx.Response.WriteAsync("test");
            }
        );

    public static TestServer MirrorResponse() =>
        CreateTestServer(
            nameof(MirrorResponse),
            async ctx =>
            {
                ctx.Response.Headers.Add("custom", "123");
                var reader = new StreamReader(ctx.Request.Body);
                await ctx.Response.WriteAsync(await reader.ReadToEndAsync());
            }
        );

    private static TestServer CreateTestServer(string name, RequestDelegate requestDelegate) =>
        TestServerBuilderOptions
            .New(name, configureHost: builder => builder.Configure(x => x.Run(requestDelegate)))
            .Build();

    private static Scenario<TSyntax>.Asserted<TRequest, ResponseContext> AssertOnResponse<
        TRequest,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<TRequest, ResponseContext> scenario,
        Action<TRequest, Response> assert
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            scenario.ActOnScenario,
            (req, ctx) =>
            {
                assert(req, ctx.Response);
                return Task.CompletedTask;
            }
        );

    private static Scenario<TSyntax>.Asserted<TRequest, Response> AssertOnResponse<
        TRequest,
        TSyntax
    >(this Scenario<TSyntax>.Acted<TRequest, Response> scenario, Action<TRequest, Response> assert)
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            scenario.ArrangeScenario,
            scenario.ActOnScenario,
            (req, resp) =>
            {
                assert(req, resp);
                return Task.CompletedTask;
            }
        );

    private static Scenario<TSyntax>.Asserted<TRequest, ResponseContext> AssertOnResponse<
        TRequest,
        TSyntax
    >(
        this Scenario<TSyntax>.Asserted<TRequest, ResponseContext> scenario,
        Action<TRequest, Response> assert
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        scenario with
        {
            AssertAgainstResult = (req, ctx) =>
            {
                assert(req, ctx.Response);
                return Task.CompletedTask;
            }
        };

    internal static Scenario<TSyntax>.Asserted<TRequest, Response> IsOk<TRequest, TSyntax>(
        this Scenario<TSyntax>.Acted<TRequest, Response> scenario
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        scenario.AssertOnResponse((_, resp) => Assert.Equal(HttpStatusCode.OK, resp.Code));

    internal static Scenario<TSyntax>.Asserted<TRequest, ResponseContext> IsOk<TRequest, TSyntax>(
        this Scenario<TSyntax>.Acted<TRequest, ResponseContext> scenario
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        scenario.AssertOnResponse((_, resp) => Assert.Equal(HttpStatusCode.OK, resp.Code));

    internal static Scenario<TSyntax>.Asserted<
        TRequest,
        ResponseContext
    > ResponseContentMatchesRequestBody<TRequest, TSyntax>(
        this Scenario<TSyntax>.Asserted<TRequest, ResponseContext> scenario
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        scenario.AssertOnResponse((_, resp) => Assert.Equal(HttpStatusCode.OK, resp.Code));

    internal static WireMockServer WithHelloWorld(this WireMockServer server)
    {
        server
            .Given(WireMock.RequestBuilders.Request.Create().WithPath("/hello-world").UsingGet())
            .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
        return server;
    }
}
