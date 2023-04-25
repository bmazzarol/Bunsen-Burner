using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace BunsenBurner.Http;

using Matcher = HttpRequestPredicate;

/// <summary>
/// Pre built predicates for matching http requests
/// </summary>
public static class HttpRequestMatchers
{
    /// <summary>
    /// Matches requests with the given HTTP method
    /// </summary>
    /// <param name="method">method to match</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasMethod(HttpMethod method) => Matcher.New(x => x.Method == method);

    [Pure]
    private static Regex WildCardToRegular(string value) =>
        new(
            "^"
                + Regex
                    .Escape(value)
#if NETCOREAPP3_1_OR_GREATER
                    .Replace("\\?", ".", StringComparison.Ordinal)
                    .Replace("\\*", ".*", StringComparison.Ordinal)
#else
                .Replace("\\?", ".").Replace("\\*", ".*")
#endif
                + "$",
            RegexOptions.None,
            1 * minute
        );

    /// <summary>
    /// Matches requests with the given HTTP request uri
    /// </summary>
    /// <param name="matcher">regex matcher</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasRequestUri(Func<Uri, bool> matcher) =>
        Matcher.New(x => x.RequestUri != null && matcher.Invoke(x.RequestUri));

    /// <summary>
    /// Matches requests with the given HTTP request uri
    /// </summary>
    /// <param name="matcher">regex matcher</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasRequestUri(Regex matcher) =>
        HasRequestUri(x => matcher.IsMatch(x.OriginalString));

    /// <summary>
    /// Matches requests with the given HTTP request uri
    /// </summary>
    /// <param name="matcher">string matcher support * and ? wildcards</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasRequestUri(string matcher) =>
        HasRequestUri(WildCardToRegular(matcher));

    /// <summary>
    /// Matches requests with the given HTTP request header
    /// </summary>
    /// <param name="matcher">header matching predicate</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasHeader(Func<HttpRequestHeaders, bool> matcher) =>
        Matcher.New(x => matcher(x.Headers));

    /// <summary>
    /// Matches requests with the given HTTP request header
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="value">value, supports wildcards</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasHeader(string name, Regex value) =>
        HasHeader(h =>
        {
            var headerValue = h.GetAsString(name);
            return !string.IsNullOrWhiteSpace(headerValue) && value.IsMatch(headerValue);
        });

    /// <summary>
    /// Matches requests with the given HTTP request header
    /// </summary>
    /// <param name="name">name</param>
    /// <param name="value">value, supports wildcards</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasHeader(string name, string value) =>
        HasHeader(name, WildCardToRegular(value));

    /// <summary>
    /// Matches requests with the given HTTP request bearer token
    /// </summary>
    /// <param name="token">some token</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasBearerToken(string token) =>
        HasHeader(
            h =>
                h.Authorization is { Scheme: "Bearer", Parameter: { } } value
                && value.Parameter.Equals(token, StringComparison.Ordinal)
        );

    /// <summary>
    /// Matches requests with the given HTTP request content
    /// </summary>
    /// <param name="matcher">content</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasContent(Func<HttpContent, bool> matcher) =>
        Matcher.New(x => x.Content != null && matcher(x.Content));

    /// <summary>
    /// Matches requests with the given HTTP request content
    /// </summary>
    /// <param name="matcher">matcher</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasContent(Func<string, bool> matcher) =>
        HasContent(content =>
        {
            var reqContent = content.ReadAsStringAsync().Result;
            return !string.IsNullOrWhiteSpace(reqContent) && matcher(reqContent);
        });

    /// <summary>
    /// Matches requests with the given HTTP request content
    /// </summary>
    /// <param name="matcher">regex matcher</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasContent(Regex matcher) => HasContent(matcher.IsMatch);

    /// <summary>
    /// Matches requests with the given HTTP request content
    /// </summary>
    /// <param name="matcher">string matcher support * and ? wildcards</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasContent(string matcher) => HasContent(WildCardToRegular(matcher));

    /// <summary>
    /// Matches requests with the given HTTP request content
    /// </summary>
    /// <param name="matcher">matcher against deserialized T</param>
    /// <param name="options">json options</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasJsonContent<T>(
        Func<T, bool> matcher,
        JsonSerializerOptions? options = default
    ) =>
        HasContent(content =>
        {
            try
            {
                var t = JsonSerializer.Deserialize<T>(content, options);
                return t != null && matcher(t);
            }
            catch
            {
                return false;
            }
        });

    /// <summary>
    /// Matches requests with the given HTTP request content
    /// </summary>
    /// <param name="matcher">matcher against deserialized T</param>
    /// <param name="serializer">xml serializer to use</param>
    /// <returns>matcher</returns>
    [Pure]
    public static Matcher HasXmlContent<T>(
        Func<T, bool> matcher,
        XmlSerializer? serializer = default
    )
        where T : class, new() =>
        HasContent(content =>
        {
            try
            {
                serializer ??= new XmlSerializer(typeof(T));
                var reader = new StringReader(content);
                var t = (T?)
                    serializer.Deserialize(
                        XmlReader.Create(
                            reader,
                            new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore }
                        )
                    );
                return t != null && matcher(t);
            }
            catch
            {
                return false;
            }
        });
}
