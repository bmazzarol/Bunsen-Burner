using System.Net;
using BunsenBurner.Logging;
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

    public static TestServer MirrorResponse(Sink? sink = default) =>
        CreateTestServer(
            nameof(MirrorResponse),
            async ctx =>
            {
                ctx.Response.Headers.Add("custom", "123");
                var reader = new StreamReader(ctx.Request.Body);
                await ctx.Response.WriteAsync(await reader.ReadToEndAsync());
            },
            sink
        );

    private static TestServer CreateTestServer(
        string name,
        RequestDelegate requestDelegate,
        Sink? sink = default
    ) =>
        TestServerBuilderOptions
            .New(
                name,
                configureHost: builder => builder.Configure(x => x.Run(requestDelegate)),
                sink: sink
            )
            .Build();

    private static Scenario<TSyntax>.Asserted<TRequest, ResponseContext> AssertOnResponse<
        TRequest,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<TRequest, ResponseContext> scenario,
        Action<TRequest, Response> assert
    )
        where TRequest : Request
        where TSyntax : struct, Syntax => scenario.Assert((req, ctx) => assert(req, ctx.Response));

    internal static Scenario<TSyntax>.Asserted<TRequest, Response> IsOk<TRequest, TSyntax>(
        this Scenario<TSyntax>.Acted<TRequest, Response> scenario
    )
        where TRequest : Request
        where TSyntax : struct, Syntax =>
        scenario.Assert((_, resp) => Assert.Equal(HttpStatusCode.OK, resp.Code));

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
        scenario.And((_, resp) => Assert.Equal(HttpStatusCode.OK, resp.Response.Code));

    internal static WireMockServer WithHelloWorld(this WireMockServer server)
    {
        server
            .Given(WireMock.RequestBuilders.Request.Create().WithPath("/hello-world").UsingGet())
            .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
        return server;
    }
}
