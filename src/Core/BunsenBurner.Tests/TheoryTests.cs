using System.Diagnostics.CodeAnalysis;

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
                    .Act(x => x.ToString(InvariantCulture))
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
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
    public static async Task Case1(Scenario scenario) => await scenario;
}
