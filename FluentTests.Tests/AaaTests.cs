using System.Diagnostics.CodeAnalysis;

namespace FluentTests.Tests;
using static Dsl.Aaa;

public class AaaTests
{
    [Fact(DisplayName = "Async methods operate correctly")]
    public async Task Case1() =>
        await Arrange(() => Task.FromResult(1))
            .And(x => Task.FromResult(x.ToString()))
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
            .And(x => x.ToString())
            .Act(x => x.Length)
            .And((_, r) => r + 1)
            .Assert((_, r) => Assert.Equal(2, r));

    [Fact(DisplayName = "Mixing sync and async methods operate correctly")]
    public async Task Case3() =>
        await "Some description"
            .Arrange(() => 2)
            .Act(x => Task.FromResult(x.ToString()))
            .Assert(r => Assert.Equal("2", r));

    [Fact(DisplayName = "Mixing sync and async methods operate correctly")]
    public async Task Case4() =>
        await "Some other description"
            .Arrange(() => Task.FromResult(2))
            .Act(x => x.ToString())
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

    private static Task<int> SomeAsyncFunction(int i) => throw new Exception("Some failure");

    [Fact(DisplayName = "Failure assertions work on async functions")]
    public async Task Case6() =>
        await Arrange(() => 1)
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
    public async Task Case8() =>
        await Assert.ThrowsAsync<NoFailureException>(
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

    private static int SomeFunction(int i) => throw new Exception("Some failure");

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
}
