using System.Globalization;

namespace BunsenBurner.Tests;

public static class ActedLinqTests
{
    [Fact(DisplayName = "Acted Aaa scenarios can be selected")]
    public static async Task Case1() =>
        await Aaa.Arrange(1).Act(_ => _).Select(x => x.ToString()).Assert(r => r == "1");

    [Fact(DisplayName = "Acted Bdd scenarios can be selected")]
    public static async Task Case2() =>
        await Bdd.Given(1).When(_ => _).Select(x => x.ToString()).Then(r => r == "1");

    [Fact(DisplayName = "Acted Aaa scenarios can be combined with bind")]
    public static async Task Case3() =>
        await Aaa.Arrange(1)
            .Act(_ => _)
            .SelectMany(a => Aaa.Arrange(2).Act(_ => DateTime.Now).Select(b => (a, b)))
            .Assert(r => r.a == 1 && r.b.Ticks > r.a);

    [Fact(DisplayName = "Acted Aaa scenarios can be combined with bind")]
    public static async Task Case4() =>
        await (
            from a in Aaa.Arrange(1).Act(x => x.ToString())
            from b in Aaa.Arrange(2).Act(_ => DateTime.Now.ToString(CultureInfo.InvariantCulture))
            from c in Aaa.Arrange(3).Act(_ => "some string")
            select a + b + c
        ).Assert(r => r.StartsWith("1") && r.EndsWith("some string"));
}
