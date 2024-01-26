using System.Text;
using System.Text.Json;
using static BunsenBurner.AaaSyntax;

namespace BunsenBurner.Tests.Examples;

public class GettingStarted
{
    #region Example1a

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTest()
    {
        // Arrange
        var widget = new { Name = "Widget1", Cost = 12.50 };
        var ms = new MemoryStream();

        // Act
        await JsonSerializer.SerializeAsync(ms, widget);

        // Assert
        Assert.Equal(
            expected: "{\"Name\":\"Widget1\",\"Cost\":12.5}",
            actual: Encoding.UTF8.GetString(ms.ToArray())
        );
    }

    #endregion

    #region Example1b

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTestUsingAaaBuilder() =>
        await Arrange(() =>
            {
                var widget = new { Name = "Widget1", Cost = 12.50 };
                var ms = new MemoryStream();
                return (widget, ms);
            })
            .Act(async data =>
            {
                await JsonSerializer.SerializeAsync(data.ms, data.widget);
                return Encoding.UTF8.GetString(data.ms.ToArray());
            })
            .Assert(
                result =>
                    Assert.Equal(expected: "{\"Name\":\"Widget1\",\"Cost\":12.5}", actual: result)
            );

    #endregion

    #region Example1c

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTestUsingAaaBuilder2() =>
        await Arrange(() =>
            {
                var widget = new { Name = "Widget1", Cost = 12.50 };
                var ms = new MemoryStream();
                return (widget, ms);
            })
            // pull out the shared test code
            .Act(CallSerializeAsync)
            .Assert(
                result =>
                    Assert.Equal(expected: "{\"Name\":\"Widget1\",\"Cost\":12.5}", actual: result)
            );

    // this is now a shared stage and can be used in many tests
    private static async Task<string> CallSerializeAsync<T>((T, MemoryStream) data)
    {
        var (widget, ms) = data;
        await JsonSerializer.SerializeAsync(ms, widget);
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    #endregion
}
