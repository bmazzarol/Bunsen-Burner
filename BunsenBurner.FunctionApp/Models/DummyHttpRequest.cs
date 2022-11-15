using System.Diagnostics.CodeAnalysis;
using System.Text;
using BunsenBurner.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
#pragma warning disable CS8764

namespace BunsenBurner.FunctionApp.Models;

[ExcludeFromCodeCoverage]
internal sealed class DummyHttpRequest : HttpRequest, IDisposable
{
    private readonly string? _content;
    private Stream? _stream;

    public DummyHttpRequest(Request request)
    {
        Method = request.Verb;
        Scheme = request.Url.Scheme;
        Host = new HostString(request.Url.Authority);
        Path = new PathString(request.Url.Path);
        QueryString = new QueryString("?" + request.Url.Query);
        Query = new DummyQueryCollection(request.Url);
        HttpContext = new DummyHttpContext(this);
        _content = request.Content();
        ContentType = request.ContentType();
        ContentLength = request.ContentLength();
        foreach (var kv in request.Headers)
            Headers.Add(kv.Key, kv.Value);
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
