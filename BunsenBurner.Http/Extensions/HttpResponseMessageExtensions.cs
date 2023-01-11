namespace BunsenBurner.Http;

/// <summary>
/// Extension methods for working with HttpResponseMessages
/// </summary>
public static class HttpResponseMessageExtensions
{
    /// <summary>
    /// Converts the response to a curl command string
    /// </summary>
    /// <param name="response">response</param>
    /// <returns>curl string</returns>
    [Pure]
    public static async Task<string> AsCurlString(this HttpResponseMessage response)
    {
        var headers = !response.Headers.Any()
            ? string.Empty
            : $" {string.Join(" ", response.Headers.Select(h => $@"-H ""{h.Key}: {string.Join(", ", h.Value)}"""))}";
        var body = await response.AsContent();
        return $"Response: {response.StatusCode}({(int)response.StatusCode}){headers}{body}";
    }

    /// <summary>
    /// Converts a response to its content as a string
    /// </summary>
    /// <param name="response">response</param>
    /// <returns>string content</returns>
    [Pure]
    public static Task<string> AsContent(this HttpResponseMessage response) =>
        response.Content.ReadAsStringAsync();
}
