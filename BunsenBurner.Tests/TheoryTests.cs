namespace BunsenBurner.Tests;
using static Aaa;
using Scenario = Scenario<Syntax.Aaa>.Asserted<int, string>;

public static class TheoryTests
{
    public static readonly TheoryData<Scenario> TestCases = Enumerable
        .Range(1, 10)
        .Select(
            i =>
                $"{i} can be converted to a string"
                    .Arrange(() => i)
                    .Act(x => x.ToString())
                    .Assert(r => Assert.Equal($"{i}", r))
        )
        .Aggregate(
            new TheoryData<Scenario>(),
            (td, s) =>
            {
                td.Add(s);
                return td;
            }
        );

    [Theory(DisplayName = "Scenarios work well in theories")]
    [MemberData(nameof(TestCases))]
    public static async Task Case1(Scenario scenario) => await scenario;
}
