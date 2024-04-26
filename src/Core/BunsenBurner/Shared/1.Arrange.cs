namespace BunsenBurner;

internal static partial class Shared
{
    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        Func<Task<TData>> fn
    )
        where TSyntax : struct, Syntax => new(fn, new HashSet<object>());

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(Func<TData> fn)
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(() => Task.FromResult(fn()));

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(TData data)
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(() => Task.FromResult(data));
}
