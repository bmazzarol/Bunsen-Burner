using System.Net;
using LanguageExt.ClassInstances;

namespace BunsenBurner.Http;

using Headers = Map<OrdStringOrdinal, string, Set<OrdStringOrdinal, string>>;

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
            toMap<OrdStringOrdinal, string, Set<OrdStringOrdinal, string>>(
                httpResp.Headers.Select(kv => (kv.Key, toSet<OrdStringOrdinal, string>(kv.Value)))
            )
        );
    }
}
