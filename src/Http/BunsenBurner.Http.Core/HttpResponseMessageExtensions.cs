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
        return $"Response: {response.StatusCode}({Code()}){Headers()}{await Body()}";

        string Code() => ((int)response.StatusCode).ToString(CultureInfo.InvariantCulture);

        string Headers() =>
            response.Headers.Any()
                ? $" {string.Join(" ", response.Headers.Select(h => $@"-H ""{h.Key}: {string.Join(", ", h.Value)}"""))}"
                : string.Empty;

        async Task<string> Body() => await response.Content.ReadAsStringAsync();
    }
}
