namespace BunsenBurner;

/// <summary>
/// DSL for building tests using the <see cref="Syntax.Aaa"/>
/// </summary>
public static partial class AaaSyntax
{
    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="fn">async function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged test</returns>
    [Pure]
    public static AaaBuilder.Arranged<TData> Arrange<TData>(Func<Task<TData>> fn) =>
        Shared.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="fn">function returning test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged test</returns>
    [Pure]
    public static AaaBuilder.Arranged<TData> Arrange<TData>(Func<TData> fn) =>
        Shared.Arrange<TData, Syntax.Aaa>(fn);

    /// <summary>
    /// Arranges the test data
    /// </summary>
    /// <param name="data">test data</param>
    /// <typeparam name="TData">data required to act on the test</typeparam>
    /// <returns>arranged test</returns>
    [Pure]
    public static AaaBuilder.Arranged<TData> Arrange<TData>(this TData data) =>
        Shared.Arrange<TData, Syntax.Aaa>(data);
}
