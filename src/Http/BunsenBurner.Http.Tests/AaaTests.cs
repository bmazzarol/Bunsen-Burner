using Eon;
using Flurl;
using JWT.Builder;
using WireMock.Server;
using static BunsenBurner.Aaa;
using static BunsenBurner.Http.Tests.Shared;

namespace BunsenBurner.Http.Tests;

public static class AaaTests
{
    [Fact(DisplayName = "GET request can be made to a test server")]
    public static async Task Case1() =>
        await Req.Get.To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(async ctx =>
            {
                ctx.Response.StatusCode.Should().Be(Resp.OK);
                var content = await ctx.Response.AsContent();
                content.Should().Be("test");
                content.Should().HaveLength(4);
                ctx.Response.Headers.GetAsString("custom").Should().Be("123");
                ctx.Response.Headers.Get("customNotThere").Should().BeEmpty();
            });

    [Fact(DisplayName = "GET request can be made to a test server, with a named test")]
    public static async Task Case2() =>
        await "Some description"
            .ArrangeRequest(
                Req.Get.To("/hello-world".SetQueryParam("a", 1))
                    .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            )
            .ActAndCall(SimpleResponse())
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "POST request can be made to a test server")]
    public static async Task Case3() =>
        await Req.Post.To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            .WithJsonContent(new { A = "1" })
            .ArrangeRequest()
            .ActAndCall(MirrorResponse())
            .Assert(ResponseCodeIsOk)
            .And(ResponseContentMatchesRequestBody);

    [Fact(DisplayName = "PUT request can be made to a test server")]
    public static async Task Case4() =>
        await Req.Put.To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            .WithJsonContent(new { A = "1" })
            .ArrangeRequest()
            .ActAndCall(MirrorResponse())
            .Assert(ResponseCodeIsOk)
            .And(ResponseContentMatchesRequestBody);

    [Fact(DisplayName = "PATCH request can be made to a test server")]
    public static async Task Case5() =>
        await Req.Patch.To("/hello-world".SetQueryParam("a", 1))
            .WithHeader("b", 123, x => x.ToString(InvariantCulture))
            .WithTextContent("hello")
            .ArrangeRequest()
            .ActAndCall(MirrorResponse())
            .Assert(ResponseCodeIsOk)
            .And(ResponseContentMatchesRequestBody);

    [Fact(DisplayName = "DELETE request can be made to a test server")]
    public static async Task Case6() =>
        await Req.Delete.To("/hello-world")
            .WithHeader("A", "1", "2")
            .WithHeader("B", "2", "3")
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(ResponseCodeIsOk)
            .And(r => r.Response.Content.Headers.ContentType == null);

    [Fact(DisplayName = "OPTION request can be made to a test server")]
    public static async Task Case7() =>
        await Req.Options.To("/hello-world")
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "HEAD request can be made to a test server")]
    public static async Task Case8() =>
        await Req.Head.To("/hello-world")
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "TRACE request can be made to a test server")]
    public static async Task Case9() =>
        await Req.Trace.To("/hello-world")
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(ResponseCodeIsOk)
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
                return Req.Get.To($"{server.Urls[0]}/hello-world");
            })
            .ActAndCall()
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "GET request can be made to a real server, with mixed data")]
    public static async Task Case12() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                server.WithHelloWorld();
                return (Req: Req.Get.To($"{server.Urls[0]}/hello-world"), SomeOtherData: "test");
            })
            .ActAndCall(x => x.Req)
            .Assert(resp => Assert.Equal(Resp.OK, resp.StatusCode));

    [Fact(DisplayName = "GET request can be made to a test server, with mixed data")]
    public static async Task Case13() =>
        await Arrange(() => (Req: Req.Get.To("/hello-world"), SomeOtherData: "test"))
            .ActAndCall(x => x.Req, _ => SimpleResponse())
            .Assert(ctx => Assert.Equal(Resp.OK, ctx.Response.StatusCode));

    [Fact(DisplayName = "GET request can be made with mixed data")]
    public static async Task Case14() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                server.WithHelloWorld();
                return (Req: Req.Get.To($"{server.Urls[0]}/hello-world"), SomeOtherData: "test");
            })
            .ActAndCall(x => x.Req, () => new HttpClient().WithoutSslCertChecks())
            .Assert(resp => Assert.Equal(Resp.OK, resp.StatusCode));

    [Fact(DisplayName = "POST request can be made to a real server")]
    public static async Task Case15() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                server
                    .Given(
                        WireMock
                            .RequestBuilders.Request.Create()
                            .WithPath("/hello-world")
                            .UsingPost()
                    )
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return Req.Post.To($"{server.Urls[0]}/hello-world")
                    .WithJsonContent(new { A = "test" });
            })
            .ActAndCall()
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "Authorized request can be made")]
    public static async Task Case16() =>
        await Req.Get.To("/hello-world")
            .WithBearerToken(
                Token
                    .New(lifetime: TimeSpan.FromDays(2))
                    .WithClaim(ClaimName.Subject, 1, 2, 3)
                    .WithHeader(HeaderName.KeyId, "1", "2", "3")
                    .WithClaim(ClaimName.Issuer, "Issuer A")
                    .WithClaim(ClaimName.Issuer, "Issuer B")
                    .WithClaim(ClaimName.Issuer, "Issuer C")
            )
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(ctx => Assert.Equal(Resp.OK, ctx.Response.StatusCode))
            .And(
                (req, _) =>
                {
                    req.Headers.Should()
                        .Contain(
                            h =>
                                string.Equals(h.Key, "Authorization", StringComparison.Ordinal)
                                && req.Headers.GetAsString("Authorization") != string.Empty
                        );
                    var token = Token.FromRaw(req.Headers.GetAsString("Authorization"));
                    token!.Claims["sub"].Value.ToString().Should().Be("[1,2,3]");
                    token.Headers["kid"].Value.ToString().Should().Be(@"[""1"",""2"",""3""]");
                    token
                        .Claims["iss"]
                        .Value.ToString()
                        .Should()
                        .Be(@"[""Issuer A"",""Issuer B"",""Issuer C""]");
                }
            );

    [Fact(DisplayName = "Authorized request can be made and configured by function")]
    public static async Task Case17() =>
        await Req.Get.To("/hello-world")
            .WithBearerToken(
                t =>
                    t.WithClaim(ClaimName.Subject, "123").WithHeader(HeaderName.ContentType, "text")
            )
            .ArrangeRequest()
            .ActAndCall(SimpleResponse())
            .Assert(ctx => Assert.Equal(Resp.OK, ctx.Response.StatusCode))
            .And(
                (req, _) =>
                {
                    req.Headers.Should()
                        .Contain(
                            h =>
                                string.Equals(h.Key, "Authorization", StringComparison.Ordinal)
                                && h.Value.Any()
                        );
                    Token
                        .FromRaw(req.Headers.GetAsString("Authorization").Replace("Bearer ", ""))
                        .Should()
                        .NotBeNull();
                    Token
                        .FromRaw(
                            req.Headers.GetAsString("Authorization")
                                .Replace("Bearer ", "", StringComparison.Ordinal) + ".extra"
                        )
                        .Should()
                        .BeNull();
                }
            );

    [Fact(DisplayName = "Repeated GET requests can be made to a real server")]
    public static async Task Case18() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                var req = WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/hello-world")
                    .UsingGet();
                server
                    .Given(req)
                    .InScenario(nameof(Case18))
                    .WillSetStateTo(1)
                    .RespondWith(
                        WireMock
                            .ResponseBuilders.Response.Create()
                            .WithStatusCode(Resp.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case18))
                    .WhenStateIs(1)
                    .WillSetStateTo(2)
                    .RespondWith(
                        WireMock
                            .ResponseBuilders.Response.Create()
                            .WithStatusCode(Resp.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case18))
                    .WhenStateIs(2)
                    .WillSetStateTo(3)
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return Req.Get.To($"{server.Urls[0]}/hello-world");
            })
            .ActAndCallUntil(
                Schedule.Spaced(TimeSpan.FromMilliseconds(10)) & Schedule.Recurs(4),
                resp => resp.StatusCode == Resp.OK
            )
            .Assert(ResponseCodeIsOk);

    [Fact(DisplayName = "Repeated GET requests can be made to a real server with extra data")]
    public static async Task Case19() =>
        await WireMockServer
            .Start()
            .ArrangeData()
            .And(server =>
            {
                var req = WireMock
                    .RequestBuilders.Request.Create()
                    .WithPath("/hello-world")
                    .UsingGet();
                server
                    .Given(req)
                    .InScenario(nameof(Case19))
                    .WillSetStateTo(1)
                    .RespondWith(
                        WireMock
                            .ResponseBuilders.Response.Create()
                            .WithStatusCode(Resp.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case19))
                    .WhenStateIs(1)
                    .WillSetStateTo(2)
                    .RespondWith(
                        WireMock
                            .ResponseBuilders.Response.Create()
                            .WithStatusCode(Resp.InternalServerError)
                    );
                server
                    .Given(req)
                    .InScenario(nameof(Case19))
                    .WhenStateIs(2)
                    .WillSetStateTo(3)
                    .RespondWith(WireMock.ResponseBuilders.Response.Create().WithSuccess());
                return server.Urls[0];
            })
            .ActAndCallUntil(
                url => Req.Get.To($"{url}/hello-world"),
                Schedule.Spaced(TimeSpan.FromMilliseconds(10)) & Schedule.Recurs(4),
                resp => resp.StatusCode == Resp.OK
            )
            .Assert(resp => resp.StatusCode == Resp.OK);

    [Fact(DisplayName = "Repeated GET requests can fail after the schedule completes")]
    public static Task Case20() =>
        Assert.ThrowsAsync<InvalidOperationException>(
            async () =>
                await WireMockServer
                    .Start()
                    .ArrangeData()
                    .And(server =>
                    {
                        var req = WireMock
                            .RequestBuilders.Request.Create()
                            .WithPath("/hello-world")
                            .UsingGet();
                        var resp = WireMock
                            .ResponseBuilders.Response.Create()
                            .WithStatusCode(Resp.InternalServerError);
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
                        return Req.Get.To($"{server.Urls[0]}/hello-world");
                    })
                    .ActAndCallUntil(
                        Schedule.Spaced(TimeSpan.FromMilliseconds(10)) & Schedule.Once,
                        resp => resp.StatusCode == Resp.OK
                    )
                    .Assert(resp => resp.StatusCode == Resp.OK)
        );
}
