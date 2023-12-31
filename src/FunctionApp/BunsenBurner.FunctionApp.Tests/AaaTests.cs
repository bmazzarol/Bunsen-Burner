using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.FunctionApp.Tests;

using static Aaa;
using static BunsenBurner.Aaa;

internal interface ISomeService
{
    Task<int> SomeMethod();
}

internal sealed class SomeService : ISomeService
{
    public Task<int> SomeMethod() => Task.FromResult(1);
}

internal sealed class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder) =>
        builder.Services.AddScoped<ISomeService, SomeService>();
}

internal interface ITestService
{
    int Result();
}

internal sealed class TestService(int result) : ITestService
{
    public int Result() => result;
}

internal sealed class Function(ISomeService service, IServiceProvider provider)
{
    [FunctionName(nameof(SomeFunctionTrigger))]
    public async Task<IActionResult> SomeFunctionTrigger(
        [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req
    )
    {
        var service1 = provider.GetService<ITestService>();

        if (service1 != null)
            return new OkObjectResult(service1.Result());
        if (req.Query.ContainsKey("empty"))
            return new EmptyResult();
        if (req.Query.ContainsKey("fail"))
            return new InternalServerErrorResult();
        if (req.Query.ContainsKey("noBody"))
            return new OkResult();
        return new OkObjectResult(await service.SomeMethod());
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
                    await Req.Get.To("/some-path").AsHttpRequest()
                );
                return result.AsResponse();
            })
            .Assert(async resp =>
            {
                Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
                Assert.Equal("1", await resp.Content.ReadAsStringAsync());
            });

    [Fact(DisplayName = "Executing a http trigger function works with description")]
    public async Task Case2() =>
        await "Some description"
            .ArrangeFunctionApp<Startup, Function>()
            .ActAndExecute(async function =>
            {
                var result = await function.SomeFunctionTrigger(
                    await Req.Get.To("/some-path").AsHttpRequest()
                );
                return result.AsResponse();
            })
            .Assert(async resp =>
            {
                Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
                Assert.Equal("1", await resp.Content.ReadAsStringAsync());
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
                        await Req.Get
                            .To($"/some-path/{i.Data}".SetQueryParam("noBody"))
                            .AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Assert(async resp =>
            {
                Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
                Assert.Empty(await resp.Content.ReadAsStringAsync());
            });

    [Fact(DisplayName = "Executing a http trigger function can fail without issue")]
    public async Task Case4() =>
        await Arrange(() => 2)
            .ActAndExecute(
                _ => FunctionAppBuilder.CreateAndCache<Startup, Function>(),
                async (i, function) =>
                {
                    var result = await function.SomeFunctionTrigger(
                        await Req.Get.To($"/some-path/{i}".SetQueryParam("fail")).AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Assert(resp => Assert.Equal(HttpStatusCode.InternalServerError, resp.StatusCode));

    [Fact(
        DisplayName = "Executing a http trigger function can return a non http object result without issue"
    )]
    public async Task Case5() =>
        await Arrange(() => 2)
            .ActAndExecute(
                _ => FunctionAppBuilder.CreateAndCache<Startup, Function>(),
                async (i, function) =>
                {
                    var result = await function.SomeFunctionTrigger(
                        await Req.Get.To($"/some-path/{i}".SetQueryParam("empty")).AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Assert(resp => Assert.Equal(HttpStatusCode.InternalServerError, resp.StatusCode));

    [Fact(DisplayName = "Executing a http trigger function without caching it")]
    public async Task Case6() =>
        await Arrange(() => 99)
            .ActAndExecute(
                i =>
                    FunctionAppBuilder.Create<Startup, Function>(
                        collection => collection.AddSingleton<ITestService>(new TestService(i))
                    ),
                async (i, function) =>
                {
                    var result = await function.SomeFunctionTrigger(
                        await Req.Get.To($"/some-path/{i}".SetQueryParam("empty")).AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Assert(async resp =>
            {
                Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
                Assert.Equal("99", await resp.Content.ReadAsStringAsync());
            });
}
