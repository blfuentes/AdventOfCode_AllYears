using BenchmarkDotNet.Attributes;
using System.Linq;
using System.Collections.Concurrent;

namespace AdventOfCode_2015_CSharp.day02;

public class Day02(bool isTest = false) : BaseDay("02", isTest)
{
    static (int, int) SquareFeet(int l, int w, int h)
    {
        return ((new int[] { l * w, w * h, h * l }).Min(), 2 * l * w + 2 * w * h + 2 * h * l);
    }

    static (int, int) RibbonFeet(int l, int w, int h)
    {
        static int Perimeter(int a, int b)
        {
            return 2 * a + 2 * b;
        }
        return ((l * w * h, (new int[] { Perimeter(l, w), Perimeter(w, h), Perimeter(h, l) }).Min()));
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        ConcurrentBag<int> results = [];
        var contentParts = File.ReadAllLines(InputPath);
        Parallel.ForEach(contentParts, (present) =>
        {
            var (b, v) = SquareFeet(
                int.Parse(present.Split('x')[0]),
                int.Parse(present.Split('x')[1]),
                int.Parse(present.Split('x')[2]));
            results.Add(b+v);
        });

        return results.Sum();
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
        ConcurrentBag<int> results = [];
        var contentParts = File.ReadAllLines(InputPath);
        Parallel.ForEach(contentParts, (present) =>
        {
            var (b, v) = RibbonFeet(
                int.Parse(present.Split('x')[0]),
                int.Parse(present.Split('x')[1]),
                int.Parse(present.Split('x')[2]));
            results.Add(b + v);
        });

        return results.Sum();
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
