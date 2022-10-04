#pragma warning disable S101
#pragma warning disable CA1715

namespace BunsenBurner;

/// <summary>
/// Supported syntax for the scenario
/// </summary>
public interface Syntax
{
    /// <summary>
    /// Arrange, act, assert
    /// </summary>
    public readonly struct Aaa : Syntax { }

    /// <summary>
    /// Given, when, then
    /// </summary>
    public readonly struct Bdd : Syntax { }
}

/// <summary>
/// A scenario defines a blueprint for an executable test.
///
/// When complete, can have up to 2 generic parameters,
///
/// 1. Data - the data required before acting
/// 2. Result - the result of acting to assert against
///
/// This construct can represent any single test, and provides the foundation for building tests as data.
/// </summary>
/// <typeparam name="TSyntax">Supported syntax</typeparam>
public abstract record Scenario<TSyntax> where TSyntax : struct, Syntax
{
    /// <summary>
    /// Optional name for the scenario
    /// </summary>
    public string Name { get; }

    /// <inheritdoc/>
    public sealed override string ToString() => Name;

    private Scenario(string? name) => Name = name ?? string.Empty;

    /// <summary>
    /// A scenario that has been arranged and is ready to act on
    /// </summary>
    /// <param name="Name">optional name</param>
    /// <param name="ArrangeScenario">operation to arrange the data for acting on the scenario</param>
    /// <typeparam name="TData">type of data required to act on the scenario</typeparam>
    public sealed record Arranged<TData>(string? Name, Func<Task<TData>> ArrangeScenario)
        : Scenario<TSyntax>(Name);

    /// <summary>
    /// A scenario that has been arranged and acted on
    /// </summary>
    /// <param name="Name">optional name</param>
    /// <param name="ArrangeScenario">operation to arrange the data for acting on the scenario</param>
    /// <param name="ActOnScenario">operation to act, based on the data, and perform the scenario</param>
    /// <typeparam name="TData">type of data required to act on the scenario</typeparam>
    /// <typeparam name="TResult">type of data returned from the result of acting</typeparam>
    public sealed record Acted<TData, TResult>(
        string? Name,
        Func<Task<TData>> ArrangeScenario,
        Func<TData, Task<TResult>> ActOnScenario
    ) : Scenario<TSyntax>(Name);

    /// <summary>
    /// A scenario that has been arranged and acted and asserted against
    /// </summary>
    /// <param name="Name">optional name</param>
    /// <param name="ArrangeScenario">operation to arrange the data for acting on the scenario</param>
    /// <param name="ActOnScenario">operation to act, based on the data, and perform the scenario</param>
    /// <param name="AssertAgainstResult">operation to assert against the result of acting</param>
    /// <typeparam name="TData">type of data required to act on the scenario</typeparam>
    /// <typeparam name="TResult">type of data returned from the result of acting</typeparam>
    public sealed record Asserted<TData, TResult>(
        string? Name,
        Func<Task<TData>> ArrangeScenario,
        Func<TData, Task<TResult>> ActOnScenario,
        Func<TData, TResult, Task> AssertAgainstResult
    ) : Scenario<TSyntax>(Name)
    {
        internal async Task Run()
        {
            var data = await ArrangeScenario();
            var result = await ActOnScenario(data);
            await AssertAgainstResult(data, result);
        }
    }
}
