using System.Runtime.CompilerServices;

namespace BunsenBurner.Verify.NUnit;

using BddScenario = Scenario<Syntax.Bdd>;

/// <summary>
/// Provides support for verify NUnit in constructing then steps
/// </summary>
public static class Bdd
{
    /// <summary>
    /// Then that the result has not changed since the last successful test run
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="resultToSnap">result to snapshot</param>
    /// <param name="folder">folder to save snapshot to</param>
    /// <param name="sourceFilePath">calling source file path</param>
    /// <param name="matchConfiguration">allows for configuration of the snapshot settings</param>
    /// <param name="scrubResults">flag to indicate that scrubbers should be enabled</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <typeparam name="TResultSnap">result to snapshot</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> ThenResultIsUnchanged<
        TData,
        TResult,
        TResultSnap
    >(
        this BddScenario.Acted<TData, TResult> scenario,
        Func<TResult, TResultSnap> resultToSnap,
        string folder = Shared.SnapshotFolder,
        [CallerFilePath] string sourceFilePath = "",
        Func<SettingsTask, SettingsTask>? matchConfiguration = null,
        bool scrubResults = false
    ) =>
        scenario.AssertResultIsUnchanged(
            resultToSnap,
            folder,
            sourceFilePath,
            matchConfiguration,
            scrubResults
        );

    /// <summary>
    /// Asserts that the result has not changed since the last successful test run
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="folder">folder to save snapshot to</param>
    /// <param name="sourceFilePath">calling source file path</param>
    /// <param name="matchConfiguration">allows for configuration of the snapshot settings</param>
    /// <param name="scrubResults">flag to indicate that scrubbers should be enabled</param>
    /// <typeparam name="TData">data required to when the scenario is run</typeparam>
    /// <typeparam name="TResult">result of running the scenario</typeparam>
    /// <returns>completed scenario</returns>
    [Pure]
    public static BddScenario.Asserted<TData, TResult> ThenResultIsUnchanged<TData, TResult>(
        this BddScenario.Acted<TData, TResult> scenario,
        string folder = Shared.SnapshotFolder,
        [CallerFilePath] string sourceFilePath = "",
        Func<SettingsTask, SettingsTask>? matchConfiguration = null,
        bool scrubResults = false
    ) => scenario.AssertResultIsUnchanged(folder, sourceFilePath, matchConfiguration, scrubResults);
}
