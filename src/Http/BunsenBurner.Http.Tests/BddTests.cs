using System.Diagnostics.CodeAnalysis;
using Eon;
using Flurl;
using WireMock.Server;
using static BunsenBurner.Bdd;
using static BunsenBurner.Http.Tests.Shared;

namespace BunsenBurner.Http.Tests;

public static class BddTests
{
    [Fact(DisplayName = "GET request can be made to a test server")]
    public static async Task Case1() =>
        await Req.Get
            .To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            .GivenRequest()
            .WhenCalled(SimpleResponse())
            .Then(async ctx =>
            {
                Assert.Equal(Resp.OK, ctx.Response.StatusCode);
                Assert.Equal("test", await ctx.Response.AsContent());
                Assert.Equal("123", ctx.Response.Headers.GetAsString("custom"));
            });

    [Fact(DisplayName = "GET request can be made to a test server, with a named test")]
    public static async Task Case2() =>
        await "Some description"
            .GivenRequest(
                Req.Get
                    .To("/hello-world".SetQueryParam("a", 1))
                    .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            )
            .WhenCalled(SimpleResponse())
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "POST request can be made to a test server")]
    public static async Task Case3() =>
        await Req.Post
            .To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            .WithJsonContent(new { A = "1" })
            .GivenRequest()
            .WhenCalled(MirrorResponse())
            .Assert(ResponseCodeIsOk)
            .And(ResponseContentMatchesRequestBody);

    [Fact(DisplayName = "PUT request can be made to a test server")]
    public static async Task Case4() =>
        await Req.Put
            .To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            .WithJsonContent(new { A = "1" })
            .GivenRequest()
            .WhenCalled(MirrorResponse())
            .Assert(ResponseCodeIsOk)
            .And(ResponseContentMatchesRequestBody);

    [Fact(DisplayName = "PATCH request can be made to a test server")]
    public static async Task Case5() =>
        await Req.Patch
            .To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            .WithJsonContent(new { A = "1" })
            .GivenRequest()
            .WhenCalled(MirrorResponse())
            .Assert(ResponseCodeIsOk)
            .And(ResponseContentMatchesRequestBody);

    [Fact(DisplayName = "DELETE request can be made to a test server")]
    public static async Task Case6() =>
        await Req.Delete
            .To("/hello-world")
            .WithHeader("A", "1")
            .WithHeader("B", "2")
            .GivenRequest()
            .WhenCalled(SimpleResponse())
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "OPTION request can be made to a test server")]
    public static async Task Case7() =>
        await Req.Options
            .To("/hello-world")
            .GivenRequest()
            .WhenCalled(SimpleResponse())
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "HEAD request can be made to a test server")]
    public static async Task Case8() =>
        await Req.Head
            .To("/hello-world")
            .GivenRequest()
            .WhenCalled(SimpleResponse())
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "TRACE request can be made to a test server")]
    public static async Task Case9() =>
        await Req.Trace
            .To("/hello-world")
            .GivenRequest()
            .WhenCalled(SimpleResponse())
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "GET request can be made to a real server")]
    public static async Task Case11() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                server.WithHelloWorld();
                return Req.Get.To($"{server.Urls[0]}/hello-world");
            })
            .WhenCalled()
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "GET request can be made to a real server, with mixed data")]
    public static async Task Case12() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                server.WithHelloWorld();
                return (Req: Req.Get.To($"{server.Urls[0]}/hello-world"), SomeOtherData: "test");
            })
            .WhenCalled(x => x.Req)
            .Then(resp => Assert.Equal(Resp.OK, resp.StatusCode));

    [Fact(DisplayName = "GET request can be made to a test server, with mixed data")]
    public static async Task Case13() =>
        await Given(() => (Req: Req.Get.To("/hello-world"), SomeOtherData: "test"))
            .WhenCalled(x => x.Req, _ => SimpleResponse())
            .Then(ctx => Assert.Equal(Resp.OK, ctx.Response.StatusCode));

    [Fact(DisplayName = "GET request can be made with mixed data")]
    public static async Task Case14() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                server.WithHelloWorld();
                return (Req: Req.Get.To($"{server.Urls[0]}/hello-world"), SomeOtherData: "test");
            })
            .WhenCalled(x => x.Req)
            .Then(resp => Assert.Equal(Resp.OK, resp.StatusCode));

    [Fact(DisplayName = "Repeated GET requests can be made to a real server")]
    public static async Task Case15() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                var req = WireMock
                    .RequestBuilders
                    .Request
                    .Create()
                    .WithPath("/hello-world")
                    .UsingGet();
                server
                    .Given(req)
                    .InScenario(nameof(Case15))
                    .WillSetStateTo(1)
                    .RespondWith(
                        WireMock
                            .ResponseBuilders
                            .Response
                            .Create()
                            .WithStatusCode(Resp.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case15))
                    .WhenStateIs(1)
                    .WillSetStateTo(2)
                    .RespondWith(
                        WireMock
                            .ResponseBuilders
                            .Response
                            .Create()
                            .WithStatusCode(Resp.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case15))
                    .WhenStateIs(2)
                    .WillSetStateTo(3)
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return Req.Get.To($"{server.Urls[0]}/hello-world");
            })
            .WhenCalledRepeatedly(
                Schedule.Spaced(TimeSpan.FromMilliseconds(10)) & Schedule.Recurs(4),
                resp => resp.StatusCode == Resp.OK
            )
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "Repeated GET requests can be made to a real server with extra data")]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
    public static async Task Case16() =>
        await WireMockServer
            .Start()
            .GivenData()
            .And(server =>
            {
                var req = WireMock
                    .RequestBuilders
                    .Request
                    .Create()
                    .WithPath("/hello-world")
                    .UsingGet();
                server
                    .Given(req)
                    .InScenario(nameof(Case16))
                    .WillSetStateTo(1)
                    .RespondWith(
                        WireMock
                            .ResponseBuilders
                            .Response
                            .Create()
                            .WithStatusCode(Resp.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case16))
                    .WhenStateIs(1)
                    .WillSetStateTo(2)
                    .RespondWith(
                        WireMock
                            .ResponseBuilders
                            .Response
                            .Create()
                            .WithStatusCode(Resp.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case16))
                    .WhenStateIs(2)
                    .WillSetStateTo(3)
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return server.Urls[0];
            })
            .WhenCalledRepeatedly(
                url => Req.Get.To($"{url}/hello-world"),
                Schedule.Spaced(TimeSpan.FromMilliseconds(10)) & Schedule.Recurs(4),
                resp => resp.StatusCode == Resp.OK
            )
            .Then(ResponseCodeIsOk);
}
