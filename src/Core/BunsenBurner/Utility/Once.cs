namespace BunsenBurner.Utility;

/// <summary>
/// Simple async lazy that allows a test to pin data to a shared memory location and
/// ensure it only executes once.
/// </summary>
/// <remarks>This is not for production code, its a simple type that has enough features for test scenarios</remarks>
/// <typeparam name="T">some T</typeparam>
public sealed class Once<T> : Lazy<Task<T>>
{
    internal Once(Func<T> valueFactory)
        : base(() => Task.Factory.StartNew(valueFactory), isThreadSafe: true) { }

    internal Once(Func<Task<T>> taskFactory)
        : base(() => Task.Factory.StartNew(taskFactory).Unwrap(), isThreadSafe: true) { }

    /// <summary>
    /// Task awaiter for once
    /// </summary>
    /// <returns>task awaiter for T</returns>
    public TaskAwaiter<T> GetAwaiter() => Value.GetAwaiter();
}

/// <summary>
/// Static constructors for once
/// </summary>
public static class Once
{
    /// <summary>
    /// Creates a new once
    /// </summary>
    /// <param name="fn">function</param>
    /// <returns>once</returns>
    [Pure]
    public static Once<T> New<T>(Func<T> fn) => new(fn);

    /// <summary>
    /// Creates a new once
    /// </summary>
    /// <param name="fn">function</param>
    /// <returns>once</returns>
    [Pure]
    public static Once<T> New<T>(Func<Task<T>> fn) => new(fn);

    /// <summary>
    /// Select (Map) for once
    /// </summary>
    /// <param name="once">Once of TA</param>
    /// <param name="fn">fn from TA to TB</param>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>Once of TB</returns>
    [Pure]
    public static Once<TB> Select<TA, TB>(this Once<TA> once, Func<TA, TB> fn) =>
        new(async () => fn(await once));

    /// <summary>
    /// SelectMany (Bind) for once
    /// </summary>
    /// <param name="once">Once of TA</param>
    /// <param name="fn">fn from TA to once of TB</param>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <returns>Once of TB</returns>
    [Pure]
    public static Once<TB> SelectMany<TA, TB>(this Once<TA> once, Func<TA, Once<TB>> fn) =>
        new(async () => await fn(await once));

    /// <summary>
    /// SelectMany (Bind) for once
    /// </summary>
    /// <param name="once">Once of TA</param>
    /// <param name="mapFn">fn from TA to once of TB</param>
    /// <param name="projectFn">projection function from TA, TB to TC</param>
    /// <typeparam name="TA">some TA</typeparam>
    /// <typeparam name="TB">some TB</typeparam>
    /// <typeparam name="TC">some TC</typeparam>
    /// <returns>Once of TB</returns>
    [Pure]
    public static Once<TC> SelectMany<TA, TB, TC>(
        this Once<TA> once,
        Func<TA, Once<TB>> mapFn,
        Func<TA, TB, TC> projectFn
    ) =>
        new(async () =>
        {
            var ta = await once;
            var tb = await mapFn(ta);
            return projectFn(ta, tb);
        });
}
