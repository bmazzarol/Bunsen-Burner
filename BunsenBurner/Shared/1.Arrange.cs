namespace BunsenBurner;

/// <summary>
/// Shared builder function implementations for arrange steps
/// </summary>
internal static partial class Shared
{
    /// <summary>
    /// Arranges a new scenario
    /// </summary>
    /// <param name="fn">function to provide the arranged data</param>
    /// <typeparam name="TData">data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(Func<Task<TData>> fn)
        where TSyntax : struct, Syntax => new(default, fn, new HashSet<IDisposable>());

    /// <summary>
    /// Arranges a new scenario
    /// </summary>
    /// <param name="fn">function to provide the arranged data</param>
    /// <typeparam name="TData">data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(Func<TData> fn)
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(() => Task.FromResult(fn()));

    /// <summary>
    /// Arranges a new scenario
    /// </summary>
    /// <param name="data">data to arrange</param>
    /// <typeparam name="TData">data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(TData data)
        where TSyntax : struct, Syntax => Arrange<TData, TSyntax>(() => Task.FromResult(data));

    /// <summary>
    /// Arranges a new scenario
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="fn">function to provide the arranged data</param>
    /// <typeparam name="TData">data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        Func<Task<TData>> fn
    )
        where TSyntax : struct, Syntax => new(name, fn, new HashSet<IDisposable>());

    /// <summary>
    /// Arranges a new scenario
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="fn">function to provide the arranged data</param>
    /// <typeparam name="TData">data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        Func<TData> fn
    )
        where TSyntax : struct, Syntax => name.Arrange<TData, TSyntax>(() => Task.FromResult(fn()));

    /// <summary>
    /// Arranges a new scenario
    /// </summary>
    /// <param name="name">name/description</param>
    /// <param name="data">data to arrange</param>
    /// <typeparam name="TData">data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TData> Arrange<TData, TSyntax>(
        this string name,
        TData data
    )
        where TSyntax : struct, Syntax => name.Arrange<TData, TSyntax>(() => Task.FromResult(data));

    /// <summary>
    /// Rearranges a scenario
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">function from data to next data</param>
    /// <typeparam name="TData">current data</typeparam>
    /// <typeparam name="TDataNext">next data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>re arranged scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TDataNext> And<TData, TDataNext, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, Task<TDataNext>> fn
    )
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            async () =>
            {
                var result = await scenario.ArrangeScenario();
                var nextResult = await fn(result);
                return nextResult;
            },
            scenario.Disposables
        );

    /// <summary>
    /// Rearranges a scenario
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="fn">function from data to next data</param>
    /// <typeparam name="TData">current data</typeparam>
    /// <typeparam name="TDataNext">next data</typeparam>
    /// <typeparam name="TSyntax">supported syntax</typeparam>
    /// <returns>re arranged scenario</returns>
    [Pure]
    public static Scenario<TSyntax>.Arranged<TDataNext> And<TData, TDataNext, TSyntax>(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TDataNext> fn
    )
        where TSyntax : struct, Syntax => scenario.And(x => Task.FromResult(fn(x)));
}
