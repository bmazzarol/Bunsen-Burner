using Gen = Hedgehog.Linq.Gen;

namespace BunsenBurner.Hedgehog.Tests;

public static class AaaTests
{
    [Fact(DisplayName = "Property test can be run")]
    public static async Task Case1() =>
        await Gen.Alpha
            .String(Range.FromValue(25))
            .ArrangeGenerator()
            .AssertPropertyHolds(s => s != string.Empty && s.Length is > 0 and < 26);

    [Fact(DisplayName = "Property test can be run using Assert")]
    public static async Task Case2() =>
        await Gen.Alpha
            .String(Range.FromValue(25))
            .ArrangeGenerator()
            .AssertPropertyHolds(s =>
            {
                Assert.NotEmpty(s);
                Assert.Equal(25, s.Length);
            });

    [Fact(DisplayName = "Property test can be run with description")]
    public static async Task Case3() =>
        await "Some description"
            .ArrangeGenerator(Gen.Alpha.String(Range.FromValue(25)))
            .AssertPropertyHolds(s => s != string.Empty && s.Length is > 0 and < 26);

    [Fact(DisplayName = "Property test can be run using Assert with description")]
    public static async Task Case4() =>
        await "Some description"
            .ArrangeGenerator(Gen.Alpha.String(Range.FromValue(25)))
            .AssertPropertyHolds(s =>
            {
                Assert.NotEmpty(s);
                Assert.Equal(25, s.Length);
            });

    [Fact(DisplayName = "Generators can be combined")]
    public static async Task Case6() =>
        await (
            from name in Gen.Alpha.String(Range.FromValue(25))
            from age in Gen.Int32(Range.LinearFromInt32(1, 20, 99))
            select (name, age)
        )
            .ArrangeGenerator()
            .AssertPropertyHolds(t => t.name != null && t.age > 0);
}
