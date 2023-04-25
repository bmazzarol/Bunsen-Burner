#pragma warning disable S3881

namespace BunsenBurner.Utility;

///  <summary>
///  When you want Bunsen Burner to not dispose of an IDisposable, wrap it in this
///  </summary>
public record ManualDisposal<T> : IDisposable
    where T : IDisposable
{
    internal ManualDisposal(T value) => Value = value;

    /// <summary>
    /// Implicit conversion back to T
    /// </summary>
    /// <param name="manualDisposal">some manual disposal</param>
    /// <returns>T</returns>
    public static implicit operator T(ManualDisposal<T> manualDisposal) => manualDisposal.Value;

    /// <summary>some disposable</summary>
    public T Value { get; init; }

    /// <inheritdoc />
    public void Dispose()
    {
        // no-op
    }
}

/// <summary>
/// Static companion object
/// </summary>
public static class ManualDisposal
{
    /// <summary>
    /// Creates a new manual disposal
    /// </summary>
    /// <param name="value">some disposable type</param>
    /// <typeparam name="T">T</typeparam>
    /// <returns>manual disposal for T</returns>
    public static ManualDisposal<T> New<T>(T value)
        where T : IDisposable => new(value);
}
