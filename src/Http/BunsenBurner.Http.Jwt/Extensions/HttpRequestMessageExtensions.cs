using System.Diagnostics.Contracts;

namespace BunsenBurner.Http.Jwt.Extensions;

/// <summary>
/// Extension methods for working with <see cref="HttpRequestMessage"/>
/// </summary>
public static class HttpRequestMessageExtensions
{
    /// <summary>
    /// Adds bearer authentication using a test JWT <see cref="Token"/>
    /// </summary>
    /// <param name="request"><see cref="HttpRequestMessage"/> to send</param>
    /// <param name="token">bearer token (JWT)</param>
    /// <returns>request with bearer authentication token</returns>
    [Pure]
    public static HttpRequestMessage WithTestBearerToken(
        this HttpRequestMessage request,
        Token token
    )
    {
        request.Headers.Add("Authorization", $"Bearer {token.Encode()}");
        return request;
    }
}
