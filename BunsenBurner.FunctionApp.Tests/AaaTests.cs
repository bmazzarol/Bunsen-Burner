using System.Net;
using BunsenBurner.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.FunctionApp.Tests;
using static BunsenBurner.Aaa;

internal interface ISomeService
{
    Task<int> SomeMethod();
}

internal class SomeService : ISomeService
{
    public Task<int> SomeMethod() => Task.FromResult(1);
}

internal sealed class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder) =>
        builder.Services.AddScoped<ISomeService, SomeService>();
}

internal sealed class Function
{
    private readonly ISomeService _service;

    public Function(ISomeService service) => _service = service;

    [FunctionName(nameof(SomeFunctionTrigger))]
    public async Task<IActionResult> SomeFunctionTrigger(
        [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req
    ) => new OkObjectResult(await _service.SomeMethod());
}

public class AaaTests
{
    [Fact(DisplayName = "Executing a http trigger function works")]
    public async Task Case1() =>
        await Arrange(() => 2)
            .ActAndExecute<int, Response, Startup, Function>(
                async (i, function) =>
                {
                    var result = await function.SomeFunctionTrigger(
                        Request.GET($"/some-path/{i}").AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Assert(resp => Assert.Equal(HttpStatusCode.OK, resp.Code));
}
