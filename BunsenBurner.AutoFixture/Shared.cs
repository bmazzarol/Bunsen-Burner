using AutoFixture;
using static BunsenBurner.Shared;

namespace BunsenBurner.AutoFixture;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        Func<Fixture, Task<TData>> fn
    ) where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(() => fn(new Fixture()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        Func<Fixture, TData> fn
    ) where TSyntax : struct, Syntax =>
        AutoArrange<TData, TSyntax>(fixture => Task.FromResult(fn(fixture)));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        this string name,
        Func<Fixture, Task<TData>> fn
    ) where TSyntax : struct, Syntax => name.Arrange<TData, TSyntax>(() => fn(new Fixture()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        this string name,
        Func<Fixture, TData> fn
    ) where TSyntax : struct, Syntax =>
        name.AutoArrange<TData, TSyntax>(fixture => Task.FromResult(fn(fixture)));
}
