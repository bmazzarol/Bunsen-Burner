namespace BunsenBurner;

/// <summary>
/// <para>A <see cref="TestBuilder{TSyntax}"/> defines a blueprint for an executable test.</para>
/// <para>When complete, can have up to 2 generic parameters,</para>
/// <para>
/// <ul>
/// <li>Data - the `data` required before acting</li>
/// <li>Result - the `result` of acting to assert against</li>
/// </ul>
/// </para>
/// <para>This construct can represent any single test, and provides the foundation for building tests as data.</para>
/// <para>The <see cref="TestBuilder{TSyntax}"/> will also manage disposal of all `data` and `result` values
/// that are used as long as they implement <see cref="IDisposable"/> or <see cref="IAsyncDisposable"/>.</para>
/// </summary>
/// <typeparam name="TSyntax">Supported syntax</typeparam>
public abstract partial record TestBuilder<TSyntax>
    where TSyntax : struct, Syntax
{
    /// <summary>
    /// Optional name for the test
    /// </summary>
    public string Name { get; }

    /// <inheritdoc/>
    public sealed override string ToString() => Name;

    /// <summary>
    /// Stores any disposables for cleanup after the <see cref="TestBuilder{TSyntax}"/> is run
    /// </summary>
    internal HashSet<object> Disposables { get; }

    private void TrackPotentialDisposal<T>(T potentialDisposable)
    {
        if (potentialDisposable is IDisposable or IAsyncDisposable)
        {
            Disposables.Add(potentialDisposable);
        }
    }

    private TestBuilder(string? name, HashSet<object> disposables)
    {
        Name = name ?? string.Empty;
        Disposables = disposables;
    }
}
