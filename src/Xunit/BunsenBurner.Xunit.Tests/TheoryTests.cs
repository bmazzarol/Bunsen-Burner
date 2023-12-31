using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using static BunsenBurner.Xunit.TheoryExtensions;

namespace BunsenBurner.Xunit.Tests;

using TestScenario = Scenario<Syntax.Aaa>.Asserted<int, string>;

[ExcludeFromCodeCoverage]
public static class TheoryTests
{
    [Fact(DisplayName = "Arranged Scenarios can be collected into theory data")]
    public static async Task Case1() =>
        await TheoryData(1.ArrangeData(), 2.ArrangeData())
            .ArrangeData()
            .Act(d => d)
            .Assert(r => r.Take(3).Count() == 2);

    [Fact(DisplayName = "Acted Scenarios can be collected into theory data")]
    public static async Task Case2() =>
        await TheoryData(
                1.ArrangeData().Act(x => x.ToString(CultureInfo.InvariantCulture)),
                2.ArrangeData().Act(x => x.ToString(CultureInfo.InvariantCulture))
            )
            .ArrangeData()
            .Act(d => d)
            .Assert(r => r.Take(3).Count() == 2);

    [Fact(DisplayName = "Asserted Scenarios can be collected into theory data")]
    public static async Task Case3() =>
        await TheoryData(
                1
                    .ArrangeData()
                    .Act(x => x.ToString(CultureInfo.InvariantCulture))
                    .Assert(r => r == "1"),
                2
                    .ArrangeData()
                    .Act(x => x.ToString(CultureInfo.InvariantCulture))
                    .Assert(r => r == "2")
            )
            .ArrangeData()
            .Act(d => d)
            .Assert(r => r.Take(3).Count() == 2);

    public static readonly TheoryData<TestScenario> TestCases = TheoryData(
        "Some test"
            .Arrange(1)
            .Act(x => x.ToString(CultureInfo.InvariantCulture))
            .Assert(r => r == "1"),
        "Some other test"
            .Arrange(2)
            .Act(x => x.ToString(CultureInfo.InvariantCulture))
            .Assert(r => r == "2")
    );

    [Theory(DisplayName = "Example running scenarios from theory data")]
    [MemberData(nameof(TestCases))]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
    public static async Task Case4(TestScenario scenario) => await scenario;
}
