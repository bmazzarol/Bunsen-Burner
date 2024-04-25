using System.Linq.Expressions;

namespace BunsenBurner;

public static partial class BddSyntax
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
    public static ScenarioBuilder.Asserted<TData, TResult> Then<TData, TResult>(
        this ScenarioBuilder.Acted<TData, TResult> scenario,
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
    public static ScenarioBuilder.Asserted<TData, TResult> Then<TData, TResult>(
        this ScenarioBuilder.Acted<TData, TResult> scenario,
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
    public static ScenarioBuilder.Asserted<TData, TResult> Then<TData, TResult>(
        this ScenarioBuilder.Acted<TData, TResult> scenario,
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
    public static ScenarioBuilder.Asserted<TData, TResult> Then<TData, TResult>(
        this ScenarioBuilder.Acted<TData, TResult> scenario,
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
    public static ScenarioBuilder.Asserted<TData, TResult> Then<TData, TResult>(
        this ScenarioBuilder.Acted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    ) => scenario.Assert(expression);

    /// <summary>
    /// Then verify the scenario
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="expression">then expression</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static ScenarioBuilder.Asserted<TData, TResult> Then<TData, TResult>(
        this ScenarioBuilder.Acted<TData, TResult> scenario,
        Expression<Func<TData, TResult, bool>> expression
    ) => scenario.Assert(expression);

    /// <summary>
    /// Allows for additional then steps
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="fn">then function</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static ScenarioBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this ScenarioBuilder.Asserted<TData, TResult> scenario,
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
    public static ScenarioBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this ScenarioBuilder.Asserted<TData, TResult> scenario,
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
    public static ScenarioBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this ScenarioBuilder.Asserted<TData, TResult> scenario,
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
    public static ScenarioBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this ScenarioBuilder.Asserted<TData, TResult> scenario,
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
    public static ScenarioBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this ScenarioBuilder.Asserted<TData, TResult> scenario,
        Expression<Func<TResult, bool>> expression
    ) => scenario.And<TData, TResult, Syntax.Bdd>(expression);

    /// <summary>
    /// Allows for additional then steps
    /// </summary>
    /// <param name="scenario">run scenario</param>
    /// <param name="expression">then expression</param>
    /// <typeparam name="TData">scenario data</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static ScenarioBuilder.Asserted<TData, TResult> And<TData, TResult>(
        this ScenarioBuilder.Asserted<TData, TResult> scenario,
        Expression<Func<TData, TResult, bool>> expression
    ) => scenario.And<TData, TResult, Syntax.Bdd>(expression);
}
