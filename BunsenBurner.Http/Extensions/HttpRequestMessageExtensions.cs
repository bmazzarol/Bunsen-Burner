using HttpBuildR;

namespace BunsenBurner.Http;

/// <summary>
/// Extension methods for working with HttpRequestMessages
/// </summary>
public static class HttpRequestMessageExtensions
{
    /// <summary>
    /// Converts the request to a curl command string
    /// </summary>
    /// <param name="request">request</param>
    /// <returns>curl string</returns>
    [Pure]
    public static async Task<string> AsCurlString(this HttpRequestMessage request)
    {
        var headers = !request.Headers.Any()
            ? string.Empty
            : $" {string.Join(" ", request.Headers.Select(h => $@"-H ""{h.Key}: {string.Join(", ", h.Value)}"""))}";
        var body =
            request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty;
        return $"Request: -X {request.Method.ToString().ToUpperInvariant()} {request.RequestUri}{headers}{body}";
    }

    /// <summary>
    /// Clones a request
    /// </summary>
    /// <param name="request">request</param>
    /// <returns>cloned request</returns>
    [Pure]
    internal static async Task<HttpRequestMessage> CloneRequest(this HttpRequestMessage request)
    {
        static async Task<HttpContent?> CloneContent(HttpContent? content)
        {
            var ms = new MemoryStream();
            if (content == null)
                return null;

            await content.CopyToAsync(ms);
            ms.Position = 0;
            var clone = new StreamContent(ms);

            foreach (var h in content.Headers)
                clone.Headers.Add(h.Key, h.Value);
            return clone;
        }

        HttpRequestMessage clone =
            new(request.Method, request.RequestUri)
            {
                Version = request.Version,
                Content = await CloneContent(request.Content)
            };

        foreach (var kvp in request.Headers)
            clone.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);

        return clone;
    }

    /// <summary>
    /// Adds a header to the request
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="name">header name</param>
    /// <param name="value">value T</param>
    /// <param name="toStringFn">function from T to string</param>
    /// <returns>request</returns>
    [Pure]
    public static HttpRequestMessage WithHeader<T>(
        this HttpRequestMessage request,
        string name,
        T value,
        Func<T, string> toStringFn
    ) => request.WithHeaderModifications(x => x.Add(name, toStringFn(value)));

    /// <summary>
    /// Adds bearer authentication
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="token">bearer token (JWT)</param>
    /// <returns>request with bearer authentication token</returns>
    [Pure]
    public static HttpRequestMessage WithBearerToken(
        this HttpRequestMessage request,
        Token token
    ) => request.WithHeader("Authorization", $"Bearer {token.Encode()}");

    /// <summary>
    /// Adds bearer authentication
    /// </summary>
    /// <param name="request">request</param>
    /// <param name="fn">bearer token config function (JWT)</param>
    /// <returns>request with bearer authentication token</returns>
    [Pure]
    public static HttpRequestMessage WithBearerToken(
        this HttpRequestMessage request,
        Func<Token, Token> fn
    ) => request.WithBearerToken(fn(Token.New()));
}
