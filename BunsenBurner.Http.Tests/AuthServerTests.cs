using System.Net;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BunsenBurner.Http.Tests;

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
    [Fact(DisplayName = "An authorized endpoint can be called with a test token")]
    public static async Task Case1() =>
        await Request
            .GET("/api/test")
            .WithBearerToken(Token.New().WithClaim(ClaimName.Issuer, Constants.TestIssuer))
            .ArrangeRequest()
            .ActAndCall(TestServerBuilderOptions.New<TestStartupWithAuth>().Build())
            .Assert(r => r.Response.Code == HttpStatusCode.OK);

    [Fact(DisplayName = "An authorized endpoint can be called without a token and is unauthorized")]
    public static async Task Case2() =>
        await Request
            .GET("/api/test")
            .ArrangeRequest()
            .ActAndCall(TestServerBuilderOptions.New<TestStartupWithAuth>().Build())
            .Assert(r => r.Response.Code == HttpStatusCode.Unauthorized);
}
