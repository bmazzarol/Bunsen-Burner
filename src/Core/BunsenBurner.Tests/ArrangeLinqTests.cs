using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Tests;

[SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
public static class ArrangeLinqTests
{
    [Fact(DisplayName = "Arranged Aaa scenarios can be selected")]
    public static async Task Case1() =>
        await Aaa.Arrange(1)
            .Select(x => x.ToString(InvariantCulture))
            .Act(s => s)
            .Assert(r => r == "1");

    [Fact(DisplayName = "Arranged Bdd scenarios can be selected")]
    public static async Task Case2() =>
        await Bdd.Given(1)
            .Select(x => x.ToString(InvariantCulture))
            .When(s => s)
            .Then(r => r == "1");

    [Fact(DisplayName = "Arranged Aaa scenarios can be combined with bind")]
    public static async Task Case3() =>
        await Aaa.Arrange(1)
            .SelectMany(a => Aaa.Arrange(DateTime.Now).Select(b => (a, b)))
            .Act(t => t)
            .Assert(r => r.a == 1 && r.b.Ticks > r.a);

    [Fact(DisplayName = "Arranged Aaa scenarios can be combined with bind")]
    public static async Task Case4() =>
        await (
            from a in Aaa.Arrange(1)
            from b in Aaa.Arrange(DateTime.Now)
            from c in Aaa.Arrange(() => "some string")
            select (a, b, c)
        )
            .Act(t => t)
            .Assert(r => r.a == 1 && r.b.Ticks > r.a && r.c == "some string");

    [Fact(DisplayName = "Scenarios can be combined with Sequence")]
    public static async Task Case5() =>
        await Enumerable
            .Range(1, 10)
            .Select(x => x.ArrangeData())
            .Sequence()
            .Act(x => x.Count())
            .Assert(x => x == 10);
}
