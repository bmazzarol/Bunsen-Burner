namespace BunsenBurner;

internal static partial class Shared
{
    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        Func<Task<TData>> fn
    )
        where TSyntax : struct, Syntax => new(default, fn, new HashSet<object>());

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(Func<TData> fn)
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(() => Task.FromResult(fn()));

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(TData data)
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(() => Task.FromResult(data));

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        Func<Task<TData>> fn
    )
        where TSyntax : struct, Syntax => new(name, fn, new HashSet<object>());

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        Func<TData> fn
    )
        where TSyntax : struct, Syntax => name.Arrange<TData, TSyntax>(() => Task.FromResult(fn()));

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        TData data
    )
        where TSyntax : struct, Syntax => name.Arrange<TData, TSyntax>(() => Task.FromResult(data));

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TDataNext> And<TData, TDataNext, TSyntax>(
        this TestBuilder<TSyntax>.Arranged<TData> test,
        Func<TData, Task<TDataNext>> fn
    )
        where TSyntax : struct, Syntax =>
        new(
            test.Name,
            async () =>
            {
                var result = await test.ArrangeStep();
                var nextResult = await fn(result);
                return nextResult;
            },
            test.Disposables
        );

    [Pure]
    internal static TestBuilder<TSyntax>.Arranged<TDataNext> And<TData, TDataNext, TSyntax>(
        this TestBuilder<TSyntax>.Arranged<TData> test,
        Func<TData, TDataNext> fn
    )
        where TSyntax : struct, Syntax => test.And(x => Task.FromResult(fn(x)));
}
