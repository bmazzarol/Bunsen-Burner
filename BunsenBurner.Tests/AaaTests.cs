using System.Diagnostics.CodeAnalysis;
using BunsenBurner.Exceptions;

namespace BunsenBurner.Tests;

using static ArrangeActAssert;

public class AaaSyntaxTests
{
    [Fact(DisplayName = "Async methods operate correctly")]
    public Task Case1() =>
        Arrange(() => Task.FromResult(1))
            .And(x => Task.FromResult(x.ToString(InvariantCulture)))
            .And(async x =>
            {
                await Task.Yield();
                Assert.Equal("1", x);
            })
            .Act(x => Task.FromResult(x.Length))
            .And(
                async (_, x) =>
                {
                    await Task.Yield();
                    Assert.Equal(1, x);
                }
            )
            .And((_, r) => Task.FromResult(r + 1))
            .Assert(
                (_, r) =>
                {
                    Assert.Equal(2, r);
                    return Task.CompletedTask;
                }
            );

    [Fact(DisplayName = "Sync methods operate correctly")]
    public Task Case2() =>
        Arrange(() => 1)
            .And(x => x.ToString(InvariantCulture))
            .And(x => Assert.Equal("1", x))
            .Act(x => x.Length)
            .And((_, x) => Assert.Equal(1, x))
            .And((_, r) => r + 1)
            .Assert((_, r) => Assert.Equal(2, r));

    [Fact(DisplayName = "Additional And assertions work")]
    public Task Case5() =>
        Arrange(() => (a: 1, b: "c"))
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

    [Fact(DisplayName = "Failure assertions work on async functions with initial data")]
    public Task Case7() =>
        Arrange(() => 1)
            .Act(SomeAsyncFunction)
            .Throw()
            .Assert(e =>
            {
                Assert.Equal("Some failure", e.Message);
                return Task.CompletedTask;
            });

    [Fact(DisplayName = "Failure assertions throw on successful async functions")]
    public async Task Case8()
    {
        var exception = await Assert.ThrowsAsync<NoFailureException>(
            () =>
                Arrange(() => 1)
                    .Act(Task.FromResult)
                    .Throw()
                    .Assert(
                        [ExcludeFromCodeCoverage]
                        (data, e) =>
                        {
                            Assert.Equal(1, data);
                            Assert.Equal("Some failure", e.Message);
                            return Task.CompletedTask;
                        }
                    )
        );

        Assert.Equal("Test did not fail as expected", exception.Message);
    }

    [Fact(DisplayName = "Failure assertions throw on successful async functions with named tests")]
    public async Task Case8b()
    {
        var exception = await Assert.ThrowsAsync<NoFailureException>(
            () =>
                (1.Arrange() with { Name = "Some custom name" })
                    .Act(Task.FromResult)
                    .Throw()
                    .Assert(
                        [ExcludeFromCodeCoverage]
                        (data, e) =>
                        {
                            Assert.Equal(1, data);
                            Assert.Equal("Some failure", e.Message);
                            return Task.CompletedTask;
                        }
                    )
        );

        Assert.Equal("Test 'Some custom name' did not fail as expected", exception.Message);
    }

    private static int SomeFunction(int i) => throw new InvalidOperationException("Some failure");

    [Fact(DisplayName = "Failure assertions work on sync functions")]
    public Task Case9() =>
        Arrange(() => 1)
            .Act(SomeFunction)
            .Throw()
            .Assert(e => Assert.Equal("Some failure", e.Message));

    [Fact(DisplayName = "Failure assertions work on sync functions and initial data")]
    public Task Case10() =>
        Arrange(() => 1)
            .Act(SomeFunction)
            .Throw()
            .Assert(
                (data, e) =>
                {
                    Assert.Equal(1, data);
                    Assert.Equal("Some failure", e.Message);
                }
            );

    [Fact(DisplayName = "Expression based assertions work")]
    public Task Case11() =>
        Arrange(() => 1).Act(x => x + 2).Assert(x => x > 0 && x < 4).And(x => x % 1 == 0);

    [Fact(DisplayName = "Expression based assertions that are wrong fail")]
    public async Task Case12()
    {
        var exception = await Assert.ThrowsAsync<ExpressionAssertionFailureException>(
            async () => await Arrange(() => 1).Act(x => x + 2).Assert(x => x > 4 && x < 6)
        );
        Assert.Equal("x => ((x > 4) AndAlso (x < 6)) is not true for input '3'", exception.Message);
        Assert.NotNull(exception.Expression);
        Assert.NotNull(exception.Result);
        Assert.Null(exception.TestData);
    }

    [Fact(DisplayName = "Expression based assertions with data work")]
    public Task Case13() =>
        1
            .Arrange()
            .Act(x => x + 2)
            .Assert((r, x) => r == 1 && x > 0 && x < 4)
            .And((r, x) => x % r == 0);

    [Fact(DisplayName = "Expression based assertions with data that are wrong fail")]
    public async Task Case14()
    {
        var exception = await Assert.ThrowsAsync<ExpressionAssertionFailureException>(
            () => 1.Arrange().Act(x => x + 2).Assert((r, x) => r == 2 && x > 4 && x < 6)
        );
        Assert.Equal(
            "(r, x) => (((r == 2) AndAlso (x > 4)) AndAlso (x < 6)) is not true for inputs '1' and '3'",
            exception.Message
        );
    }

    [Fact(DisplayName = "Expression based assertions on failures work")]
    public Task Case15() =>
        // ReSharper disable once IntDivisionByZero
        1
            .Arrange()
            .Act(x => x / 0)
            .Throw<DivideByZeroException>()
            .Assert(e => e.Message == "Attempted to divide by zero.");

    [Fact(DisplayName = "Expression based assertions on failures with data work")]
    public Task Case16() =>
        // ReSharper disable once IntDivisionByZero
        1
            .Arrange()
            .Act(x => x / 0)
            .Throw()
            .Assert((r, e) => r == 1 && e.Message == "Attempted to divide by zero.");

    [Fact(DisplayName = "Expression based assertions on typed failures work")]
    public Task Case17() =>
        // ReSharper disable once IntDivisionByZero
        1
            .Arrange()
            .Act(x => x / 0)
            .Throw<DivideByZeroException>()
            .Assert((r, e) => r == 1 && e.Message == "Attempted to divide by zero.");
}
