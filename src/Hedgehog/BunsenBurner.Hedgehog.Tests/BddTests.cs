using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Hedgehog.Tests;

[SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
public static class BddTests
{
    [Fact(DisplayName = "Property test can be run")]
    public static async Task Case1() =>
        await Gen.Alpha
            .String(Range.FromValue(25))
            .GivenGenerator()
            .ThenPropertyHolds(s => s != string.Empty && s.Length is > 0 and < 26);

    [Fact(DisplayName = "Property test can be run using Assert")]
    public static async Task Case2() =>
        await Gen.Alpha
            .String(Range.FromValue(25))
            .GivenGenerator()
            .ThenPropertyHolds(s =>
            {
                Assert.NotEmpty(s);
                Assert.Equal(25, s.Length);
            });

    [Fact(DisplayName = "Property test can be run with description")]
    public static async Task Case3() =>
        await "Some description"
            .GivenGenerator(Gen.Alpha.String(Range.FromValue(25)))
            .ThenPropertyHolds(s => s != string.Empty && s.Length is > 0 and < 26);

    [Fact(DisplayName = "Property test can be run using Assert with description")]
    public static async Task Case4() =>
        await "Some description"
            .GivenGenerator(Gen.Alpha.String(Range.FromValue(25)))
            .ThenPropertyHolds(s =>
            {
                Assert.NotEmpty(s);
                Assert.Equal(25, s.Length);
            });
}
