namespace BunsenBurner.FunctionApp;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResult> ActAndExecute<
        TData,
        TResult,
        TFunction,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        TFunction functionApp,
        Func<TData, TFunction, Task<TResult>> fn
    )
        where TFunction : class
        where TSyntax : struct, Syntax =>
        new(scenario.Name, scenario.ArrangeScenario, data => fn(data, functionApp));
}
