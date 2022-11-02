namespace BunsenBurner.Http.Tests;

using static BunsenBurner.Aaa;

public static class RequestTests
{
    [Fact(DisplayName = "Request with no content has a length of 0")]
    public static async Task Case1() =>
        await Arrange((Request)Request.GET("/test"))
            .Act(req => req.ContentLength())
            .Assert(length => !length.HasValue);

    [Fact(DisplayName = "Request with no content has no content type")]
    public static async Task Case2() =>
        await Arrange((Request)Request.GET("/test"))
            .Act(req => req.ContentType())
            .Assert(type => type == null);
}
