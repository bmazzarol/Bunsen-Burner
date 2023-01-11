namespace BunsenBurner.FunctionApp.Tests;

using static BunsenBurner.Bdd;
using static Bdd;

public class BddTests
{
    [Fact(DisplayName = "Executing a http trigger function works")]
    public async Task Case1() =>
        await GivenFunctionApp<Startup, Function>()
            .WhenExecuted(async function =>
            {
                var result = await function.SomeFunctionTrigger(
                    await Req.Get.To("/some-path").AsHttpRequest()
                );
                return result.AsResponse();
            })
            .Then(resp => Assert.Equal(HttpStatusCode.OK, resp.StatusCode));

    [Fact(DisplayName = "Executing a http trigger function works with description")]
    public async Task Case2() =>
        await "Some description"
            .GivenFunctionApp<Startup, Function>()
            .WhenExecuted(async function =>
            {
                var result = await function.SomeFunctionTrigger(
                    await Req.Get.To("/some-path").AsHttpRequest()
                );
                return result.AsResponse();
            })
            .Then(resp => Assert.Equal(HttpStatusCode.OK, resp.StatusCode));

    [Fact(DisplayName = "Executing a http trigger function works")]
    public async Task Case3() =>
        await Given(() => 1)
            .AndFunctionApp<int, Startup, Function>()
            .WhenExecuted(
                x => x.FunctionApp,
                async (i, function) =>
                {
                    var result = await function.SomeFunctionTrigger(
                        await Req.Get.To($"/some-path/{i.Data}").AsHttpRequest()
                    );
                    return result.AsResponse();
                }
            )
            .Then(resp => Assert.Equal(HttpStatusCode.OK, resp.StatusCode));
}
