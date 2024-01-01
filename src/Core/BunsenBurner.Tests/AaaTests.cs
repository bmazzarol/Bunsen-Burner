using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Tests;

using static Aaa;

public class AaaTests
{
    [Fact(DisplayName = "Async methods operate correctly")]
    public async Task Case1() =>
        await Arrange(() => Task.FromResult(1))
            .And(x => Task.FromResult(x.ToString(InvariantCulture)))
            .Act(x => Task.FromResult(x.Length))
            .And((_, r) => Task.FromResult(r + 1))
            .Assert(
                (_, r) =>
                {
                    Assert.Equal(2, r);
                    return Task.CompletedTask;
                }
            );

    [Fact(DisplayName = "Sync methods operate correctly")]
    public async Task Case2() =>
        await Arrange(() => 1)
            .And(x => x.ToString(InvariantCulture))
            .Act(x => x.Length)
            .And((_, r) => r + 1)
            .Assert((_, r) => Assert.Equal(2, r));

    [Fact(DisplayName = "Mixing sync and async methods operate correctly")]
    public async Task Case3() =>
        await "Some description"
            .Arrange(2)
            .Act(x => Task.FromResult(x.ToString(InvariantCulture)))
            .Assert(r => Assert.Equal("2", r));

    [Fact(DisplayName = "Mixing sync and async methods operate correctly")]
    public async Task Case4() =>
        await "Some other description"
            .Arrange(() => Task.FromResult(2))
            .Act(x => x.ToString(InvariantCulture))
            .Assert(r =>
            {
                Assert.Equal("2", r);
                return Task.CompletedTask;
            });

    [Fact(DisplayName = "Additional And assertions work")]
    public async Task Case5() =>
        await Arrange(() => (a: 1, b: "c"))
            .Act(x => x.a + x.b)
            .Assert(
                (d, r) =>
                {
                    Assert.NotEqual(d.b, r);
                    return Task.CompletedTask;
                }
            )
            .And(
                (d, r) =>
                {
                    Assert.Equal(2, r.Length);
                    Assert.Equal(1, d.a);
                    return Task.CompletedTask;
                }
            )
            .And(r =>
            {
                Assert.NotEqual(1, r.Length);
                return Task.CompletedTask;
            })
            .And((d, r) => Assert.NotEqual(d.a, r.Length))
            .And(r => Assert.NotEqual(1, r.Length));

    private static Task<int> SomeAsyncFunction(int i) =>
        throw new InvalidOperationException("Some failure");

    [Fact(DisplayName = "Failure assertions work on async functions")]
    public async Task Case6() =>
        await "Some description"
            .Arrange(() => 1)
            .Act(SomeAsyncFunction)
            .AssertFailsWith(
                (_, e) =>
                {
                    Assert.Equal("Some failure", e.Message);
                    return Task.CompletedTask;
                }
            );

    [Fact(DisplayName = "Failure assertions work on async functions with initial data")]
    public async Task Case7() =>
        await Arrange(() => 1)
            .Act(SomeAsyncFunction)
            .AssertFailsWith(e =>
            {
                Assert.Equal("Some failure", e.Message);
                return Task.CompletedTask;
            });

    [Fact(DisplayName = "Failure assertions throw on successful async functions")]
    public Task Case8() =>
        Assert.ThrowsAsync<NoFailureException>(
            async () =>
                await Arrange(() => 1)
                    .Act(Task.FromResult)
                    .AssertFailsWith(
                        [ExcludeFromCodeCoverage]
                        (data, e) =>
                        {
                            Assert.Equal(1, data);
                            Assert.Equal("Some failure", e.Message);
                            return Task.CompletedTask;
                        }
                    )
        );

    private static int SomeFunction(int i) => throw new InvalidOperationException("Some failure");

    [Fact(DisplayName = "Failure assertions work on sync functions")]
    public async Task Case9() =>
        await Arrange(() => 1)
            .Act(SomeFunction)
            .AssertFailsWith(e => Assert.Equal("Some failure", e.Message));

    [Fact(DisplayName = "Failure assertions work on sync functions and initial data")]
    public async Task Case10() =>
        await Arrange(() => 1)
            .Act(SomeFunction)
            .AssertFailsWith(
                (data, e) =>
                {
                    Assert.Equal(1, data);
                    Assert.Equal("Some failure", e.Message);
                }
            );

    [Fact(DisplayName = "Expression based assertions work")]
    public async Task Case11() =>
        await Arrange(() => 1).Act(x => x + 2).Assert(x => x > 0 && x < 4).And(x => x % 1 == 0);

    [Fact(DisplayName = "Expression based assertions that are wrong fail")]
    public async Task Case12()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await Arrange(() => 1).Act(x => x + 2).Assert(x => x > 4 && x < 6)
        );
        Assert.Equal(
            "x => ((x > 4) AndAlso (x < 6)) is not true for the result 3",
            exception.Message
        );
    }

    [Fact(DisplayName = "Expression based assertions with data work")]
    public async Task Case13() =>
        await Arrange(1)
            .Act(x => x + 2)
            .Assert((r, x) => r == 1 && x > 0 && x < 4)
            .And((r, x) => x % r == 0);

    [Fact(DisplayName = "Expression based assertions with data that are wrong fail")]
    public async Task Case14()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () =>
                await 1.ArrangeData().Act(x => x + 2).Assert((r, x) => r == 2 && x > 4 && x < 6)
        );
        Assert.Equal(
            "(r, x) => (((r == 2) AndAlso (x > 4)) AndAlso (x < 6)) is not true for the result 3 and data 1",
            exception.Message
        );
    }

    [Fact(DisplayName = "Expression based assertions on failures work")]
    public async Task Case15() =>
        // ReSharper disable once IntDivisionByZero
        await 1
            .ArrangeData()
            .Act(x => x / 0)
            .AssertFailsWith(e => e.Message == "Attempted to divide by zero.");

    [Fact(DisplayName = "Expression based assertions on failures with data work")]
    public async Task Case16() =>
        // ReSharper disable once IntDivisionByZero
        await 1
            .ArrangeData()
            .Act(x => x / 0)
            .AssertFailsWith((r, e) => r == 1 && e.Message == "Attempted to divide by zero.");

    [Fact(DisplayName = "Expression based assertions on typed failures work")]
    public async Task Case17() =>
        // ReSharper disable once IntDivisionByZero
        await 1
            .ArrangeData()
            .Act(x => x / 0)
            .AssertFailsWith(
                (int r, DivideByZeroException e) =>
                    r == 1 && e.Message == "Attempted to divide by zero."
            );

    [Fact(DisplayName = "Scenario can be reset back to arranged from asserted")]
    public async Task Case18()
    {
        int ActFn(int x) => x + 1;
        void AssertFn(int x) => Assert.Equal(2, x);
        await 1
            .ArrangeData()
            .Act(ActFn)
            .Assert(AssertFn)
            .ResetToArranged()
            .Act(ActFn)
            .Assert(AssertFn);
    }

    [Fact(DisplayName = "Scenario can be reset back to arranged from acted")]
    public async Task Case19()
    {
        int ActFn(int x) => x + 1;
        void AssertFn(int x) => Assert.Equal(2, x);
        await 1.ArrangeData().Act(ActFn).ResetToArranged().Act(ActFn).Assert(AssertFn);
    }

    [Fact(DisplayName = "Scenario can be reset back to acted")]
    public async Task Case20()
    {
        void AssertFn(int x) => Assert.Equal(2, x);
        await 1.ArrangeData().Act(x => x + 1).Assert(AssertFn).ResetToActed().Assert(AssertFn);
    }

    [Fact(DisplayName = "Scenario can have act redefined")]
    public async Task Case21() =>
        await 1.ArrangeData().Act(x => x + 1).Assert(x => x == 3).ReplaceAct(x => x + 2);

    [Fact(DisplayName = "Scenario can have an async act redefined")]
    public async Task Case22() =>
        await 1
            .ArrangeData()
            .Act(x => Task.FromResult(x + 1))
            .Assert(x => x == 3)
            .ReplaceAct(x => Task.FromResult(x + 2));
}
