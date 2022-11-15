using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;

#pragma warning disable CS8610
#pragma warning disable CS8609
#pragma warning disable CS8764

namespace BunsenBurner.FunctionApp.Models;

[ExcludeFromCodeCoverage]
internal sealed class DummyHttpContext : HttpContext
{
    public DummyHttpContext(HttpRequest request) => Request = request;

    public override void Abort() { }

    public override IFeatureCollection Features { get; } = new FeatureCollection();
    public override HttpRequest Request { get; }
    public override HttpResponse? Response => null;
    public override ConnectionInfo? Connection => null;
    public override WebSocketManager? WebSockets => null;

    [Obsolete(
        "This is obsolete and will be removed in a future version. The recommended alternative is to use Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions. See https://go.microsoft.com/fwlink/?linkid=845470."
    )]
    public override AuthenticationManager? Authentication => null;

    public override ClaimsPrincipal? User { get; set; }
    public override IDictionary<object, object> Items { get; set; } =
        new Dictionary<object, object>();
    public override IServiceProvider? RequestServices { get; set; }
    public override CancellationToken RequestAborted { get; set; }
    public override string? TraceIdentifier { get; set; }
    public override ISession? Session { get; set; }
}
