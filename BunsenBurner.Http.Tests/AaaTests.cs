using System.Net;
using WireMock.Server;
using Flurl;
using JWT.Builder;
using static BunsenBurner.Http.Tests.Shared;
using static BunsenBurner.Aaa;

namespace BunsenBurner.Http.Tests;

public static class AaaTests
{
    [Fact(DisplayName = "GET request can be made to a test server")]
    public static async Task Case1() =>
        await Req.Get
            .To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString())
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(async ctx =>
            {
                Assert.Equal(HttpStatusCode.OK, ctx.Response.StatusCode);
                var content = await ctx.Response.AsContent();
                Assert.Equal("test", content);
                Assert.Equal(4, content.Length);
                Assert.Equal("123", ctx.Response.Headers.GetAsString("custom"));
                Assert.Empty(ctx.Response.Headers.Get("customNotThere"));
            });

    [Fact(DisplayName = "GET request can be made to a test server, with a named test")]
    public static async Task Case2() =>
        await "Some description"
            .ArrangeRequest(
                Req.Get
                    .To("/hello-world".SetQueryParam("a", 1))
                    .WithHeader("b", 123, x => x.ToString())
            )
            .ActAndCall(SimpleResponse())
            .IsOk();

    [Fact(DisplayName = "POST request can be made to a test server")]
    public static async Task Case3() =>
        await Req.Post
            .To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString())
            .WithJsonContent(new { A = "1" })
            .ArrangeRequest()
            .ActAndCall(MirrorResponse())
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "PUT request can be made to a test server")]
    public static async Task Case4() =>
        await Req.Put
            .To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString())
            .WithJsonContent(new { A = "1" })
            .ArrangeRequest()
            .ActAndCall(MirrorResponse())
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "PATCH request can be made to a test server")]
    public static async Task Case5() =>
        await Req.Patch
            .To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString())
            .WithTextContent("hello")
            .ArrangeRequest()
            .ActAndCall(MirrorResponse())
            .IsOk()
            .ResponseContentMatchesRequestBody();

    [Fact(DisplayName = "DELETE request can be made to a test server")]
    public static async Task Case6() =>
        await Req.Delete
            .To("/hello-world")
            .WithHeader("A", "1", "2")
            .WithHeader("B", "2", "3")
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .IsOk()
            .And(r => r.Response.Content.Headers.ContentType == null);

    [Fact(DisplayName = "OPTION request can be made to a test server")]
    public static async Task Case7() =>
        await Req.Options.To("/hello-world").ArrangeRequest().ActAndCall(SimpleResponse()).IsOk();

    [Fact(DisplayName = "HEAD request can be made to a test server")]
    public static async Task Case8() =>
        await Req.Head.To("/hello-world").ArrangeRequest().ActAndCall(SimpleResponse()).IsOk();

    [Fact(DisplayName = "TRACE request can be made to a test server")]
    public static async Task Case9() =>
        await Req.Trace
            .To("/hello-world")
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .IsOk()
            .And(async x =>
            {
                var content = await x.Response.AsContent();
                Assert.True(content.Length > 0);
            });

    [Fact(DisplayName = "GET request can be made to a real server")]
    public static async Task Case11() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                server.WithHelloWorld();
                return Req.Get.To($"{server.Urls.First()}/hello-world");
            })
            .ActAndCall()
            .IsOk();

    [Fact(DisplayName = "GET request can be made to a real server, with mixed data")]
    public static async Task Case12() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                server.WithHelloWorld();
                return (
                    Req: Req.Get.To($"{server.Urls.First()}/hello-world"),
                    SomeOtherData: "test"
                );
            })
            .ActAndCall(x => x.Req)
            .Assert(resp => Assert.Equal(HttpStatusCode.OK, resp.StatusCode));

    [Fact(DisplayName = "GET request can be made to a test server, with mixed data")]
    public static async Task Case13() =>
        await Arrange(() => (Req: Req.Get.To("/hello-world"), SomeOtherData: "test"))
            .ActAndCall(x => x.Req, _ => SimpleResponse())
            .Assert(ctx => Assert.Equal(HttpStatusCode.OK, ctx.Response.StatusCode));

    [Fact(DisplayName = "GET request can be made with mixed data")]
    public static async Task Case14() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                server.WithHelloWorld();
                return (
                    Req: Req.Get.To($"{server.Urls.First()}/hello-world"),
                    SomeOtherData: "test"
                );
            })
            .ActAndCall(x => x.Req, () => new HttpClient().WithoutSslCertChecks())
            .Assert(resp => Assert.Equal(HttpStatusCode.OK, resp.StatusCode));

    [Fact(DisplayName = "POST request can be made to a real server")]
    public static async Task Case15() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                server
                    .Given(
                        WireMock.RequestBuilders.Request
                            .Create()
                            .WithPath("/hello-world")
                            .UsingPost()
                    )
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return Req.Post
                    .To($"{server.Urls.First()}/hello-world")
                    .WithJsonContent(new { A = "test" });
            })
            .ActAndCall()
            .IsOk();

    [Fact(DisplayName = "Authorized request can be made")]
    public static async Task Case16() =>
        await Req.Get
            .To("/hello-world")
            .WithBearerToken(
                Token
                    .New()
                    .WithClaim(ClaimName.Subject, 1, 2, 3)
                    .WithHeader(HeaderName.KeyId, "1", "2", "3")
            )
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(ctx => Assert.Equal(HttpStatusCode.OK, ctx.Response.StatusCode))
            .And(
                (req, _) =>
                {
                    Assert.Contains(
                        req.Headers,
                        h =>
                            h.Key == "Authorization"
                            && req.Headers.GetAsString("Authorization") != string.Empty
                    );
                    var token = Token.FromRaw(req.Headers.GetAsString("Authorization")!);
                    Assert.Equal("[1,2,3]", token?.Claims.Find("sub").First().Case.ToString());
                    Assert.Equal(
                        @"[""1"",""2"",""3""]",
                        token?.Headers.Find("kid").First().Case.ToString()
                    );
                }
            );

    [Fact(DisplayName = "Authorized request can be made and configured by function")]
    public static async Task Case17() =>
        await Req.Get
            .To("/hello-world")
            .WithBearerToken(
                t =>
                    t.WithClaim(ClaimName.Subject, "123").WithHeader(HeaderName.ContentType, "text")
            )
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(ctx => Assert.Equal(HttpStatusCode.OK, ctx.Response.StatusCode))
            .And(
                (req, _) =>
                {
                    Assert.Contains(req.Headers, h => h.Key == "Authorization" && h.Value.Any());
                    Assert.NotNull(
                        Token.FromRaw(
                            req.Headers.GetAsString("Authorization")?.Replace("Bearer ", "")
                                ?? string.Empty
                        )
                    );
                    Assert.Null(
                        Token.FromRaw(
                            req.Headers.GetAsString("Authorization")?.Replace("Bearer ", "")
                                + ".extra"
                        )
                    );
                }
            );

    [Fact(DisplayName = "Repeated GET requests can be made to a real server")]
    public static async Task Case18() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                var req = WireMock.RequestBuilders.Request
                    .Create()
                    .WithPath("/hello-world")
                    .UsingGet();
                server
                    .Given(req)
                    .InScenario(nameof(Case18))
                    .WillSetStateTo(1)
                    .RespondWith(
                        WireMock.ResponseBuilders.Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case18))
                    .WhenStateIs(1)
                    .WillSetStateTo(2)
                    .RespondWith(
                        WireMock.ResponseBuilders.Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case18))
                    .WhenStateIs(2)
                    .WillSetStateTo(3)
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return Req.Get.To($"{server.Urls.First()}/hello-world");
            })
            .ActAndCallUntil(
                Schedule.spaced(10 * ms) & Schedule.recurs(4),
                resp => resp.StatusCode == HttpStatusCode.OK
            )
            .IsOk();

    [Fact(DisplayName = "Repeated GET requests can be made to a real server with extra data")]
    public static async Task Case19() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                var req = WireMock.RequestBuilders.Request
                    .Create()
                    .WithPath("/hello-world")
                    .UsingGet();
                server
                    .Given(req)
                    .InScenario(nameof(Case19))
                    .WillSetStateTo(1)
                    .RespondWith(
                        WireMock.ResponseBuilders.Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case19))
                    .WhenStateIs(1)
                    .WillSetStateTo(2)
                    .RespondWith(
                        WireMock.ResponseBuilders.Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case19))
                    .WhenStateIs(2)
                    .WillSetStateTo(3)
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return server.Urls.First();
            })
            .ActAndCallUntil(
                url => Req.Get.To($"{url}/hello-world"),
                Schedule.spaced(10 * ms) & Schedule.recurs(4),
                resp => resp.StatusCode == HttpStatusCode.OK
            )
            .Assert(resp => resp.StatusCode == HttpStatusCode.OK);

    [Fact(DisplayName = "Repeated GET requests can fail after the schedule completes")]
    public static async Task Case20() =>
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () =>
                await WireMockServer
                    .Start()
                    .ArrangeData()
                    .And(server =>
                    {
                        var req = WireMock.RequestBuilders.Request
                            .Create()
                            .WithPath("/hello-world")
                            .UsingGet();
                        var resp = WireMock.ResponseBuilders.Response
                            .Create()
                            .WithStatusCode(HttpStatusCode.InternalServerError);
                        server
                            .Given(req)
                            .InScenario(nameof(Case20))
                            .WillSetStateTo(1)
                            .RespondWith(resp);
                        server
                            .Given(req)
                            .InScenario(nameof(Case20))
                            .WhenStateIs(1)
                            .WillSetStateTo(2)
                            .RespondWith(resp);
                        server
                            .Given(req)
                            .InScenario(nameof(Case20))
                            .WhenStateIs(2)
                            .WillSetStateTo(3)
                            .RespondWith(resp);
                        return Req.Get.To($"{server.Urls.First()}/hello-world");
                    })
                    .ActAndCallUntil(
                        Schedule.spaced(10 * ms) & Schedule.Once,
                        resp => resp.StatusCode == HttpStatusCode.OK
                    )
                    .Assert(resp => resp.StatusCode == HttpStatusCode.OK)
        );
}
