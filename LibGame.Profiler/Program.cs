using System.Security.Cryptography;
using System.Xml;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using LibGame.Noise;

namespace LibGame.Profiler;

public static class Program
{
    [MemoryDiagnoser]
    public class TestFramework
    {
        [Benchmark] // 12.79ns
        public float ComputeNoise()
        {
            const float x = 3329.0f;
            const float y = 7907.0f;
            return SimplexNoise.Noise(x, y);
        }

    }


    public static void Main(string[] _)
    {
        BenchmarkRunner.Run<TestFramework>();
    }
}
