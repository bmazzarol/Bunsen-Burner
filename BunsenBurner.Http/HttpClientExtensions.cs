#pragma warning disable S4830
#pragma warning disable CA5359

using System.Net.Security;

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
    public static readonly RemoteCertificateValidationCallback NoValidationCertificateValidationCallback =
        (_, _, _, _) => true;

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
            new SocketsHttpHandler
            {
                SslOptions = new SslClientAuthenticationOptions
                {
                    RemoteCertificateValidationCallback = NoValidationCertificateValidationCallback
                }
            }
        );
        newClient.Timeout = client.Timeout;
        newClient.BaseAddress = client.BaseAddress;
        newClient.DefaultRequestVersion = client.DefaultRequestVersion;
        newClient.DefaultVersionPolicy = client.DefaultVersionPolicy;
        newClient.MaxResponseContentBufferSize = client.MaxResponseContentBufferSize;
        return newClient;
    }
}
