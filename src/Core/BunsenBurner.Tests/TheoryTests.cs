using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Tests;

using static ArrangeActAssert;
using Test = TestBuilder<ArrangeActAssertSyntax>.Asserted<int, string>;

public static class TheoryTests
{
    public static readonly TheoryData<Test> TestCases = Enumerable
        .Range(1, 10)
        .Select(i =>
            Arrange(() => i)
                .Act(x => x.ToString(InvariantCulture))
                .Assert(r => Assert.Equal($"{i}", r)) with
            {
                Name = $"{i} can be converted to a string"
            }
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
