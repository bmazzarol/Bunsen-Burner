using System.Net;

namespace BunsenBurner.Http;

using Headers = IEnumerable<Header>;

/// <summary>
/// HTTP Response
/// </summary>
/// <param name="Code">status code</param>
/// <param name="Content">response content</param>
/// <param name="MediaType">media type</param>
/// <param name="Headers">response headers</param>
public sealed record Response(
    HttpStatusCode Code,
    string? Content,
    string? MediaType,
    Headers Headers
)
{
    /// <summary>
    /// Raw status code
    /// </summary>
    public int RawStatusCode { get; } = (int)Code;

    /// <summary>
    /// Content length
    /// </summary>
    public int? Length { get; } = Content?.Length;

    internal static async Task<Response> New(HttpResponseMessage httpResp)
    {
        var content = await httpResp.Content.ReadAsStringAsync();
        return new Response(
            httpResp.StatusCode,
            content,
            httpResp.Content.Headers.ContentType?.MediaType,
            httpResp.Headers.Select(x => new Header(x.Key, string.Join(',', x.Value)))
        );
    }
}
