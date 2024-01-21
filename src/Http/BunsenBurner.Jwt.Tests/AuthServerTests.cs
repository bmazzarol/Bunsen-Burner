#pragma warning disable CA1822

using System.Net;
using System.Net.Mime;
using BunsenBurner.Extensions;
using BunsenBurner.Jwt.Extensions;
using HttpBuildR;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using static BunsenBurner.TestServerConstants;
using Req = System.Net.Http.HttpMethod;

namespace BunsenBurner.Jwt.Tests;

internal sealed class TestStartupWithAuth
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = false
                };
            });
        services.AddMvc(x => x.EnableEndpointRouting = false);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseMvc();
    }
}

[Route("api/[controller]")]
[ApiController]
public sealed class TestController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public ActionResult<IEnumerable<string>> Get() =>
        new[] { "value1", "value2", "value3", "value4", "value5" };
}

public static class AuthServerTests
{
    #region Example1

    [Fact(DisplayName = "An authorized endpoint can be called with a test token")]
    public static async Task Case1() =>
        await
        // create a request
        // using HTTP-BuildR https://github.com/bmazzarol/Http-BuildR)
        Req.Get.To("/api/test")
            // now add the test token
            .WithTestBearerToken(
                // pass it a shared signing key, some shared string
                Token
                    .New(SigningKey)
                    // now headers and claims can be added
                    .WithClaim(ClaimName.Issuer, Issuer)
                    .WithHeader(HeaderName.ContentType, MediaTypeNames.Application.Json)
            )
            .ArrangeData()
            .Act(
                new TestServerBuilder.Options
                {
                    Startup = typeof(TestStartupWithAuth),
                    SigningKey = SigningKey,
                    Issuer = Issuer
                }
                    .Build()
                    .CallTestServer()
            )
            .Assert(ctx => ctx.Response.IsSuccessStatusCode);

    #endregion

    [Fact(DisplayName = "An authorized endpoint can be called without a token and is unauthorized")]
    public static async Task Case2() =>
        await Req.Get.To("/api/test")
            .ArrangeData()
            .Act(
                async req =>
                    await new TestServerBuilder.Options { Startup = typeof(TestStartupWithAuth) }
                        .Build()
                        .CallTestServer(req)
            )
            .Assert(r => r.StatusCode == HttpStatusCode.Unauthorized);
}
