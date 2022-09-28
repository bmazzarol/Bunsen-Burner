using System.Net;
using System.Web.Http;
using BunsenBurner.Http;
using Flurl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.FunctionApp.Tests;
using static BunsenBurner.Aaa;
using static Aaa;

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
    )
    {
        if (req.Query.ContainsKey("empty"))
            return new EmptyResult();
        if (req.Query.ContainsKey("fail"))
            return new InternalServerErrorResult();
        if (req.Query.ContainsKey("noBody"))
            return new OkResult();
        return new OkObjectResult(await _service.SomeMethod());
    }
}

public class AaaTests
{
    [Fact(DisplayName = "Executing a http trigger function works")]
    public async Task Case1() =>
        await ArrangeFunctionApp<Startup, Function>()
            .ActAndExecute(async function =>
            {
                var result = await function.SomeFunctionTrigger(
                    Request.GET($"/some-path").AsHttpRequest()
                );
                return result.AsResponse();
            })
            .Assert(resp =>
            {
                Assert.Equal(HttpStatusCode.OK, resp.Code);
                Assert.Equal("1", resp.Content);
            });

    [Fact(DisplayName = "Executing a http trigger function works with description")]
    public async Task Case2() =>
        await "Some description"
            .ArrangeFunctionApp<Startup, Function>()
            .ActAndExecute(async function =>
            {
                var result = await function.SomeFunctionTrigger(
                    Request.GET($"/some-path").AsHttpRequest()
                );
                return result.AsResponse();
            })
            .Assert(resp =>
            {
                Assert.Equal(HttpStatusCode.OK, resp.Code);
                Assert.Equal("1", resp.Content);
            });

    [Fact(DisplayName = "Executing a http trigger function works with no response body")]
    public async Task Case3() =>
        await Arrange(() => 2)
            .AndFunctionApp<int, Startup, Function>()
            .ActAndExecute(
                x => x.FunctionApp,
                async (i, function) =>
                {
                    var result = await function.SomeFunctionTrigger(
                        Request.GET($"/some-path/{i.Data}".SetQueryParam("noBody")).AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Assert(resp =>
            {
                Assert.Equal(HttpStatusCode.OK, resp.Code);
                Assert.Empty(resp.Content ?? string.Empty);
            });

    [Fact(DisplayName = "Executing a http trigger function can fail without issue")]
    public async Task Case4() =>
        await Arrange(() => 2)
            .ActAndExecute(
                _ => FunctionAppBuilder.Create<Startup, Function>(),
                async (i, function) =>
                {
                    var result = await function.SomeFunctionTrigger(
                        Request.GET($"/some-path/{i}".SetQueryParam("fail")).AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Assert(resp => Assert.Equal(HttpStatusCode.InternalServerError, resp.Code));

    [Fact(
        DisplayName = "Executing a http trigger function can return a non http object result without issue"
    )]
    public async Task Case5() =>
        await Arrange(() => 2)
            .ActAndExecute(
                _ => FunctionAppBuilder.Create<Startup, Function>(),
                async (i, function) =>
                {
                    var result = await function.SomeFunctionTrigger(
                        Request.GET($"/some-path/{i}".SetQueryParam("empty")).AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Assert(resp => Assert.Equal(HttpStatusCode.InternalServerError, resp.Code));
}
