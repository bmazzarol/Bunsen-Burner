using System.Net;
using WireMock.Server;
using Flurl;
using static BunsenBurner.Http.Tests.Shared;
using static BunsenBurner.Aaa;

namespace BunsenBurner.Http.Tests;

public sealed class AaaTests : IClassFixture<MockServerFixture>
{
    private readonly WireMockServer _server;

    public AaaTests(MockServerFixture fixture) => _server = fixture.Server;

    [Fact(DisplayName = "GET request can be made to a test server")]
    public async Task Case1() =>
        await Request
            .GET("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString())
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(ctx =>
            {
                Assert.Equal(HttpStatusCode.OK, ctx.Response.Code);
                Assert.Equal(200, ctx.Response.RawStatusCode);
                Assert.Equal("test", ctx.Response.Content);
                Assert.Equal(4, ctx.Response.Length);
                Assert.Equal(
                    "123",
                    ctx.Response.Headers.FirstOrDefault(x => x.Key == "custom")?.Value
                );
            });

    [Fact(DisplayName = "GET request can be made to a test server, with a named test")]
    public async Task Case2() =>
        await "Some description"
            .ArrangeRequest(
                Request
                    .GET("/hello-world".SetQueryParam("a", 1))
                    .WithHeader("b", 123, x => x.ToString())
            )
            .ActAndCall(SimpleResponse())
            .IsOk();

    [Fact(DisplayName = "POST request can be made to a test server")]
    public async Task Case3() =>
        await Request
            .POST("/hello-world".SetQueryParam("a", 1), new { A = "1" })
            .WithHeader("b", 123, x => x.ToString())
            .ArrangeRequest()
            .ActAndCall(MirrorResponse())
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "PUT request can be made to a test server")]
    public async Task Case4() =>
        await Request
            .PUT("/hello-world".SetQueryParam("a", 1), new { A = "1" })
            .WithHeader("b", 123, x => x.ToString())
            .ArrangeRequest()
            .ActAndCall(MirrorResponse())
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "PATCH request can be made to a test server")]
    public async Task Case5() =>
        await Request
            .PATCH("/hello-world".SetQueryParam("a", 1), new { A = "1" })
            .WithHeader("b", 123, x => x.ToString())
            .ArrangeRequest()
            .ActAndCall(MirrorResponse())
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "DELETE request can be made to a test server")]
    public async Task Case6() =>
        await Request
            .DELETE("/hello-world")
            .WithHeaders(new Header("A", "1"), new Header("B", "2"))
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .IsOk();

    [Fact(DisplayName = "OPTION request can be made to a test server")]
    public async Task Case7() =>
        await Request.OPTION("/hello-world").ArrangeRequest().ActAndCall(SimpleResponse()).IsOk();

    [Fact(DisplayName = "HEAD request can be made to a test server")]
    public async Task Case8() =>
        await Request.HEAD("/hello-world").ArrangeRequest().ActAndCall(SimpleResponse()).IsOk();

    [Fact(DisplayName = "TRACE request can be made to a test server")]
    public async Task Case9() =>
        await Request.TRACE("/hello-world").ArrangeRequest().ActAndCall(SimpleResponse()).IsOk();

    [Fact(DisplayName = "CONNECT request can be made to a test server")]
    public async Task Case10() =>
        await Request
            .CONNECT("/hello-world")
            .WithHeader("a", "1")
            .WithHeader("a", "2")
            .WithHeader("a", "1")
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .IsOk();

    [Fact(DisplayName = "GET request can be made to a real server")]
    public async Task Case11() =>
        await Arrange(() =>
            {
                _server.WithHelloWorld();
                return Request.GET($"{_server.Urls.First()}/hello-world");
            })
            .ActAndCall()
            .IsOk();

    [Fact(DisplayName = "GET request can be made to a real server, with mixed data")]
    public async Task Case12() =>
        await Arrange(() =>
            {
                _server.WithHelloWorld();
                return (
                    Req: Request.GET($"{_server.Urls.First()}/hello-world"),
                    SomeOtherData: "test"
                );
            })
            .ActAndCall(x => x.Req)
            .Assert(resp => Assert.Equal(HttpStatusCode.OK, resp.Code));

    [Fact(DisplayName = "GET request can be made to a test server, with mixed data")]
    public async Task Case13() =>
        await Arrange(() => (Req: Request.GET($"/hello-world"), SomeOtherData: "test"))
            .ActAndCall(x => x.Req, SimpleResponse())
            .Assert(ctx => Assert.Equal(HttpStatusCode.OK, ctx.Response.Code));

    [Fact(DisplayName = "GET request can be made with mixed data")]
    public async Task Case14() =>
        await Arrange(() =>
            {
                _server.WithHelloWorld();
                return (
                    Req: Request.GET($"{_server.Urls.First()}/hello-world"),
                    SomeOtherData: "test"
                );
            })
            .ActAndCall(x => x.Req)
            .Assert(resp => Assert.Equal(HttpStatusCode.OK, resp.Code));

    [Fact(DisplayName = "POST request can be made to a real server")]
    public async Task Case15() =>
        await Arrange(() =>
            {
                _server
                    .Given(
                        WireMock.RequestBuilders.Request
                            .Create()
                            .WithPath("/hello-world")
                            .UsingPost()
                    )
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return Request.POST($"{_server.Urls.First()}/hello-world", new { A = "test" });
            })
            .ActAndCall()
            .IsOk();
}
