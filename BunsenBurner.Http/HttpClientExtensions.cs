#pragma warning disable S4830
#pragma warning disable CA5359
#pragma warning disable MA0039

using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace BunsenBurner.Http;

/// <summary>
/// Extension methods for working with http clients
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Permissive SSL certificate validator which always returns true for any certificate
    /// </summary>
    /// <remarks>For testing code only</remarks>
#if NETCOREAPP3_1_OR_GREATER
    public static readonly RemoteCertificateValidationCallback NoValidationCertificateValidationCallback =
        (_, _, _, _) => true;
#else
    public static readonly Func<
        HttpRequestMessage,
        X509Certificate2,
        X509Chain,
        SslPolicyErrors,
        bool
    > NoValidationCertificateValidationCallback = (_, _, _, _) => true;
#endif

    /// <summary>
    /// Disables the Ssl cert checks, returning a new http client
    /// </summary>
    /// <remarks>For testing code only</remarks>
    /// <param name="client">existing client</param>
    /// <returns>new client without ssl certificate checks</returns>
    [Pure]
    public static HttpClient WithoutSslCertChecks(this HttpClient client)
    {
        var newClient = new HttpClient(
#if NETCOREAPP3_1_OR_GREATER
            new SocketsHttpHandler
            {
                SslOptions = new SslClientAuthenticationOptions
                {
                    RemoteCertificateValidationCallback = NoValidationCertificateValidationCallback
                }
            }
#else
            new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    NoValidationCertificateValidationCallback
            }
#endif
        );
        newClient.Timeout = client.Timeout;
        newClient.BaseAddress = client.BaseAddress;
#if NETCOREAPP3_1_OR_GREATER
        newClient.DefaultRequestVersion = client.DefaultRequestVersion;
#endif
#if NET5_0_OR_GREATER
        newClient.DefaultVersionPolicy = client.DefaultVersionPolicy;
#endif
        newClient.MaxResponseContentBufferSize = client.MaxResponseContentBufferSize;
        return newClient;
    }
}
