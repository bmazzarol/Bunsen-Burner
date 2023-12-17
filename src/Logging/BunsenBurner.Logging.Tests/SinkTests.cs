using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace BunsenBurner.Logging.Tests;

public sealed class SinkTests
{
    private readonly ITestOutputHelper _outputHelper;

    public SinkTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

    [Fact(DisplayName = "Sink can be used to interface Dummy logger to output helpers")]
    public async Task Case1() =>
        await DummyLogger
            .New<SinkTests>(sink: Sink.New(_outputHelper.WriteLine))
            .ArrangeData()
            .Act(logger =>
            {
                logger.LogInformation("Some test message");
                return true;
            })
            .Assert(_ => _);
}
