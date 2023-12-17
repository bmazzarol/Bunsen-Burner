using System.Net.Security;

namespace BunsenBurner.Http.Tests;

using static BunsenBurner.Aaa;

public static class HttpClientExtensionTests
{
    [Fact(DisplayName = "No validation certificate validator always returns true")]
    public static async Task Case1() =>
        await Arrange(HttpClientExtensions.NoValidationCertificateValidationCallback)
            .Act(validator => validator.Invoke(default!, default!, default!, SslPolicyErrors.None))
            .Assert(_ => _);
}
