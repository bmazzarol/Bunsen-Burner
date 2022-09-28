namespace BunsenBurner.FunctionApp;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    [Pure]
    internal static Scenario<TSyntax>.Arranged<TFunction> ArrangeFunctionApp<
        TStartup,
        TFunction,
        TSyntax
    >()
        where TFunction : class
        where TStartup : FunctionsStartup, new()
        where TSyntax : struct, Syntax =>
        new(default, () => Task.FromResult(FunctionAppBuilder.Create<TStartup, TFunction>()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<TFunction> ArrangeFunctionApp<
        TStartup,
        TFunction,
        TSyntax
    >(this string name)
        where TFunction : class
        where TStartup : FunctionsStartup, new()
        where TSyntax : struct, Syntax =>
        new(name, () => Task.FromResult(FunctionAppBuilder.Create<TStartup, TFunction>()));

    [Pure]
    internal static Scenario<TSyntax>.Arranged<(TData Data, TFunction FunctionApp)> AndFunctionApp<
        TData,
        TStartup,
        TFunction,
        TSyntax
    >(this Scenario<TSyntax>.Arranged<TData> scenario)
        where TFunction : class
        where TStartup : FunctionsStartup, new()
        where TSyntax : struct, Syntax =>
        new(
            scenario.Name,
            async () =>
            {
                var data = await scenario.ArrangeScenario();
                var ctx = FunctionAppBuilder.Create<TStartup, TFunction>();
                return (Data: data, FunctionApp: ctx);
            }
        );

    [Pure]
    internal static Scenario<TSyntax>.Acted<TData, TResult> ActAndExecute<
        TData,
        TResult,
        TFunction,
        TSyntax
    >(
        this Scenario<TSyntax>.Arranged<TData> scenario,
        Func<TData, TFunction> functionApp,
        Func<TData, TFunction, Task<TResult>> fn
    )
        where TFunction : class
        where TSyntax : struct, Syntax =>
        new(scenario.Name, scenario.ArrangeScenario, data => fn(data, functionApp(data)));

    [Pure]
    internal static Scenario<TSyntax>.Acted<TFunction, TResult> ActAndExecute<
        TFunction,
        TResult,
        TSyntax
    >(this Scenario<TSyntax>.Arranged<TFunction> scenario, Func<TFunction, Task<TResult>> fn)
        where TFunction : class
        where TSyntax : struct, Syntax => new(scenario.Name, scenario.ArrangeScenario, fn);
}
