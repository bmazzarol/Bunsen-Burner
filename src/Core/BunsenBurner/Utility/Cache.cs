using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace BunsenBurner.Utility;

/// <summary>
/// Very simple cache, with support for concurrent access
/// </summary>
/// <typeparam name="TValue">some value to cache per key</typeparam>
public sealed class Cache<TValue> : IDisposable
{
    private readonly ConcurrentDictionary<string, Lazy<TValue>> _concurrentDictionary;

    [ExcludeFromCodeCoverage]
    internal Cache(bool cleanupOnProcessExit = true)
    {
        _concurrentDictionary = new ConcurrentDictionary<string, Lazy<TValue>>(
            StringComparer.Ordinal
        );

        if (cleanupOnProcessExit)
            AppDomain.CurrentDomain.ProcessExit += (_, _) => Dispose();
    }

    /// <summary>
    /// Gets a value from the cache
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="factory">factory to build a new TValue</param>
    /// <returns>TValue</returns>
    public TValue Get(string key, Func<string, TValue> factory) =>
        _concurrentDictionary.GetOrAdd(key, x => new Lazy<TValue>(() => factory(x))).Value;

    /// <summary>
    /// Clears the cache, calling dispose on TValue if it implements IDisposable
    /// </summary>
    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        foreach (var (key, _) in _concurrentDictionary)
        {
            if (!_concurrentDictionary.TryRemove(key, out var v))
            {
                continue;
            }

            if (!v.IsValueCreated)
            {
                continue;
            }

            if (v.Value is IDisposable d)
            {
#pragma warning disable S3966
                d.Dispose();
#pragma warning restore S3966
            }
        }

        _concurrentDictionary.Clear();
    }

    /// <summary>
    /// True if the cache is empty
    /// </summary>
    /// <returns>true if the cache is empty</returns>
    public bool IsEmpty() => _concurrentDictionary.IsEmpty;
}

/// <summary>
/// Static functions for working with Cache
/// </summary>
public static class Cache
{
    /// <summary>
    /// Creates a new cache with a string key
    /// </summary>
    /// <param name="cleanupOnProcessExit">flag to indicate that the cache should clean itself up on process exit</param>
    /// <returns>cache</returns>
    [Pure]
    public static Cache<TValue> New<TValue>(bool cleanupOnProcessExit = true) =>
        new(cleanupOnProcessExit);
}
