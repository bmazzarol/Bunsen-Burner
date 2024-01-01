using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using static BunsenBurner.NUnit.TheoryExtensions;

namespace BunsenBurner.NUnit.Tests;

using TestScenario = Scenario<Syntax.Aaa>.Asserted<int, string>;

[ExcludeFromCodeCoverage]
public static class TheoryTests
{
    [Test(Description = "Arranged Scenarios can be collected into theory data")]
    public static async Task Case1() =>
        await TheoryData(1.ArrangeData(), 2.ArrangeData())
            .ArrangeData()
            .Act(d => d)
            .Assert(r => r.Take(3).Count() == 2);

    [Test(Description = "Acted Scenarios can be collected into theory data")]
    public static async Task Case2() =>
        await TheoryData(
                1.ArrangeData().Act(x => x.ToString(CultureInfo.InvariantCulture)),
                2.ArrangeData().Act(x => x.ToString(CultureInfo.InvariantCulture))
            )
            .ArrangeData()
            .Act(d => d)
            .Assert(r => r.Take(3).Count() == 2);

    [Test(Description = "Asserted Scenarios can be collected into theory data")]
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

    [Test(Description = "Example running scenarios from theory data")]
    [TestCaseSource(nameof(TestCases))]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions")]
    public static async Task Case4(TestScenario scenario) => await scenario;
}
