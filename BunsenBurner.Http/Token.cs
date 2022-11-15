using System.Collections.Immutable;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.IdentityModel.Tokens;

namespace BunsenBurner.Http;

using Header = KeyValuePair<string, string>;
using Headers = IImmutableDictionary<string, string>;
using Claim = KeyValuePair<string, string>;
using Claims = IImmutableDictionary<string, string>;

/// <summary>
/// Authentication token (JWT)
/// </summary>
public sealed record Token
{
    private static readonly Headers Empty = ImmutableDictionary<string, string>.Empty;

    /// <summary>
    /// Signing key used
    /// </summary>
    public string SigningKey { get; init; }

    /// <summary>
    /// Default lifetime
    /// </summary>
    public TimeSpan Lifetime { get; init; }

    private Token(
        string signingKey = Constants.TestSigningKey,
        TimeSpan? lifetime = default,
        Headers? headers = default,
        Claims? claims = default
    )
    {
        SigningKey = signingKey;
        Lifetime = lifetime ?? TimeSpan.FromSeconds(10);
        Headers = headers ?? Empty;
        Claims = claims ?? Empty;
    }

    /// <summary>
    /// Creates a new token
    /// </summary>
    /// <param name="signingKey">signing key</param>
    /// <param name="lifetime">default lifetime</param>
    /// <returns>token</returns>
    public static Token New(
        string signingKey = Constants.TestSigningKey,
        TimeSpan? lifetime = default
    ) => new(signingKey, lifetime);

    /// <summary>
    /// Converts a raw JWT to a token
    /// </summary>
    /// <param name="rawToken">raw token</param>
    /// <returns>token</returns>
    public static Token? FromRaw(string rawToken)
    {
        if (
            rawToken
                .ToLower(CultureInfo.InvariantCulture)
                .StartsWith("bearer ", StringComparison.Ordinal)
        )
            rawToken = rawToken.Split(' ').LastOrDefault() ?? string.Empty;

        var parts = rawToken.Split('.');

        if (parts.Length != 3)
            return default;
        var rawHeader = parts[0];
        var headers = JsonSerializer.Deserialize<Dictionary<string, object>>(
            Base64UrlEncoder.Decode(rawHeader)
        );
        var rawBody = parts[1];
        var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(
            Base64UrlEncoder.Decode(rawBody)
        );

        return new Token
        {
            Headers =
                headers
                    ?.Select(
                        v =>
                            new KeyValuePair<string, string>(
                                v.Key,
                                v.Value.ToString() ?? string.Empty
                            )
                    )
                    .ToImmutableDictionary() ?? Empty,
            Claims =
                claims
                    ?.Select(
                        v =>
                            new KeyValuePair<string, string>(
                                v.Key,
                                v.Value.ToString() ?? string.Empty
                            )
                    )
                    .ToImmutableDictionary() ?? Empty
        };
    }

    /// <summary>
    /// User headers
    /// </summary>
    public Headers Headers { get; private init; }

    /// <summary>
    /// User claims
    /// </summary>
    public Claims Claims { get; private init; }

    private static string GetDescription<T>(T value) where T : struct, Enum
    {
        var valueString = value.ToString();
        return value
                .GetType()
                .GetField(valueString)
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description ?? valueString;
    }

    /// <summary>
    /// Adds a header to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="value">value of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader(string name, string value) =>
        this with
        {
            Headers = Headers.AddOrUpdate(new Header(name, value))
        };

    /// <summary>
    /// Adds a header to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="values">values of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader(string name, params string[] values) =>
        values.Aggregate(this, (t, v) => t.WithHeader(name, v));

    /// <summary>
    /// Adds a header to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="value">value of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader(HeaderName name, string value) =>
        WithHeader(GetDescription(name), value);

    /// <summary>
    /// Adds the header with multiple values to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="values">values of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader(HeaderName name, params string[] values) =>
        WithHeader(GetDescription(name), values);

    /// <summary>
    /// Adds a claim to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="value">value of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim(string name, string value) =>
        this with
        {
            Claims = Claims.AddOrUpdate(new Claim(name, value))
        };

    /// <summary>
    /// Adds the claim with multiple values to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="values">values of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim(string name, params string[] values) =>
        values.Aggregate(this, (t, v) => t.WithClaim(name, v));

    /// <summary>
    /// Adds a claim to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="value">value of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim(ClaimName name, string value) => WithClaim(GetDescription(name), value);

    /// <summary>
    /// Adds the claim with multiple values to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="values">values of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim(ClaimName name, params string[] values) =>
        WithClaim(GetDescription(name), values);

    /// <summary>
    /// Encodes the token
    /// </summary>
    /// <param name="now">optional date time offset</param>
    /// <returns>jwt token</returns>
    public string Encode(DateTimeOffset? now = null)
    {
        var currentTime = now ?? DateTime.Now;
        var lifetimeSeconds = (int)Lifetime.TotalSeconds;
        var issuedAt =
            lifetimeSeconds < 0 ? currentTime.AddMinutes(lifetimeSeconds * 2) : currentTime;
        var notBefore =
            lifetimeSeconds < 0 ? currentTime.AddMinutes(lifetimeSeconds * 2) : currentTime;
        var builder = JwtBuilder
            .Create()
#pragma warning disable CS0618
            .WithAlgorithm(new HMACSHA256Algorithm())
#pragma warning restore CS0618
            .WithSecret(SigningKey);
        return Headers
            .Aggregate(builder, (b, v) => b.AddHeader(v.Key, v.Value))
            .AddClaim(
                ClaimName.ExpirationTime,
                currentTime.AddMinutes(lifetimeSeconds).ToUnixTimeSeconds()
            )
            .AddClaim(ClaimName.IssuedAt, issuedAt.ToUnixTimeSeconds())
            .AddClaim(ClaimName.NotBefore, notBefore.ToUnixTimeSeconds())
            .AddClaims(Claims.Select(x => new KeyValuePair<string, object>(x.Key, x.Value)))
            .Encode();
    }
}
