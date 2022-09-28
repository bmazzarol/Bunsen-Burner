namespace BunsenBurner.FunctionApp;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// DSL for building tests using a given, when, then syntax
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Executes the function app
    /// </summary>
    /// <param name="scenario">given scenario</param>
    /// <param name="functionApp">function app to test</param>
    /// <param name="fn">execute the function app, returning a result</param>
    /// <typeparam name="TData">given data</typeparam>
    /// <typeparam name="TResult">result of executing the function app</typeparam>
    /// <typeparam name="TFunction">function app to execute</typeparam>
    /// <returns>scenario that is run</returns>
    [Pure]
    public static BddScenario.Acted<TData, TResult> WhenExecuted<TData, TResult, TFunction>(
        this BddScenario.Arranged<TData> scenario,
        TFunction functionApp,
        Func<TData, TFunction, Task<TResult>> fn
    ) where TFunction : class => scenario.ActAndExecute(functionApp, fn);
}
