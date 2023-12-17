using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.IdentityModel.Tokens;

namespace BunsenBurner.Http;

using TokenPartValues = Dictionary<string, TokenPartValue>;

/// <summary>
/// Authentication token (JWT)
/// </summary>
public sealed record Token
{
    private readonly TokenPartValues _headers = new(StringComparer.OrdinalIgnoreCase);
    private readonly TokenPartValues _claims = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Signing key used
    /// </summary>
    public string SigningKey { get; init; }

    /// <summary>
    /// Default lifetime
    /// </summary>
    public TimeSpan Lifetime { get; init; }

    /// <summary>
    /// Headers
    /// </summary>
    public IReadOnlyDictionary<string, TokenPartValue> Headers => _headers;

    /// <summary>
    /// Claims
    /// </summary>
    public IReadOnlyDictionary<string, TokenPartValue> Claims => _claims;

    private Token(string signingKey = Constants.TestSigningKey, TimeSpan? lifetime = default)
    {
        SigningKey = signingKey;
        Lifetime = lifetime ?? TimeSpan.FromSeconds(10);
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
        {
            rawToken = rawToken.Split(' ').LastOrDefault() ?? string.Empty;
        }

        var parts = rawToken.Split('.');

        if (parts.Length != 3)
            return default;

        var token = new Token();

        var rawHeader = parts[0];
        foreach (var kvp in StringToDictionary(rawHeader))
        {
            token._headers.Add(kvp.Key, new TokenPartValue(kvp.Value));
        }

        var rawBody = parts[1];
        foreach (var kvp in StringToDictionary(rawBody))
        {
            token._claims.Add(kvp.Key, new TokenPartValue(kvp.Value));
        }

        return token;

        static Dictionary<string, object> StringToDictionary(string rawJson)
        {
            var raw =
                JsonSerializer.Deserialize<Dictionary<string, object>>(
                    Base64UrlEncoder.Decode(rawJson)
                ) ?? new Dictionary<string, object>(StringComparer.Ordinal);
            return raw;
        }
    }

    private static string GetDescription<T>(T value)
        where T : struct, Enum
    {
        var valueString = value.ToString();
        return value
                .GetType()
                .GetField(valueString)
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description ?? valueString;
    }

    private static void AddToTokenComponent<T>(
        Dictionary<string, TokenPartValue> dictionary,
        string name,
        T value
    )
        where T : notnull
    {
        var tvp = new TokenPartValue(value);

        if (dictionary.TryGetValue(name, out var existing))
        {
            dictionary[name] = existing + tvp;
        }
        else
        {
            dictionary.Add(name, tvp);
        }
    }

    /// <summary>
    /// Adds a header to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="value">value of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader<T>(string name, T value)
        where T : notnull
    {
        AddToTokenComponent(_headers, name, value);
        return this;
    }

    /// <summary>
    /// Adds a header to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="values">values of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader<T>(string name, params T[] values)
        where T : notnull => WithHeader<T[]>(name, values);

    /// <summary>
    /// Adds a header to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="value">value of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader<T>(HeaderName name, T value)
        where T : notnull => WithHeader(GetDescription(name), value);

    /// <summary>
    /// Adds the header with multiple values to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="values">values of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader<T>(HeaderName name, params T[] values)
        where T : notnull => WithHeader(GetDescription(name), values);

    /// <summary>
    /// Adds a claim to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="value">value of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim<T>(string name, T value)
        where T : notnull
    {
        AddToTokenComponent(_claims, name, value);
        return this;
    }

    /// <summary>
    /// Adds the claim with multiple values to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="values">values of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim<T>(string name, params T[] values)
        where T : notnull => WithClaim<T[]>(name, values);

    /// <summary>
    /// Adds a claim to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="value">value of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim<T>(ClaimName name, T value)
        where T : notnull => WithClaim(GetDescription(name), value);

    /// <summary>
    /// Adds the claim with multiple values to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="values">values of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim<T>(ClaimName name, params T[] values)
        where T : notnull => WithClaim(GetDescription(name), values);

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

        builder = _headers.Aggregate(
            builder,
            (current, header) => current.AddHeader(header.Key, header.Value.Value)
        );

        builder = _claims.Aggregate(
            builder,
            (current, claim) => current.AddClaim(claim.Key, claim.Value.Value)
        );

        builder = builder
            .AddClaim(
                ClaimName.ExpirationTime,
                currentTime.AddMinutes(lifetimeSeconds).ToUnixTimeSeconds()
            )
            .AddClaim(ClaimName.IssuedAt, issuedAt.ToUnixTimeSeconds())
            .AddClaim(ClaimName.NotBefore, notBefore.ToUnixTimeSeconds());

        return builder.Encode();
    }
}
