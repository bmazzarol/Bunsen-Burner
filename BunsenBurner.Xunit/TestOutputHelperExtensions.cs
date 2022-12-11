using System.Diagnostics.Contracts;
using BunsenBurner.Logging;
using Xunit.Abstractions;

namespace BunsenBurner.Xunit;

/// <summary>
/// Extension for test output helper
/// </summary>
public static class TestOutputHelperExtensions
{
    /// <summary>
    /// Converts a test output helper to a sink
    /// </summary>
    /// <param name="helper">helper</param>
    /// <returns>sink</returns>
    [Pure]
    public static Sink AsSink(this ITestOutputHelper helper) => Sink.New(helper.WriteLine);
}
