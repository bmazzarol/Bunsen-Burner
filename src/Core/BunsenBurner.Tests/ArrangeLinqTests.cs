using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Tests;

[SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
public static class ArrangeLinqTests
{
    [Fact(DisplayName = "Arranged Aaa tests can be selected")]
    public static async Task Case1() =>
        await AaaSyntax
            .Arrange(1)
            .Select(x => x.ToString(InvariantCulture))
            .Act(s => s)
            .Assert(r => r == "1");

    [Fact(DisplayName = "Arranged Bdd tests can be selected")]
    public static async Task Case2() =>
        await BddSyntax
            .Given(1)
            .Select(x => x.ToString(InvariantCulture))
            .When(s => s)
            .Then(r => r == "1");

    [Fact(DisplayName = "Arranged Aaa tests can be combined with bind")]
    public static async Task Case3() =>
        await AaaSyntax
            .Arrange(1)
            .SelectMany(a => AaaSyntax.Arrange(DateTime.Now).Select(b => (a, b)))
            .Act(t => t)
            .Assert(r => r.a == 1 && r.b.Ticks > r.a);

    [Fact(DisplayName = "Arranged Aaa tests can be combined with bind")]
    public static async Task Case4() =>
        await (
            from a in AaaSyntax.Arrange(1)
            from b in AaaSyntax.Arrange(DateTime.Now)
            from c in AaaSyntax.Arrange(() => "some string")
            select (a, b, c)
        )
            .Act(t => t)
            .Assert(r => r.a == 1 && r.b.Ticks > r.a && r.c == "some string");

    [Fact(DisplayName = "Tests can be combined with Sequence")]
    public static async Task Case5() =>
        await Enumerable
            .Range(1, 10)
            .Select(x => x.ArrangeData())
            .Combine()
            .Act(x => x.Count())
            .Assert(x => x == 10);
}
