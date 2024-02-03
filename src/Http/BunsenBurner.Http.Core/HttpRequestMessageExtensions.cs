using System.Diagnostics.Contracts;

namespace BunsenBurner.Http;

/// <summary>
/// Extension methods for working with <see cref="HttpRequestMessage"/>
/// </summary>
public static class HttpRequestMessageExtensions
{
    /// <summary>
    /// Converts the <see cref="HttpRequestMessage"/> to a curl command string
    /// </summary>
    /// <param name="request">request</param>
    /// <returns>curl string</returns>
    [Pure]
    public static async Task<string> ToCurlString(this HttpRequestMessage request)
    {
        return $"Request: -X {Method()} {request.RequestUri}{Headers()}{await Body()}";

        string Method() => request.Method.ToString().ToUpperInvariant();

        string Headers() =>
            request.Headers.Any()
                ? $" {string.Join(" ", request.Headers.Select(h => $@"-H ""{h.Key}: {string.Join(", ", h.Value)}"""))}"
                : string.Empty;

        async Task<string> Body() =>
            request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty;
    }
}
