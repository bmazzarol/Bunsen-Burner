using System.Diagnostics.Contracts;
using System.Globalization;

namespace BunsenBurner.Http;

/// <summary>
/// Extension methods for working with <see cref="HttpResponseMessage"/>
/// </summary>
public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Converts the <see cref="HttpResponseMessage"/> to a curl command string
    /// </summary>
    /// <param name="response">response</param>
    /// <returns>curl string</returns>
    [Pure]
    public static async Task<string> ToCurlString(this HttpResponseMessage response)
    {
        var headers = response.Headers.Any()
            ? $" {string.Join(" ", response.Headers.Select(h => $@"-H ""{h.Key}: {string.Join(", ", h.Value)}"""))}"
            : string.Empty;
        var body = await response.Content.ReadAsStringAsync();
        var code = ((int)response.StatusCode).ToString(CultureInfo.InvariantCulture);
        return $"Response: {response.StatusCode}({code}){headers}{body}";
    }
}
