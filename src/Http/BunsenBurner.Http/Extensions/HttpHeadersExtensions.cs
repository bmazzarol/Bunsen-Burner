using System.Net.Http.Headers;
using static System.String;

namespace BunsenBurner.Http;

/// <summary>
/// Extension methods for working with Http request/response headers
/// </summary>
public static class HttpHeadersExtensions
{
    /// <summary>
    /// Gets the header values for the name
    /// </summary>
    /// <param name="headers">headers</param>
    /// <param name="name">header name</param>
    /// <returns>values or empty</returns>
    [Pure]
    public static IEnumerable<string> Get(this HttpHeaders headers, string name) =>
        headers.TryGetValues(name, out var values) ? values : Enumerable.Empty<string>();

    /// <summary>
    /// Gets the header values for the name as a string
    /// </summary>
    /// <param name="headers">headers</param>
    /// <param name="name">header name</param>
    /// <returns>values as a string</returns>
    [Pure]
    public static string GetAsString(this HttpHeaders headers, string name) =>
        Join(",", headers.Get(name));
}
