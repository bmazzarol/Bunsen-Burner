using System.Net;
using BunsenBurner.Http;

namespace BunsenBurner.FunctionApp.Tests;
using static BunsenBurner.Bdd;

public class BddTests
{
    [Fact(DisplayName = "Executing a http trigger function works")]
    public async Task Case1() =>
        await Given(() => 2)
            .WhenExecuted(
                FunctionAppBuilder.Create<Startup, Function>(),
                async (i, function) =>
                {
                    var result = await function.SomeFunctionTrigger(
                        Request.GET($"/some-path/{i}").AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Then(resp => Assert.Equal(HttpStatusCode.OK, resp.Code));
}
