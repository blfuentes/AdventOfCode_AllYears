using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day11;

public class Day11(bool isTest = false) : BaseDay("11", isTest)
{
    private readonly Dictionary<string, (int x, int y, int z)> _offsets = new()
    {
        { "n", (0, 1, -1)},
        { "ne", (1, 0, -1)},
        { "nw", (-1, 1, 0)},
        { "s", (0, -1, 1)},
        { "se", (1, -1, 0)},
        { "sw", (-1, 0, 1)}
    };

    private static int Distance((int x, int y, int z) a, (int x, int y, int z) b)
    {
        return (Math.Abs(a.x - b.x) +
                Math.Abs(a.y - b.y) +
                Math.Abs((a.x + a.y) - (b.x + b.y)))
                / 2;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        (int x, int y, int z) = Content.Split(",")
            .Select(m => _offsets[m])
            .Aggregate((0, 0, 0), ((int x, int y, int z) a, (int x, int y, int z) b)
                => (a.x + b.x, a.y + b.y, a.z + b.z));

        return Distance((0, 0, 0), (x, y, z));
    }

    public override string SolvePart1()
    {
        StopWatch.Start();
        var result = RunPart1();
        StopWatch.Stop();
        return $"Final result Day {Day} part 1: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion

    #region Part 2
    [Benchmark]
    public int RunPart2()
    {
        int maxDistance = 0;
        (int x, int y, int z) = Content.Split(",")
            .Select(m => _offsets[m])
            .Aggregate((0, 0, 0), ((int x, int y, int z) a, (int x, int y, int z) b)
                =>
                {
                    (int x, int y, int z) = (a.x + b.x, a.y + b.y, a.z + b.z);
                    int distance = Distance((0, 0, 0), (x, y, z));
                    maxDistance = Math.Max(maxDistance, distance);
                    return (x, y, z);
                });
        return maxDistance;
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion
}
