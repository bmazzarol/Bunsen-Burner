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

    private static Scenario<TSyntax>.Asserted<
        HttpRequestMessage,
        ResponseContext
    > AssertOnResponse<TSyntax>(
        this Scenario<TSyntax>.Acted<HttpRequestMessage, ResponseContext> scenario,
        Action<HttpRequestMessage, HttpResponseMessage> assert
    )
        where TSyntax : struct, Syntax => scenario.Assert((req, ctx) => assert(req, ctx.Response));

    internal static Scenario<TSyntax>.Asserted<
        HttpRequestMessage,
        HttpResponseMessage
    > IsOk<TSyntax>(this Scenario<TSyntax>.Acted<HttpRequestMessage, HttpResponseMessage> scenario)
        where TSyntax : struct, Syntax =>
        scenario.Assert((_, resp) => Assert.Equal(HttpStatusCode.OK, resp.StatusCode));

    internal static Scenario<TSyntax>.Asserted<HttpRequestMessage, ResponseContext> IsOk<TSyntax>(
        this Scenario<TSyntax>.Acted<HttpRequestMessage, ResponseContext> scenario
    )
        where TSyntax : struct, Syntax =>
        scenario.AssertOnResponse((_, resp) => Assert.Equal(HttpStatusCode.OK, resp.StatusCode));

    internal static Scenario<TSyntax>.Asserted<
        HttpRequestMessage,
        ResponseContext
    > ResponseContentMatchesRequestBody<TSyntax>(
        this Scenario<TSyntax>.Asserted<HttpRequestMessage, ResponseContext> scenario
    )
        where TSyntax : struct, Syntax =>
        scenario.And((_, resp) => Assert.Equal(HttpStatusCode.OK, resp.Response.StatusCode));

    internal static WireMockServer WithHelloWorld(this WireMockServer server)
    {
        server
            .Given(WireMock.RequestBuilders.Request.Create().WithPath("/hello-world").UsingGet())
            .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
        return server;
    }
}
