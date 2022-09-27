using Bogus;

namespace BunsenBurner.Bogus;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        Func<Faker<TData>, Task<TData>> fn
    )
        where TData : class
        where TSyntax : struct, Syntax => new(default, () => fn(new Faker<TData>()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        Func<Faker<TData>, TData> fn
    )
        where TData : class
        where TSyntax : struct, Syntax =>
        AutoArrange<TData, TSyntax>(fixture => Task.FromResult(fn(fixture)));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        this string name,
        Func<Faker<TData>, Task<TData>> fn
    )
        where TData : class
        where TSyntax : struct, Syntax => new(name, () => fn(new Faker<TData>()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        this string name,
        Func<Faker<TData>, TData> fn
    )
        where TData : class
        where TSyntax : struct, Syntax =>
        name.AutoArrange<TData, TSyntax>(fixture => Task.FromResult(fn(fixture)));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        Func<Faker, Task<TData>> fn,
        string? locale = "en"
    ) where TSyntax : struct, Syntax => new(default, () => fn(new Faker(locale)));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        Func<Faker, TData> fn,
        string? locale = "en"
    ) where TSyntax : struct, Syntax =>
        AutoArrange<TData, TSyntax>(fixture => Task.FromResult(fn(fixture)), locale);

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        this string name,
        Func<Faker, Task<TData>> fn,
        string? locale = "en"
    ) where TSyntax : struct, Syntax => new(name, () => fn(new Faker(locale)));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> AutoArrange<TData, TSyntax>(
        this string name,
        Func<Faker, TData> fn,
        string? locale = "en"
    ) where TSyntax : struct, Syntax =>
        name.AutoArrange<TData, TSyntax>(fixture => Task.FromResult(fn(fixture)), locale);
}
