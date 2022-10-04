namespace BunsenBurner;

/// <summary>
/// Shared builder function implementations for arrange steps
/// </summary>
internal static partial class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(Func<Task<TData>> fn)
        where TSyntax : struct, Syntax => new(default, fn);

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(Func<TData> fn)
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(() => Task.FromResult(fn()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        Func<Task<TData>> fn
    ) where TSyntax : struct, Syntax => new(name, fn);

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        Func<TData> fn
    ) where TSyntax : struct, Syntax => name.Arrange<TData, TSyntax>(() => Task.FromResult(fn()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TDataNext> And<TData, TDataNext, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, Task<TDataNext>> fn
    ) where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            async () =>
            {
                var result = await scenario.ArrangeScenario();
                var nextResult = await fn(result);
                return nextResult;
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TDataNext> And<TData, TDataNext, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TDataNext> fn
    ) where TSyntax : struct, Syntax => scenario.And(x => Task.FromResult(fn(x)));
}
