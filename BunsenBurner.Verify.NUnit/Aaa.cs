using System.Runtime.CompilerServices;

namespace BunsenBurner.Verify.NUnit;

using AaaScenario = Scenario<Syntax.Aaa>;

/// <summary>
/// Provides support for verify NUnit in constructing arrange steps
/// </summary>
public static class Aaa
{
    /// <summary>
    /// Asserts that the result has not changed since the last successful test run
    /// </summary>
    /// <param name="scenario">scenario</param>
    /// <param name="resultToSnap">result to snapshot</param>
    /// <param name="folder">folder to save snapshot to</param>
    /// <param name="sourceFilePath">calling source file path</param>
    /// <param name="matchConfiguration">allows for configuration of the snapshot settings</param>
    /// <param name="scrubResults">flag to indicate that scrubbers should be enabled</param>
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TResult">result of acting</typeparam>
    /// <typeparam name="TResultSnap">result to snapshot</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> AssertResultIsUnchanged<
        TData,
        TResult,
        TResultSnap
    >(
        this AaaScenario.Acted<TData, TResult> scenario,
        Func<TResult, TResultSnap> resultToSnap,
        string folder = Shared.SnapshotFolder,
        [CallerFilePath] string sourceFilePath = "",
        Func<SettingsTask, SettingsTask>? matchConfiguration = null,
        bool scrubResults = false
    ) =>
        scenario.AssertResultIsUnchanged<TData, TResult, TResultSnap, Syntax.Aaa>(
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
    /// <typeparam name="TData">arranged data</typeparam>
    /// <typeparam name="TResult">result of acting to snapshot</typeparam>
    /// <returns>asserted scenario</returns>
    [Pure]
    public static AaaScenario.Asserted<TData, TResult> AssertResultIsUnchanged<TData, TResult>(
        this AaaScenario.Acted<TData, TResult> scenario,
        string folder = Shared.SnapshotFolder,
        [CallerFilePath] string sourceFilePath = "",
        Func<SettingsTask, SettingsTask>? matchConfiguration = null,
        bool scrubResults = false
    ) =>
        scenario.AssertResultIsUnchanged<TData, TResult, Syntax.Aaa>(
            folder,
            sourceFilePath,
            matchConfiguration,
            scrubResults
        );
}
