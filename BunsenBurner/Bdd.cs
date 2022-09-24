using System.Runtime.CompilerServices;

namespace BunsenBurner;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> Given<TData>(Func<Task<TData>> fn) =>
        Shared.Arrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="fn">function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> Given<TData>(Func<TData> fn) =>
        Shared.Arrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> Given<TData>(
        this string name,
        Func<Task<TData>> fn
    ) => name.Arrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Given the scenario data
    /// </summary>
    /// <param name="name">name/description for the scenario</param>
    /// <param name="fn">async function returning scenario data</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TData> Given<TData>(this string name, Func<TData> fn) =>
        name.Arrange<TData, Syntax.Bdd>(fn);

    /// <summary>
    /// Allows for additional given steps
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async function transforming scenario data into scenario data</param>
    /// <typeparam name="TData">initial scenario data</typeparam>
    /// <typeparam name="TDataNext">next scenario data</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TDataNext> And<TData, TDataNext>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, Task<TDataNext>> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Allows for additional given steps
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">function transforming test data into test data</param>
    /// <typeparam name="TData">initial scenario data</typeparam>
    /// <typeparam name="TDataNext">next scenario data</typeparam>
    /// <returns>scenario with the given data</returns>
    [Pure]
    public static BddScenario.Arranged<TDataNext> And<TData, TDataNext>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, TDataNext> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// When the scenario is run
    /// </summary>
    /// <param name="scenario">ready to run scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResult> When<TData, TResult>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, Task<TResult>> fn
    ) => scenario.Act(fn);

    /// <summary>
    /// When the scenario is run
    /// </summary>
    /// <param name="scenario">ready to run scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResult> When<TData, TResult>(
        this BddScenario.Arranged<TData> scenario,
        Func<TData, TResult> fn
    ) => scenario.Act(fn);

    /// <summary>
    /// Allows for additional when steps
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">async when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">initial result of running the scenario</typeparam>
    /// <typeparam name="TResultNext">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this BddScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task<TResultNext>> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Allows for additional when steps
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="fn">when function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">initial result of running the scenario</typeparam>
    /// <typeparam name="TResultNext">result of running the scenario</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResultNext> And<TData, TResult, TResultNext>(
        this BddScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, TResultNext> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">async then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> Then<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) => Shared.Assert(scenario, fn);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">async then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> Then<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) => Shared.Assert(scenario, fn);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> Then<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) => Shared.Assert(scenario, fn);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> Then<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        Action<TResult> fn
    ) => Shared.Assert(scenario, fn);

    /// <summary>
    /// Then verify the scenario fails
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then on failure function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, Exception> ThenFailsWith<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        Func<TData, Exception, Task> fn
    ) => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Then verify the scenario fails
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then on failure function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, Exception> ThenFailsWith<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        Func<Exception, Task> fn
    ) => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Then verify the scenario fails
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then on failure function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, Exception> ThenFailsWith<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        Action<TData, Exception> fn
    ) => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Then verify the scenario fails
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then on failure function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, Exception> ThenFailsWith<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        Action<Exception> fn
    ) => Shared.AssertFailsWith(scenario, fn);

    /// <summary>
    /// Allows for additional then steps
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> And<TData, TResult>(
        this BddScenario.Asserted<TData, TResult> scenario,
        Func<TData, TResult, Task> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Allows for additional then steps
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> And<TData, TResult>(
        this BddScenario.Asserted<TData, TResult> scenario,
        Func<TResult, Task> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Allows for additional then steps
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> And<TData, TResult>(
        this BddScenario.Asserted<TData, TResult> scenario,
        Action<TData, TResult> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Allows for additional then steps
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> And<TData, TResult>(
        this BddScenario.Asserted<TData, TResult> scenario,
        Action<TResult> fn
    ) => Shared.And(scenario, fn);

    /// <summary>
    /// Awaiter for a scenario so it can be run
    /// </summary>
    /// <param name="scenario">scenario to run</param>
    /// <typeparam name="TData">context</typeparam>
    /// <typeparam name="TResult">result</typeparam>
    /// <returns>awaiter</returns>
    public static TaskAwaiter GetAwaiter<TData, TResult>(
        this BddScenario.Asserted<TData, TResult> scenario
    ) => scenario.Run().GetAwaiter();
}
