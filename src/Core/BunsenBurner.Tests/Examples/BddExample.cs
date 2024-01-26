using System.Text;
using System.Text.Json;

namespace BunsenBurner.Tests.Examples;

#region Example1

using static BunsenBurner.BddSyntax;

public class BddExample
{
    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public async Task ExampleTestUsingBddBuilder() =>
        await Given(
                () =>
                    (
                        Widget: new { Name = "Widget1", Cost = 12.50 },
                        MemoryStream: new MemoryStream()
                    )
            )
            .When(async data =>
            {
                await JsonSerializer.SerializeAsync(data.MemoryStream, data.Widget);
                return Encoding.UTF8.GetString(data.MemoryStream.ToArray());
            })
            .Then(result => result == "{\"Name\":\"Widget1\",\"Cost\":12.5}");
}

#endregion
