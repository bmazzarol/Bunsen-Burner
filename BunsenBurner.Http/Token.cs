using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.IdentityModel.Tokens;

namespace BunsenBurner.Http;

using TokenPartValue = Either<object, Seq<object>>;

/// <summary>
/// Authentication token (JWT)
/// </summary>
public sealed record Token
{
    private enum TokenPart
    {
        Header,
        Claim
    }

    private Map<(string Key, TokenPart Part), TokenPartValue> Components { get; init; } =
        LanguageExt.Map<(string Key, TokenPart Part), TokenPartValue>.Empty;

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
    public Map<string, TokenPartValue> Headers =>
        Components
            .AsEnumerable()
            .Where(x => x.Key.Part == TokenPart.Header)
            .Select(x => (x.Key.Key, x.Value))
            .ToMap();

    /// <summary>
    /// Claims
    /// </summary>
    public Map<string, TokenPartValue> Claims =>
        Components
            .AsEnumerable()
            .Where(x => x.Key.Part == TokenPart.Claim)
            .Select(x => (x.Key.Key, x.Value))
            .ToMap();

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
        var rawHeader = parts[0];
        var headers = (
            JsonSerializer.Deserialize<Dictionary<string, object>>(
                Base64UrlEncoder.Decode(rawHeader)
            ) ?? new Dictionary<string, object>(StringComparer.Ordinal)
        ).Select(kv => (Key: (kv.Key, Part: TokenPart.Header), kv.Value));
        var rawBody = parts[1];
        var claims = (
            JsonSerializer.Deserialize<Dictionary<string, object>>(Base64UrlEncoder.Decode(rawBody))
            ?? new Dictionary<string, object>(StringComparer.Ordinal)
        ).Select(kv => (Key: (kv.Key, Part: TokenPart.Claim), kv.Value));

        return headers
            .Append(claims)
            .Aggregate(
                new Token(),
                (t, pair) => t.WithComponent(pair.Key.Key, pair.Key.Part, pair.Value)
            );
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

    [ExcludeFromCodeCoverage]
    private Token WithComponent<T>(string name, TokenPart part, T value)
    {
        TokenPartValue SetSeqOrValue(
            Func<object, TokenPartValue> setSingle,
            Func<Seq<object>, TokenPartValue> setSequence
        ) =>
            value switch
            {
                IEnumerable<object> seq => setSequence(seq.ToSeq()),
                _ => setSingle(value!)
            };

        return this with
        {
            Components = Components.AddOrUpdate(
                (name, part),
                data =>
                    data.Match(
                        seq => SetSeqOrValue(o => seq.Add(o!), objects => seq.Append(objects)),
                        v =>
                            SetSeqOrValue(o => Seq1(v).Add(o!), objects => Seq1(v).Append(objects)),
                        () => SetSeqOrValue(TokenPartValue.Left, TokenPartValue.Right)
                    ),
                () => SetSeqOrValue(TokenPartValue.Left, TokenPartValue.Right)
            )
        };
    }

    /// <summary>
    /// Adds a header to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="value">value of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader<T>(string name, T value) =>
        WithComponent(name, TokenPart.Header, value);

    /// <summary>
    /// Adds a header to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="values">values of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader<T>(string name, params T[] values) =>
        values.Aggregate(this, (t, v) => t.WithHeader(name, v));

    /// <summary>
    /// Adds a header to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="value">value of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader<T>(HeaderName name, T value) => WithHeader(GetDescription(name), value);

    /// <summary>
    /// Adds the header with multiple values to the token
    /// </summary>
    /// <param name="name">name of the header</param>
    /// <param name="values">values of the header</param>
    /// <returns>token with the header</returns>
    public Token WithHeader<T>(HeaderName name, params T[] values) =>
        WithHeader(GetDescription(name), values);

    /// <summary>
    /// Adds a claim to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="value">value of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim<T>(string name, T value) => WithComponent(name, TokenPart.Claim, value);

    /// <summary>
    /// Adds the claim with multiple values to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="values">values of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim<T>(string name, params T[] values) =>
        values.Aggregate(this, (t, v) => t.WithClaim(name, v));

    /// <summary>
    /// Adds a claim to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="value">value of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim<T>(ClaimName name, T value) => WithClaim(GetDescription(name), value);

    /// <summary>
    /// Adds the claim with multiple values to the token
    /// </summary>
    /// <param name="name">name of the claim</param>
    /// <param name="values">values of the claim</param>
    /// <returns>token with the claim</returns>
    public Token WithClaim<T>(ClaimName name, params T[] values) =>
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
        return Components
            .Aggregate(
                builder,
                (b, kv) =>
                {
#pragma warning disable CS8524
                    return kv.Key.Part switch
#pragma warning restore CS8524
                    {
                        TokenPart.Header => b.AddHeader(kv.Key.Key, kv.Value.Case),
                        TokenPart.Claim => b.AddClaim(kv.Key.Key, kv.Value.Case)
                    };
                }
            )
            .AddClaim(
                ClaimName.ExpirationTime,
                currentTime.AddMinutes(lifetimeSeconds).ToUnixTimeSeconds()
            )
            .AddClaim(ClaimName.IssuedAt, issuedAt.ToUnixTimeSeconds())
            .AddClaim(ClaimName.NotBefore, notBefore.ToUnixTimeSeconds())
            .Encode();
    }
}
