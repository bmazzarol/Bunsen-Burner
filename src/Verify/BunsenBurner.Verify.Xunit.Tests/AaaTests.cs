namespace BunsenBurner.Verify.Xunit.Tests;

using static BunsenBurner.Aaa;

[UsesVerify]
public static class AaaTests
{
    [Fact(DisplayName = "Assertion with scrubbing and projection works")]
    public static async Task Case1() =>
        await Arrange(
                () =>
                    new
                    {
                        A = 1,
                        B = "2",
                        C = DateTimeOffset.Now
                    }
            )
            .Act(_ => _)
            .AssertResultIsUnchanged(x => new { x.A, x.C }, scrubResults: true);

    [Fact(DisplayName = "Named scenario assertion with scrubbing and projection works")]
    public static async Task Case2() =>
        await "Some description"
            .Arrange(
                () =>
                    new
                    {
                        A = 1,
                        B = "2",
                        C = DateTimeOffset.Now
                    }
            )
            .Act(_ => _)
            .AssertResultIsUnchanged(x => new { x.A, x.C }, scrubResults: true);

    [Fact(DisplayName = "Assertion without scrubbing works")]
    public static async Task Case3() =>
        await Arrange(() => new { A = 1, B = "2" }).Act(_ => _).AssertResultIsUnchanged();

    [Fact(DisplayName = "Named scenario assertion without scrubbing works")]
    public static async Task Case4() =>
        await "Some description"
            .Arrange(() => new { A = 1, B = "2" })
            .Act(_ => _)
            .AssertResultIsUnchanged();
}
