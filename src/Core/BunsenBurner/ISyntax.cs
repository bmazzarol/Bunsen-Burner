#pragma warning disable S101, CA1715

namespace BunsenBurner;

/// <summary>
/// Supported syntax for the <see cref="TestBuilder{TSyntax}"/>
/// </summary>
public interface ISyntax<TThis>
    where TThis : struct, ISyntax<TThis> { }
