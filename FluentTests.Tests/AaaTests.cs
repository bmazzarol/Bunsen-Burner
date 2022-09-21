namespace FluentTests.Tests;
using static Dsl.Aaa;
using static Dsl.Shared;

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
}
