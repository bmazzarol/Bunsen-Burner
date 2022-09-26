using System.Net;
using WireMock.Server;
using static BunsenBurner.Http.Tests.Shared;
using Flurl;
using static BunsenBurner.Bdd;

namespace BunsenBurner.Http.Tests;

public class BddTests : IClassFixture<MockServerFixture>
{
    private readonly WireMockServer _server;

    public BddTests(MockServerFixture fixture) => _server = fixture.Server;

    [Fact(DisplayName = "GET request can be made to a test server")]
    public async Task Case1() =>
        await Request
            .GET("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString())
            .GivenRequest()
            .WhenCalled(SimpleResponse.Value)
            .Then(resp =>
            {
                Assert.Equal(HttpStatusCode.OK, resp.Code);
                Assert.Equal("test", resp.Content);
                Assert.Equal("123", resp.Headers.FirstOrDefault(x => x.Key == "custom")?.Value);
            });

    [Fact(DisplayName = "GET request can be made to a test server, with a named test")]
    public async Task Case2() =>
        await "Some description"
            .GivenRequest(
                Request
                    .GET("/hello-world".SetQueryParam("a", 1))
                    .WithHeader("b", 123, x => x.ToString())
            )
            .WhenCalled(SimpleResponse.Value)
            .IsOk();

    [Fact(DisplayName = "POST request can be made to a test server")]
    public async Task Case3() =>
        await Request
            .POST("/hello-world".SetQueryParam("a", 1), new { A = "1" })
            .WithHeader("b", 123, x => x.ToString())
            .GivenRequest()
            .WhenCalled(MirrorResponse.Value)
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "PUT request can be made to a test server")]
    public async Task Case4() =>
        await Request
            .PUT("/hello-world".SetQueryParam("a", 1), new { A = "1" })
            .WithHeader("b", 123, x => x.ToString())
            .GivenRequest()
            .WhenCalled(MirrorResponse.Value)
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "PATCH request can be made to a test server")]
    public async Task Case5() =>
        await Request
            .PATCH("/hello-world".SetQueryParam("a", 1), new { A = "1" })
            .WithHeader("b", 123, x => x.ToString())
            .GivenRequest()
            .WhenCalled(MirrorResponse.Value)
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "DELETE request can be made to a test server")]
    public async Task Case6() =>
        await Request
            .DELETE("/hello-world")
            .WithHeaders(new Header("A", "1"), new Header("B", "2"))
            .GivenRequest()
            .WhenCalled(SimpleResponse.Value)
            .IsOk();

    [Fact(DisplayName = "OPTION request can be made to a test server")]
    public async Task Case7() =>
        await Request.OPTION("/hello-world").GivenRequest().WhenCalled(SimpleResponse.Value).IsOk();

    [Fact(DisplayName = "HEAD request can be made to a test server")]
    public async Task Case8() =>
        await Request.HEAD("/hello-world").GivenRequest().WhenCalled(SimpleResponse.Value).IsOk();

    [Fact(DisplayName = "TRACE request can be made to a test server")]
    public async Task Case9() =>
        await Request.TRACE("/hello-world").GivenRequest().WhenCalled(SimpleResponse.Value).IsOk();

    [Fact(DisplayName = "CONNECT request can be made to a test server")]
    public async Task Case10() =>
        await Request
            .CONNECT("/hello-world")
            .WithHeader("a", "1")
            .WithHeader("a", "2")
            .WithHeader("a", "1")
            .GivenRequest()
            .WhenCalled(SimpleResponse.Value)
            .IsOk();

    [Fact(DisplayName = "GET request can be made to a real server")]
    public async Task Case11() =>
        await Given(() =>
            {
                _server.WithHelloWorld();
                return Request.GET($"{_server.Urls.First()}/hello-world");
            })
            .WhenCalled()
            .IsOk();

    [Fact(DisplayName = "GET request can be made to a real server, with mixed data")]
    public async Task Case12() =>
        await Given(() =>
            {
                _server.WithHelloWorld();
                return (
                    Req: Request.GET($"{_server.Urls.First()}/hello-world"),
                    SomeOtherData: "test"
                );
            })
            .WhenCalled(x => x.Req)
            .Then(resp => Assert.Equal(HttpStatusCode.OK, resp.Code));

    [Fact(DisplayName = "GET request can be made to a test server, with mixed data")]
    public async Task Case13() =>
        await Given(() =>
            {
                _server.WithHelloWorld();
                return (
                    Req: Request.GET($"{_server.Urls.First()}/hello-world"),
                    SomeOtherData: "test"
                );
            })
            .WhenCalled(x => x.Req, SimpleResponse.Value)
            .Then(resp => Assert.Equal(HttpStatusCode.OK, resp.Code));
}
