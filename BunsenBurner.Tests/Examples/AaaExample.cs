using System.Text;
using System.Text.Json;

namespace BunsenBurner.Tests.Examples;

#region Example1

using static BunsenBurner.ArrangeActAssert;

public class AaaExample
{
    [Fact(DisplayName = "SerializeAsync can work with anonymous objects")]
    public Task ExampleTestUsingAaaBuilder() =>
        Arrange(
                () =>
                    (
                        Widget: new { Name = "Widget1", Cost = 12.50 },
                        MemoryStream: new MemoryStream()
                    )
            )
            .Act(async data =>
            {
                await JsonSerializer.SerializeAsync(data.MemoryStream, data.Widget);
                return Encoding.UTF8.GetString(data.MemoryStream.ToArray());
            })
            .Assert(result => result == "{\"Name\":\"Widget1\",\"Cost\":12.5}");
}

#endregion
