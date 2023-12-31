namespace BunsenBurner.Http.Tests;

public static class DummyHttpFactoryTests
{
    [Fact(
        DisplayName = "A http factory can be mocked for a given request, returning a configured response"
    )]
    public static async Task Case1() =>
        await DummyHttpFactory
            .New()
            .ArrangeData()
            .And(x =>
            {
                x.Store.Setup(
                    nameof(Case1),
                    message => message.RequestUri?.AbsolutePath.Contains("some/test") ?? false,
                    req => Resp.Accepted.Result(request: req).WithTextContent("ok")
                );
                return x;
            })
            .Act(
                x =>
                    x.CreateClient(nameof(Case1))
                        .SendAsync(Req.Get.To("http://localhost/some/test/path"))
            )
            .Assert(resp => resp.IsSuccessStatusCode)
            .And(
                (ctx, _) =>
                    ctx.Store.Any(
                        m =>
                            m.ClientName == nameof(Case1)
                            && m.Request.Method == Req.Get
                            && m.Response.StatusCode == Resp.Accepted
                    )
            );

    [Fact(DisplayName = "A http factory can be called for an unknown named client")]
    public static async Task Case2() =>
        await DummyHttpFactory
            .New()
            .ArrangeData()
            .Act(x => x.CreateClient(nameof(Case1)))
            .Assert(Assert.NotNull);

    [Fact(DisplayName = "A http factory can be called for a client with no matching setups")]
    public static async Task Case3() =>
        await DummyHttpFactory
            .New()
            .ArrangeData()
            .And(
                x =>
                    x.Store.Setup(
                        nameof(Case1),
                        message => message.RequestUri?.AbsolutePath.Contains("wont-match") ?? false,
                        req => Resp.Accepted.Result(request: req).WithTextContent("ok")
                    )
            )
            .Act(
                x =>
                    x.CreateClient(nameof(Case1))
                        .SendAsync(Req.Get.To("http://localhost/something/else"))
            )
            .AssertFailsWith(
                e =>
                    e.Message
                    == "No setup matches/generates a response for request: Request: -X GET http://localhost/something/else"
            );

    [Fact(DisplayName = "A http factory can be called for a client with a single response setup")]
    public static async Task Case4() =>
        await DummyHttpFactory
            .New()
            .ArrangeData()
            .And(
                x =>
                    x.Store.Setup(
                        nameof(Case4),
                        message => message.RequestUri?.AbsolutePath.Contains("match/") ?? false,
                        Resp.OK.Result().WithTextContent("matched")
                    )
            )
            .Act(async x =>
            {
                var resp1 = await x.CreateClient(nameof(Case4))
                    .SendAsync(Req.Get.To("http://localhost/match/me"));
                var resp2 = await x.CreateClient(nameof(Case4))
                    .SendAsync(Req.Get.To("http://localhost/match/me-as-well"));
                return (resp1, resp2);
            })
            .Assert(x => Assert.Same(x.resp1, x.resp2));

    [Fact(
        DisplayName = "A http factory can be called for a client with a fixed number of responses setup"
    )]
    public static async Task Case5() =>
        await DummyHttpFactory
            .New()
            .ArrangeData()
            .And(
                x =>
                    x.Store
                        .Setup(
                            nameof(Case4),
                            message => message.RequestUri?.AbsolutePath.Contains("match/") ?? false,
                            Resp.Unauthorized.Result().WithTextContent("no-way"),
                            Resp.OK.Result().WithTextContent("matched")
                        )
                        .Setup(
                            nameof(Case4),
                            message => message.RequestUri?.AbsolutePath.Contains("match/") ?? false,
                            Resp.Accepted.Result().WithTextContent("matched")
                        )
            )
            .Act(async x =>
            {
                var resp1 = await x.CreateClient(nameof(Case4))
                    .SendAsync(Req.Get.To("http://localhost/match/me"));
                var resp2 = await x.CreateClient(nameof(Case4))
                    .SendAsync(Req.Get.To("http://localhost/match/me-as-well"));
                var resp3 = await x.CreateClient(nameof(Case4))
                    .SendAsync(Req.Get.To("http://localhost/match/me-default"));
                return (resp1, resp2, resp3);
            })
            .Assert(x => Assert.NotSame(x.resp1, x.resp2))
            .And(
                x =>
                    !x.resp1.IsSuccessStatusCode
                    && x.resp2.IsSuccessStatusCode
                    && x.resp3.StatusCode == Resp.Accepted
            );
}
