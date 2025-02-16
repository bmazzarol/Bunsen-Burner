using System.Text;
using System.Text.Json;
#region ArrangeActAssertUsing

// for AAA style tests
using static BunsenBurner.ArrangeActAssert;

#endregion

namespace BunsenBurner.Tests.Examples;

public class ArrangeActAssert
{
    #region Example1

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public Task ExampleTest() =>
        Arrange(() =>
            {
                var widget = new { Name = "Widget1", Cost = 12.50 };
                var ms = new MemoryStream();
                return (widget, ms);
            })
            .Act(async data =>
            {
                await JsonSerializer.SerializeAsync(
                    data.ms,
                    data.widget,
                    cancellationToken: TestContext.Current.CancellationToken
                );
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
    public Task ExampleTest2() =>
        Arrange(async () =>
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
                await JsonSerializer.SerializeAsync(
                    data.ms,
                    data.widget,
                    cancellationToken: TestContext.Current.CancellationToken
                );
                return Encoding.UTF8.GetString(data.ms.ToArray());
            })
            .Assert(result => !string.IsNullOrWhiteSpace(result));

    #endregion

    #region Example3

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public Task ExampleTest3() =>
        // existing objects can also be lifted into the DSL
        new { Name = "Widget1", Cost = 12.50 }
            // using the extension method
            .Arrange()
            .And(widget =>
            {
                var ms = new MemoryStream();
                return (widget, ms);
            })
            .Act(async data =>
            {
                await JsonSerializer.SerializeAsync(
                    data.ms,
                    data.widget,
                    cancellationToken: TestContext.Current.CancellationToken
                );
                return Encoding.UTF8.GetString(data.ms.ToArray());
            })
            .Assert(result => !string.IsNullOrWhiteSpace(result));

    #endregion

    #region Example4

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public Task ExampleTest4() =>
        new { Name = "Widget1", Cost = 12.50 }
            .Arrange()
            // supports async
            .Act(async widget =>
            {
                var ms = new MemoryStream();
                await JsonSerializer.SerializeAsync(
                    ms,
                    widget,
                    cancellationToken: TestContext.Current.CancellationToken
                );
                return ms;
            })
            // and sync with additional act steps
            .And((_, ms) => Encoding.UTF8.GetString(ms.ToArray()))
            .Assert(result => !string.IsNullOrWhiteSpace(result));

    #endregion

    private static async Task<string> SerializeAsync<T>(T widget)
    {
        var ms = new MemoryStream();
        await JsonSerializer.SerializeAsync(
            ms,
            widget,
            cancellationToken: TestContext.Current.CancellationToken
        );
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    #region Example5

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public Task ExampleTest5() =>
        new { Name = "Widget1", Cost = 12.50 }
            .Arrange()
            .Act(SerializeAsync)
            // standard action where any assertions can be made
            .Assert(Assert.NotEmpty)
            // many and steps are supported
            .And(json =>
                Assert.Contains(
                    expectedSubstring: "Widget1",
                    actualString: json,
                    StringComparison.Ordinal
                )
            )
            .And(json =>
                Assert.Contains(
                    expectedSubstring: "12.5",
                    actualString: json,
                    StringComparison.Ordinal
                )
            );

    #endregion

    #region Example6

    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public Task ExampleTest6() =>
        new { Name = "Widget1", Cost = 12.50 }
            .Arrange()
            .Act(SerializeAsync)
            // any expression can be used to form the assertion
            .Assert(json => !string.IsNullOrWhiteSpace(json))
            .And(json => json.Contains("Widget1"))
            .And(json => json.Contains("12.5"));

    #endregion

    #region Example7

    [Fact(DisplayName = "Divide by 0 fails")]
    public Task ExampleTest7() =>
        (dividend: 3, divisor: 0)
            .Arrange()
            .Act(parts => parts.dividend / parts.divisor)
            // errors can be asserted against as well
            .Throw<DivideByZeroException>()
            .Assert(e => e.Message == "Attempted to divide by zero.");

    #endregion
}
