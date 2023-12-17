using FluentAssertions;
using static BunsenBurner.Http.HttpRequestMatchers;

namespace BunsenBurner.Http.Tests;

public static class HttpMessageMatchersTests
{
    [Fact(DisplayName = "HasMethod matches for the correct HTTP method")]
    public static void Case1() =>
        HasMethod(HttpMethod.Delete).IsMatch(Req.Delete.To("test")).Should().BeTrue();

    [Fact(DisplayName = "HasMethod does not match for the incorrect HTTP method")]
    public static void Case2() =>
        HasMethod(HttpMethod.Delete).IsMatch(Req.Get.To("test")).Should().BeFalse();

    [Fact(DisplayName = "And can be used to combine matchers")]
    public static void Case3() =>
        (HasMethod(HttpMethod.Delete) & HasHeader("test", "*"))
            .IsMatch(Req.Delete.To("test").WithHeader("test", "1"))
            .Should()
            .BeTrue();

    [Fact(DisplayName = "And can be used to combine matchers, must match all conditions")]
    public static void Case4() =>
        (HasMethod(HttpMethod.Delete) & HasHeader("test", "*"))
            .IsMatch(Req.Delete.To("test"))
            .Should()
            .BeFalse();

    [Fact(DisplayName = "Or can be used to combine matchers")]
    public static void Case5() =>
        (HasMethod(HttpMethod.Delete) | HasHeader("test", "*"))
            .IsMatch(Req.Delete.To("test"))
            .Should()
            .BeTrue();

    [Fact(DisplayName = "Or can be used to combine matchers, match only second condition")]
    public static void Case6() =>
        (HasMethod(HttpMethod.Delete) | HasHeader("test", "a?"))
            .IsMatch(Req.Get.To("test1").WithHeader("test", "a2"))
            .Should()
            .BeTrue();

    [Fact(DisplayName = "Or can be used to combine matchers, no matches")]
    public static void Case7() =>
        (HasMethod(HttpMethod.Delete) | HasHeader("test", "a?"))
            .IsMatch(Req.Get.To("test1").WithHeader("test", "b2"))
            .Should()
            .BeFalse();

    [Fact(DisplayName = "Or can be used to combine matchers, no matches again")]
    public static void Case8() =>
        (HasMethod(HttpMethod.Delete) | HasHeader("test", "a?"))
            .IsMatch(Req.Get.To("test1").WithHeader("test2", "a2"))
            .Should()
            .BeFalse();

    [Fact(DisplayName = "HasBearer can match on a provided token")]
    public static void Case9() =>
        HasBearerToken("abcd")
            .IsMatch(Req.Get.To("some-endpoint").WithBearerToken("abcd"))
            .Should()
            .BeTrue();

    [Fact(DisplayName = "HasRequestUri can match on a provided request uri")]
    public static void Case10() =>
        HasRequestUri("some-*-path").IsMatch(Req.Get.To("some-abcd-path")).Should().BeTrue();

    [Fact(
        DisplayName = "HasRequestUri can match on a provided request uri including host and port"
    )]
    public static void Case11() =>
        HasRequestUri("https://some-*:123?/some-*-path")
            .IsMatch(Req.Get.To("https://some-host:1236/some-abcd-path"))
            .Should()
            .BeTrue();

    [Fact(DisplayName = "HasContent can match on a provided request content")]
    public static void Case12() =>
        HasContent("some ?ool ?ontent *")
            .IsMatch(Req.Post.To("path").WithTextContent("some kool Content that is posted"))
            .Should()
            .BeTrue();

    public sealed class TestContent
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    [Fact(DisplayName = "HasJsonContent can match on a provided request content")]
    public static void Case13() =>
        HasJsonContent((TestContent content) => content.Age > 50 && content.Name.Contains("en"))
            .IsMatch(
                Req.Post.To("path").WithJsonContent(new TestContent { Name = "Jen", Age = 56 })
            )
            .Should()
            .BeTrue();

    [Fact(DisplayName = "HasJsonContent can not match on a provided request content")]
    public static void Case14() =>
        HasJsonContent((TestContent content) => content.Age > 50 && content.Name.Contains("en"))
            .IsMatch(
                Req.Post.To("path").WithJsonContent(new TestContent { Name = "Max", Age = 56 })
            )
            .Should()
            .BeFalse();

    [Fact(
        DisplayName = "HasJsonContent can not match on a provided request content that is not valid json"
    )]
    public static void Case15() =>
        HasJsonContent((TestContent content) => content.Age > 50 && content.Name.Contains("en"))
            .IsMatch(Req.Post.To("path").WithTextContent("som cool stuff"))
            .Should()
            .BeFalse();

    [Fact(
        DisplayName = "HasJsonContent can not match on a provided request content that is different valid json"
    )]
    public static void Case16() =>
        HasJsonContent((TestContent content) => content.Age > 50 && content.Name.Contains("en"))
            .IsMatch(Req.Post.To("path").WithJsonContent(new { Fish = 12345 }))
            .Should()
            .BeFalse();

    [Fact(DisplayName = "HasXmlContent can match on a provided request content")]
    public static void Case17() =>
        HasXmlContent((TestContent content) => content.Age > 50 && content.Name.Contains("en"))
            .IsMatch(Req.Post.To("path").WithXmlContent(new TestContent { Name = "Jen", Age = 56 }))
            .Should()
            .BeTrue();

    [Fact(DisplayName = "HasXmlContent can not match on a provided request content")]
    public static void Case18() =>
        HasXmlContent((TestContent content) => content.Age > 50 && content.Name.Contains("en"))
            .IsMatch(Req.Post.To("path").WithXmlContent(new TestContent { Name = "Max", Age = 56 }))
            .Should()
            .BeFalse();

    [Fact(
        DisplayName = "HasXmlContent can not match on a provided request content that is not valid json"
    )]
    public static void Case19() =>
        HasXmlContent((TestContent content) => content.Age > 50 && content.Name.Contains("en"))
            .IsMatch(Req.Post.To("path").WithTextContent("som cool stuff"))
            .Should()
            .BeFalse();

    public sealed class FishModel
    {
        public int Fish { get; set; }
    }

    [Fact(
        DisplayName = "HasXmlContent can not match on a provided request content that is different valid xml"
    )]
    public static void Case20() =>
        HasXmlContent((TestContent content) => content.Age > 50 && content.Name.Contains("en"))
            .IsMatch(Req.Post.To("path").WithXmlContent(new FishModel { Fish = 12345 }))
            .Should()
            .BeFalse();
}
