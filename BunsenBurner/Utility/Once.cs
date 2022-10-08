namespace BunsenBurner.Utility;

/// <summary>
/// Simple async lazy that allows a test to pin data to a shared memory location and
/// ensure it only executes once.
/// </summary>
/// <remarks>This is not for production code, its a simple type that has enough features for test scenarios</remarks>
/// <typeparam name="T">some T</typeparam>
public sealed class Once<T> : Lazy<Task<T>>
{
    internal Once(Func<T> valueFactory) : base(() => Task.Factory.StartNew(valueFactory)) { }

    internal Once(Func<Task<T>> taskFactory)
        : base(() => Task.Factory.StartNew(taskFactory).Unwrap()) { }

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
    public static Once<T> New<T>(Func<T> fn) => new(fn);

    /// <summary>
    /// Creates a new once
    /// </summary>
    /// <param name="fn">function</param>
    /// <returns>once</returns>
    public static Once<T> New<T>(Func<Task<T>> fn) => new(fn);
}
