using Microsoft.Extensions.DependencyInjection;

namespace BunsenBurner.Http.Tests;

public static class TestServerBuilderOptionsTests
{
    [Fact(DisplayName = "Tests for the build methods")]
    public static async Task Case1() =>
        await TestServerBuilderOptions
            .New()
            .WithName("Test")
            .WithIssuer("http://test.com")
            .WithEnvironmentName("localhost")
            .WithSigningKey("test")
            .WithSetting("a", "1")
            .WithStartup<Startup>()
            .WithStartup(typeof(Startup))
            .WithConfig(c => c.Properties.Add("b", 2))
            .WithServices(collection => collection.AddSingleton(string.Empty))
            .WithHost(_ => { })
            .ArrangeData()
            .Act(x =>
            {
                x.Build();
                return x;
            })
            .Assert(
                r =>
                    r.Name == "Test"
                    && r.Issuer == "http://test.com"
                    && r.EnvironmentName == "localhost"
                    && r.SigningKey == "test"
                    && r.AppSettingsToOverride!.ContainsKey("a")
            );
}
