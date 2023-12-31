using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace BunsenBurner.BenchmarkDotNet.Tests;

public class TestBenchmarks
{
    private const int N = 100;
    private readonly byte[] _data;

    private readonly SHA256 _sha256 = SHA256.Create();

    [SuppressMessage("Security", "CA5351:Do Not Use Broken Cryptographic Algorithms")]
    private readonly MD5 _md5 = MD5.Create();

    public TestBenchmarks()
    {
        _data = new byte[N];
        new Random(42).NextBytes(_data);
    }

    [Benchmark]
    public byte[] Sha256() => _sha256.ComputeHash(_data);

    [Benchmark(Baseline = true)]
    public byte[] Md5() => _md5.ComputeHash(_data);
}
