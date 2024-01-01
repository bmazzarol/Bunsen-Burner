using BunsenBurner.Utility;

namespace BunsenBurner.Tests;

using static Aaa;

public static class OnceTests
{
    [Fact(DisplayName = "Once of T on ever runs once")]
    public static async Task Case1() =>
        await Arrange(Once.New(() => DateTime.Now))
            .Act(async o =>
            {
                var d1 = await o;
                var d2 = await o;
                var d3 = await o;
                return (d1, d2, d3);
            })
            .Assert(r => r.d1 == r.d2 && r.d1 == r.d3);

    [Fact(DisplayName = "Once of T on ever runs once for an async function")]
    public static async Task Case2() =>
        await Arrange(
                Once.New(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(10));
                    return DateTime.Now;
                })
            )
            .Act(async o =>
            {
                var d1 = await o;
                var d2 = await o;
                var d3 = await o;
                return (d1, d2, d3);
            })
            .Assert(r => r.d1 == r.d2 && r.d1 == r.d3);

    [Fact(DisplayName = "Once of T can be accessed in parallel without issue")]
    public static async Task Case3() =>
        await Arrange(
                Once.New(async () =>
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(10));
                    return DateTime.Now;
                })
            )
            .Act(async o =>
            {
                var result = Enumerable.Range(1, 1000).Select(async _ => await o);
                return await Task.WhenAll(result);
            })
            .Assert(r => r.Distinct().Count() == 1);

    [Fact(DisplayName = "Once's can be mapped")]
    public static async Task Case4() =>
        await Arrange(Once.New(() => DateTime.Now))
            .Act(async o => await o.Select(x => x.ToFileTime()))
            .Assert(r => r != 0);

    [Fact(DisplayName = "Once's can be combined")]
    public static async Task Case5() =>
        await Arrange(
                from o1 in Once.New(() => DateTime.Now)
                from o2 in Once.New(() => DateTime.Now)
                from o3 in Once.New(() => DateTime.Now)
                select (o1, o2, o3)
            )
            .Act(async o =>
            {
                var result = Enumerable.Range(1, 1000).Select(async _ => await o);
                return await Task.WhenAll(result);
            })
            .Assert(r => r.Distinct().Count() == 1);

    [Fact(DisplayName = "Once's can be combined once")]
    public static async Task Case6() =>
        await Arrange(Once.New(() => DateTime.Now).SelectMany(x => Once.New(() => (x, 1))))
            .Act(async o =>
            {
                var result = Enumerable.Range(1, 1000).Select(async _ => await o);
                return await Task.WhenAll(result);
            })
            .Assert(r => r.Distinct().Count() == 1);
}
