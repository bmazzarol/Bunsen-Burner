using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Tests;

using static AaaSyntax;
using Test = TestBuilder<Syntax.Aaa>.Asserted<int, string>;

public static class TheoryTests
{
    public static readonly TheoryData<Test> TestCases = Enumerable
        .Range(1, 10)
        .Select(i =>
            $"{i} can be converted to a string"
                .Arrange(() => i)
                .Act(x => x.ToString(InvariantCulture))
                .Assert(r => Assert.Equal($"{i}", r))
        )
        .Aggregate(
            new TheoryData<Test>(),
            (td, s) =>
            {
                td.Add(s);
                return td;
            }
        );

    [Theory(DisplayName = "Tests work well in theories")]
    [MemberData(nameof(TestCases))]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
    public static async Task Case1(Test scenario) => await scenario;
}
