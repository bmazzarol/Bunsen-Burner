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
                ctx.Response.Headers["custom"] = "123";
                await ctx.Response.WriteAsync("test", cancellationToken: ctx.RequestAborted);
            }
        );

    public static TestServer MirrorResponse(Sink? sink = default) =>
        CreateTestServer(
            nameof(MirrorResponse),
            async ctx =>
            {
                ctx.Response.Headers["custom"] = "123";
                var reader = new StreamReader(ctx.Request.Body);
                await ctx.Response.WriteAsync(
                    await reader.ReadToEndAsync(),
                    cancellationToken: ctx.RequestAborted
                );
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

    internal static WireMockServer WithHelloWorld(this WireMockServer server)
    {
        server
            .Given(WireMock.RequestBuilders.Request.Create().WithPath("/hello-world").UsingGet())
            .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
        return server;
    }

    internal static void ResponseCodeIsOk(ResponseContext context)
    {
        ResponseCodeIsOk(context.Response);
    }

    internal static void ResponseCodeIsOk(HttpResponseMessage response)
    {
        response.StatusCode.Should().Be(Resp.OK);
    }

    internal static async Task ResponseContentMatchesRequestBody(
        HttpRequestMessage requestMessage,
        ResponseContext responseContext
    )
    {
        var body = await requestMessage.Content!.ReadAsStringAsync();
        var content = await responseContext.Response.Content.ReadAsStringAsync();
        body.Should().Be(content);
    }
}
