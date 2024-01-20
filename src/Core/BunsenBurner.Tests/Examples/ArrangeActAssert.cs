using System.Text;
using System.Text.Json;
using static BunsenBurner.Aaa;

namespace BunsenBurner.Tests.Examples;

public class ArrangeActAssert
{
    #region Example1

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTest() =>
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
            // split assertions into parts
            .Assert(result => !string.IsNullOrWhiteSpace(result))
            // And can only follow after at least one Assert
            .And(result => result.Contains("Widget1"))
            // can have as many And calls as required
            .And(result => result.Contains("12.5"));

    #endregion

    #region Example2

    private static Task<string> LoadWidgetNameFromDatabase()
    {
        return Task.FromResult("Widget1");
    }

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTest2() =>
        await Arrange(async () =>
            {
                // async is supported
                var widgetNameFromDatabase = await LoadWidgetNameFromDatabase();
                return new { Name = widgetNameFromDatabase, Cost = 12.50 };
            })
            // additional And stages are also supported after at least one Arrange call
            .And(widget =>
            {
                var ms = new MemoryStream();
                return (widget, ms);
            })
            .Act(async data =>
            {
                await JsonSerializer.SerializeAsync(data.ms, data.widget);
                return Encoding.UTF8.GetString(data.ms.ToArray());
            })
            .Assert(result => !string.IsNullOrWhiteSpace(result));

    #endregion

    #region Example3

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTest3() =>
        await
        // existing objects can also be lifted into the DSL
        new { Name = "Widget1", Cost = 12.50 }
            // using the extension method
            .ArrangeData()
            .And(widget =>
            {
                var ms = new MemoryStream();
                return (widget, ms);
            })
            .Act(async data =>
            {
                await JsonSerializer.SerializeAsync(data.ms, data.widget);
                return Encoding.UTF8.GetString(data.ms.ToArray());
            })
            .Assert(result => !string.IsNullOrWhiteSpace(result));

    #endregion

    #region Example4

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTest4() =>
        await new { Name = "Widget1", Cost = 12.50 }
            .ArrangeData()
            // supports async
            .Act(async widget =>
            {
                var ms = new MemoryStream();
                await JsonSerializer.SerializeAsync(ms, widget);
                return ms;
            })
            // and sync with additional act steps
            .And((_, ms) => Encoding.UTF8.GetString(ms.ToArray()))
            .Assert(result => !string.IsNullOrWhiteSpace(result));

    #endregion

    private static async Task<string> SerializeAsync<T>(T widget)
    {
        var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(ms, widget);
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    #region Example5

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTest5() =>
        await new { Name = "Widget1", Cost = 12.50 }
            .ArrangeData()
            .Act(SerializeAsync)
            // standard action where any assertions can be made
            .Assert(Assert.NotEmpty)
            // many and steps are supported
            .And(
                json =>
                    Assert.Contains(
                        expectedSubstring: "Widget1",
                        actualString: json,
                        StringComparison.Ordinal
                    )
            )
            .And(
                json =>
                    Assert.Contains(
                        expectedSubstring: "12.5",
                        actualString: json,
                        StringComparison.Ordinal
                    )
            );

    #endregion

    #region Example6

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTest6() =>
        await new { Name = "Widget1", Cost = 12.50 }
            .ArrangeData()
            .Act(SerializeAsync)
            // any expression can be used to form the assertion
            .Assert(json => !string.IsNullOrWhiteSpace(json))
            .And(json => json.Contains("Widget1"))
            .And(json => json.Contains("12.5"));

    #endregion

    #region Example7

    [Fact(DisplayName = "Divide by 0 fails")]
    public async Task ExampleTest7() =>
        await (dividend: 3, divisor: 0)
            .ArrangeData()
            .Act(parts => parts.dividend / parts.divisor)
            // errors can be assert against as well
            .AssertFailsWith(
                // if you provide a type hint it reduces down to just that exception, otherwise its just `Exception`
                (DivideByZeroException e) => e.Message == "Attempted to divide by zero."
            );

    #endregion
}
