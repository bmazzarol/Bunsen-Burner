using JWT.Builder;
using Xunit;

namespace BunsenBurner.Http.Jwt.Tests;

public static class TokenTests
{
    #region Example1

    [Fact(DisplayName = "Token can be decoded")]
    public static void Case1()
    {
        const string rawToken =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        var token = Token.FromRaw(rawToken);
        Assert.NotNull(token);
        Assert.Collection(
            token.Headers,
            pair =>
            {
                Assert.Equal("alg", pair.Key);
                Assert.Equal("HS256", pair.Value.Value.ToString());
            },
            pair =>
            {
                Assert.Equal("typ", pair.Key);
                Assert.Equal("JWT", pair.Value.Value.ToString());
            }
        );
        Assert.Collection(
            token.Claims,
            pair =>
            {
                Assert.Equal("sub", pair.Key);
                Assert.Equal("1234567890", pair.Value.Value.ToString());
            },
            pair =>
            {
                Assert.Equal("name", pair.Key);
                Assert.Equal("John Doe", pair.Value.Value.ToString());
            },
            pair =>
            {
                Assert.Equal("iat", pair.Key);
                Assert.Equal("1516239022", pair.Value.Value.ToString());
            }
        );
    }

    #endregion

    #region Example2

    [Fact(DisplayName = "Token can have a multi-part value")]
    public static void Case2()
    {
        var token = Token
            .New("test")
            .WithHeader(HeaderName.Type, 1, 2, 3)
            .WithClaim(ClaimName.Subject, "A", "B", "C")
            .WithClaim(ClaimName.Subject, "D");
        Assert.Collection(
            token.Headers,
            pair =>
            {
                Assert.Equal("typ", pair.Key);
                Assert.True(pair.Value.HasMultipleValues);
                Assert.Equal(new[] { 1, 2, 3 }, pair.Value.Value);
            }
        );
        Assert.Collection(
            token.Claims,
            pair =>
            {
                Assert.Equal("sub", pair.Key);
                Assert.True(pair.Value.HasMultipleValues);
                Assert.Equal(new[] { "A", "B", "C", "D" }, pair.Value.Value);
            }
        );
    }

    #endregion

    [Fact(DisplayName = "Invalid Token when decoded return null")]
    public static void Case3()
    {
        const string rawToken = "bearer invalid";
        var token = Token.FromRaw(rawToken);
        Assert.Null(token);
    }
}
