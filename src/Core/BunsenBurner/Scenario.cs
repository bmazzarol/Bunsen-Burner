#pragma warning disable S101, CA1715

namespace BunsenBurner;

/// <summary>
/// Supported syntax for the scenario
/// </summary>
public interface Syntax
{
    /// <summary>
    /// Arrange, act, assert
    /// </summary>
    public readonly struct Aaa : Syntax;

    /// <summary>
    /// Given, when, then
    /// </summary>
    public readonly struct Bdd : Syntax;
}

/// <summary>
/// <para>A <see cref="Scenario{T}"/> defines a blueprint for an executable test.</para>
/// <para>When complete, can have up to 2 generic parameters,</para>
/// <para>
/// 1. Data - the data required before acting
/// 2. Result - the result of acting to assert against
/// </para>
/// <para>This construct can represent any single test, and provides the foundation for building tests as data.</para>
/// </summary>
/// <typeparam name="TSyntax">Supported syntax</typeparam>
public abstract partial record Scenario<TSyntax>
    where TSyntax : struct, Syntax
{
    /// <summary>
    /// Optional name for the <see cref="Scenario{T}"/>
    /// </summary>
    public string Name { get; }

    /// <inheritdoc/>
    public sealed override string ToString() => Name;

    /// <summary>
    /// Stores any disposables for cleanup after the <see cref="Scenario{T}"/> is run
    /// </summary>
    internal HashSet<object> Disposables { get; }

    private void TrackPotentialDisposal<T>(T potentialDisposable)
    {
        if (potentialDisposable is IDisposable or IAsyncDisposable)
        {
            Disposables.Add(potentialDisposable);
        }
    }

    private Scenario(string? name, HashSet<object> disposables)
    {
        Name = name ?? string.Empty;
        Disposables = disposables;
    }
}
