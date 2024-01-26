using System.Diagnostics.CodeAnalysis;
using BunsenBurner.Utility;

namespace BunsenBurner.Tests;

using static AaaSyntax;

[SuppressMessage("Major Code Smell", "S3881:\"IDisposable\" should be implemented correctly")]
internal sealed class TestDisposableType : IDisposable
{
    private int _state = 1;

    public int CurrentState() => _state;

    public void Dispose() => _state = 2;
}

public static class CacheTests
{
    [Fact(DisplayName = "Caches dispose all disposable values")]
    public static async Task Case1() =>
        await Arrange(() =>
            {
                var cache = Cache.New<TestDisposableType>();
                return (
                    cache,
                    valueA: cache.Get("a", _ => new TestDisposableType()),
                    valueB: cache.Get("b", _ => new TestDisposableType())
                );
            })
            .Act(x =>
            {
                var aResult = x.valueA.CurrentState();
                var bResult = x.valueB.CurrentState();
                x.cache.Dispose();
                return (aResult, bResult);
            })
            .Assert(
                (ctx, result) =>
                    result.aResult == 1
                    && result.bResult == 1
                    && ctx.valueA.CurrentState() == 2
                    && ctx.valueB.CurrentState() == 2
                    && ctx.cache.IsEmpty()
            );

    [Fact(DisplayName = "Cache is cleared and non-disposable values can be cached")]
    public static async Task Case2() =>
        await Arrange(() =>
            {
                var cache = Cache.New<Guid>();
                return (
                    cache,
                    valueA: cache.Get("a", _ => Guid.NewGuid()),
                    valueB: cache.Get("b", _ => Guid.NewGuid())
                );
            })
            .Act(x =>
            {
                var result = (
                    valueA: x.cache.Get("a", _ => Guid.NewGuid()),
                    valueB: x.cache.Get("b", _ => Guid.NewGuid())
                );
                x.cache.Dispose();
                return result;
            })
            .Assert(
                (ctx, result) =>
                    ctx.valueA == result.valueA
                    && ctx.valueB == result.valueB
                    && ctx.cache.IsEmpty()
            );
}
