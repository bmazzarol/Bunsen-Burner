using System.Net;
using WireMock.Server;
using static BunsenBurner.Http.Tests.Shared;
using Flurl;
using static BunsenBurner.Bdd;

namespace BunsenBurner.Http.Tests;

public static class BddTests
{
    [Fact(DisplayName = "GET request can be made to a test server")]
    public static async Task Case1() =>
        await Request
            .GET("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString())
            .GivenRequest()
            .WhenCalled(SimpleResponse())
            .Then(ctx =>
            {
                Assert.Equal(HttpStatusCode.OK, ctx.Response.Code);
                Assert.Equal("test", ctx.Response.Content);
                Assert.Equal("123", ctx.Response.Headers.Get("custom"));
            });

    [Fact(DisplayName = "GET request can be made to a test server, with a named test")]
    public static async Task Case2() =>
        await "Some description"
            .GivenRequest(
                Request
                    .GET("/hello-world".SetQueryParam("a", 1))
                    .WithHeader("b", 123, x => x.ToString())
            )
            .WhenCalled(SimpleResponse())
            .IsOk();

    [Fact(DisplayName = "POST request can be made to a test server")]
    public static async Task Case3() =>
        await Request
            .POST("/hello-world".SetQueryParam("a", 1), new { A = "1" })
            .WithHeader("b", 123, x => x.ToString())
            .GivenRequest()
            .WhenCalled(MirrorResponse())
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "PUT request can be made to a test server")]
    public static async Task Case4() =>
        await Request
            .PUT("/hello-world".SetQueryParam("a", 1), new { A = "1" })
            .WithHeader("b", 123, x => x.ToString())
            .WithUrl(x => x.SetQueryParams(new { b = 2 }))
            .GivenRequest()
            .WhenCalled(MirrorResponse())
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "PATCH request can be made to a test server")]
    public static async Task Case5() =>
        await Request
            .PATCH("/hello-world".SetQueryParam("a", 1), new { A = "1" })
            .WithHeader("b", 123, x => x.ToString())
            .GivenRequest()
            .WhenCalled(MirrorResponse())
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "DELETE request can be made to a test server")]
    public static async Task Case6() =>
        await Request
            .DELETE("/hello-world2")
            .WithHeader("A", "1")
            .WithHeader("B", "2")
            .WithUrl("/hello-world")
            .GivenRequest()
            .WhenCalled(SimpleResponse())
            .IsOk();

    [Fact(DisplayName = "OPTION request can be made to a test server")]
    public static async Task Case7() =>
        await Request.OPTION("/hello-world").GivenRequest().WhenCalled(SimpleResponse()).IsOk();

    [Fact(DisplayName = "HEAD request can be made to a test server")]
    public static async Task Case8() =>
        await Request.HEAD("/hello-world").GivenRequest().WhenCalled(SimpleResponse()).IsOk();

    [Fact(DisplayName = "TRACE request can be made to a test server")]
    public static async Task Case9() =>
        await Request.TRACE("/hello-world").GivenRequest().WhenCalled(SimpleResponse()).IsOk();

    [Fact(DisplayName = "CONNECT request can be made to a test server")]
    public static async Task Case10() =>
        await Request
            .CONNECT("/hello-world")
            .WithHeader("a", "1")
            .WithHeader("a", "2")
            .WithHeader("a", "1")
            .GivenRequest()
            .WhenCalled(SimpleResponse())
            .IsOk();

    [Fact(DisplayName = "GET request can be made to a real server")]
    public static async Task Case11() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                server.WithHelloWorld();
                return Request.GET($"{server.Urls.First()}/hello-world");
            })
            .WhenCalled()
            .IsOk();

    [Fact(DisplayName = "GET request can be made to a real server, with mixed data")]
    public static async Task Case12() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                server.WithHelloWorld();
                return (
                    Req: Request.GET($"{server.Urls.First()}/hello-world"),
                    SomeOtherData: "test"
                );
            })
            .WhenCalled(x => x.Req)
            .Then(resp => Assert.Equal(HttpStatusCode.OK, resp.Code));

    [Fact(DisplayName = "GET request can be made to a test server, with mixed data")]
    public static async Task Case13() =>
        await Given(() => (Req: Request.GET($"/hello-world"), SomeOtherData: "test"))
            .WhenCalled(x => x.Req, SimpleResponse())
            .Then(ctx => Assert.Equal(HttpStatusCode.OK, ctx.Response.Code));

    [Fact(DisplayName = "GET request can be made with mixed data")]
    public static async Task Case14() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                server.WithHelloWorld();
                return (
                    Req: Request.GET($"{server.Urls.First()}/hello-world"),
                    SomeOtherData: "test"
                );
            })
            .WhenCalled(x => x.Req)
            .Then(resp => Assert.Equal(HttpStatusCode.OK, resp.Code));

    [Fact(DisplayName = "Repeated GET requests can be made to a real server")]
    public static async Task Case15() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                var req = WireMock.RequestBuilders.Request
                    .Create()
                    .WithPath("/hello-world")
                    .UsingGet();
                server
                    .Given(req)
                    .InScenario(nameof(Case15))
                    .WillSetStateTo(1)
                    .RespondWith(
                        WireMock.ResponseBuilders.Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case15))
                    .WhenStateIs(1)
                    .WillSetStateTo(2)
                    .RespondWith(
                        WireMock.ResponseBuilders.Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case15))
                    .WhenStateIs(2)
                    .WillSetStateTo(3)
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return Request.GET($"{server.Urls.First()}/hello-world");
            })
            .WhenCalledRepeatedly(
                Schedule.spaced(10 * ms) & Schedule.recurs(4),
                resp => resp.Code == HttpStatusCode.OK
            )
            .IsOk();

    [Fact(DisplayName = "Repeated GET requests can be made to a real server with extra data")]
    public static async Task Case16() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                var req = WireMock.RequestBuilders.Request
                    .Create()
                    .WithPath("/hello-world")
                    .UsingGet();
                server
                    .Given(req)
                    .InScenario(nameof(Case16))
                    .WillSetStateTo(1)
                    .RespondWith(
                        WireMock.ResponseBuilders.Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case16))
                    .WhenStateIs(1)
                    .WillSetStateTo(2)
                    .RespondWith(
                        WireMock.ResponseBuilders.Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case16))
                    .WhenStateIs(2)
                    .WillSetStateTo(3)
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return server.Urls.First();
            })
            .WhenCalledRepeatedly(
                url => Request.GET($"{url}/hello-world"),
                Schedule.spaced(10 * ms) & Schedule.recurs(4),
                resp => resp.Code == HttpStatusCode.OK
            )
            .Then(resp => resp.Code == HttpStatusCode.OK);
}
