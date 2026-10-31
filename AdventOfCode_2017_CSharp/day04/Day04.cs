using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day04;

public class Day04(bool isTest = false) : BaseDay("04", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        var lines = File.ReadAllLines(InputPath);
        return lines.Count(line =>
        {
            return line.Split(" ").CountBy(w => w).All(c => c.Value == 1);
        });
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
        var lines = File.ReadAllLines(InputPath);
        return lines.Count(line =>
        {
            return line.Split(" ")
                .Select(w => String.Join("", w.ToArray().Order()))
                .CountBy(w => w).All(c => c.Value == 1);
        });
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
