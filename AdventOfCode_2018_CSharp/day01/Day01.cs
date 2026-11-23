using System.Windows.Markup;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2018_CSharp.day01;

public class Day01(bool isTest = false) : BaseDay("01", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        return Content.Split(System.Environment.NewLine).Sum(v => int.Parse(v));
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
        HashSet<int> visited = [];
        int current = 0;
        int idx = 0;
        string[] values = Content.Split(Environment.NewLine);
        while (!visited.Contains(current))
        {
            visited.Add(current);
            current += int.Parse(values[idx++ % values.Length]);
        }

        return current;
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
