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
        var headers = request.Headers.Any()
            ? $" {string.Join(" ", request.Headers.Select(h => $@"-H ""{h.Key}: {string.Join(", ", h.Value)}"""))}"
            : string.Empty;
        var body =
            request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty;
        return $"Request: -X {request.Method.ToString().ToUpperInvariant()} {request.RequestUri}{headers}{body}";
    }
}
