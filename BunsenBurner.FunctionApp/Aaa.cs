namespace BunsenBurner.FunctionApp;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// DSL for building tests using an arrange, act, assert syntax
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Executes the function app
    /// </summary>
    /// <param name="scenario">arranged scenario</param>
    /// <param name="functionApp">function app to test</param>
    /// <param name="fn">execute the function app, returning a result</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TResult">result of executing the function app</typeparam>
    /// <typeparam name="TFunction">function app to execute</typeparam>
    /// <returns>acted scenario</returns>
    [Pure]
    public static AaaScenario.Acted<TData, TResult> ActAndExecute<TData, TResult, TFunction>(
        this AaaScenario.Arranged<TData> scenario,
        TFunction functionApp,
        Func<TData, TFunction, Task<TResult>> fn
    ) where TFunction : class =>
        scenario.ActAndExecute<TData, TResult, TFunction, Syntax.Aaa>(functionApp, fn);
}
