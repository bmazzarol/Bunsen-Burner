using System.Runtime.CompilerServices;

namespace BunsenBurner.Verify.NUnit;

/// <summary>
/// Shared builder function implementations
/// </summary>
internal static class Shared
{
    internal const string SnapshotFolder = "./__snapshots__";

    internal static SettingsTask ShouldMatchSnapshot<TA, TB>(
        this TA value,
        Func<TA, TB>? resultToSnap = null,
        string folder = SnapshotFolder,
        [CallerFilePath] string sourceFilePath = ""
    ) =>
        VerifyNUnit
            .Verifier
            .Verify(
                resultToSnap != null ? resultToSnap(value) as object : value,
                sourceFile: sourceFilePath
            )
            .UseDirectory(folder);

    internal static SettingsTask ShouldMatchSnapshotWithoutScrubbing<TA, TB>(
        this TA value,
        Func<TA, TB>? resultToSnap = null,
        string folder = SnapshotFolder,
        [CallerFilePath] string sourceFilePath = ""
    ) =>
        value
            .ShouldMatchSnapshot(resultToSnap, folder, sourceFilePath)
            .DontScrubGuids()
            .DontScrubDateTimes();

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TContext, TResult> AssertResultIsUnchanged<
        TContext,
        TResult,
        TResultSnap,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<TContext, TResult> scenario,
        Func<TResult, TResultSnap> resultToSnap,
        string folder = SnapshotFolder,
        [CallerFilePath] string sourceFilePath = "",
        Func<SettingsTask, SettingsTask>? matchConfiguration = null,
        bool scrubResults = false
    )
        where TSyntax : struct, Syntax =>
        scenario.Assert(async result =>
        {
            var settings = scrubResults
                ? result.ShouldMatchSnapshot(resultToSnap, folder, sourceFilePath)
                : result.ShouldMatchSnapshotWithoutScrubbing(resultToSnap, folder, sourceFilePath);
            settings =
                scenario.Name != string.Empty
                    ? settings.UseTextForParameters(scenario.Name)
                    : settings;
            await (matchConfiguration != null ? matchConfiguration(settings) : settings);
        });

    [Pure]
    internal static Scenario<TSyntax>.Asserted<TContext, TResult> AssertResultIsUnchanged<
        TContext,
        TResult,
        TSyntax
    >(
        this Scenario<TSyntax>.Acted<TContext, TResult> scenario,
        string folder = SnapshotFolder,
        [CallerFilePath] string sourceFilePath = "",
        Func<SettingsTask, SettingsTask>? matchConfiguration = null,
        bool scrubResults = false
    )
        where TSyntax : struct, Syntax =>
        scenario.AssertResultIsUnchanged(
            static _ => _,
            folder,
            sourceFilePath,
            matchConfiguration,
            scrubResults
        );
}
