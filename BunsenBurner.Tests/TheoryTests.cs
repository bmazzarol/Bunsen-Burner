namespace BunsenBurner.Tests;

#region TheoryExample

using static ArrangeActAssert;
using Test = TestBuilder<ArrangeActAssertSyntax>.Asserted<int, string>;

public static class TheoryTests
{
    public static readonly TheoryData<Test> TestCases = Enumerable
        .Range(1, 10)
        .Select(i =>
            (i.Arrange() with { Name = $"{i} can be converted to a string" })
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
    public static Task Case1(Test scenario) => scenario;
}

#endregion
