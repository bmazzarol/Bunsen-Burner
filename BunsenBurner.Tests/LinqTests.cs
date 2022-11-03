namespace BunsenBurner.Tests;

public static class LinqTests
{
    [Fact(DisplayName = "Arranged Aaa scenarios can be selected")]
    public static async Task Case1() =>
        await Aaa.Arrange(1).Select(x => x.ToString()).Act(_ => _).Assert(r => r == "1");

    [Fact(DisplayName = "Arranged Aaa scenarios can be selected")]
    public static async Task Case2() =>
        await Bdd.Given(1).Select(x => x.ToString()).When(_ => _).Then(r => r == "1");

    [Fact(DisplayName = "Arranged Aaa scenarios can be combined with bind")]
    public static async Task Case3() =>
        await Aaa.Arrange(1)
            .SelectMany(a => Aaa.Arrange(DateTime.Now).Select(b => (a, b)))
            .Act(_ => _)
            .Assert(r => r.a == 1 && r.b.Ticks > r.a);

    [Fact(DisplayName = "Arranged Aaa scenarios can be combined with bind")]
    public static async Task Case4() =>
        await (
            from a in Aaa.Arrange(1)
            from b in Aaa.Arrange(DateTime.Now)
            from c in Aaa.Arrange(() => "some string")
            select (a, b, c)
        )
            .Act(_ => _)
            .Assert(r => r.a == 1 && r.b.Ticks > r.a && r.c == "some string");
}
