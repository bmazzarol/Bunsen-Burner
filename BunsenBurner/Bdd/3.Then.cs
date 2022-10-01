using System.Linq.Expressions;

namespace BunsenBurner;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static partial class Bdd
{
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
    ) => scenario.Assert(fn);

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
    ) => scenario.Assert(fn);

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
    ) => scenario.Assert(fn);

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
    ) => scenario.Assert(fn);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="expression">then expression</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> Then<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    ) => scenario.Assert(expression);

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
    ) => scenario.AssertFailsWith(fn);

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
    ) => scenario.AssertFailsWith(fn);

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
    ) => scenario.AssertFailsWith(fn);

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
    ) => scenario.AssertFailsWith(fn);

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
    /// Allows for additional then steps
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="expression">then expression</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> And<TData, TResult>(
        this BddScenario.Asserted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    ) => scenario.And<TData, TResult, Syntax.Bdd>(expression);

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
