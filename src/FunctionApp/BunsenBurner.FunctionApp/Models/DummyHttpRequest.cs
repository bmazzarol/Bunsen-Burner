#pragma warning disable CS8764

using System.Diagnostics.CodeAnalysis;
using System.Text;
using Flurl;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace BunsenBurner.FunctionApp.Models;

[ExcludeFromCodeCoverage]
internal sealed class DummyHttpRequest : HttpRequest, IDisposable
{
    private readonly string? _content;
    private Stream? _stream;

    internal DummyHttpRequest(HttpRequestMessage request, string content)
    {
        Method = request.Method.ToString();
        var url = new Url(request.RequestUri);
        Scheme = url.Scheme;
        Host = new HostString(url.Host);
        Path = new PathString(url.Path);
        QueryString = new QueryString("?" + url.Query);
        Query = new DummyQueryCollection(url);
        HttpContext = new DummyHttpContext(this);
        _content = content;
        ContentType =
            request.Content != null ? request.Content.Headers.ContentType.MediaType : string.Empty;
        ContentLength = _content.Length;
        foreach (var header in request.Headers)
            Headers.Add(header.Key, new StringValues(header.Value.ToArray()));
    }

    public override Task<IFormCollection> ReadFormAsync(
        CancellationToken cancellationToken = new()
    ) =>
        Task.FromResult(
            (IFormCollection)
                new FormCollection(new Dictionary<string, StringValues>(StringComparer.Ordinal))
        );

    public override HttpContext HttpContext { get; }
    public override string Method { get; set; }
    public override string Scheme { get; set; }
    public override bool IsHttps { get; set; }
    public override HostString Host { get; set; }
    public override PathString PathBase { get; set; }
    public override PathString Path { get; set; }
    public override QueryString QueryString { get; set; }
    public override IQueryCollection Query { get; set; }
    public override string? Protocol { get; set; }
    public override IHeaderDictionary Headers { get; } = new HeaderDictionary();
    public override IRequestCookieCollection? Cookies { get; set; }
    public override long? ContentLength { get; set; }
    public override string? ContentType { get; set; }

    public override Stream? Body
    {
        get => _stream ??= new MemoryStream(Encoding.UTF8.GetBytes(_content ?? string.Empty));
        set => _stream = value;
    }

    public override bool HasFormContentType => false;
    public override IFormCollection? Form { get; set; }

    public void Dispose() => _stream?.Dispose();
}
