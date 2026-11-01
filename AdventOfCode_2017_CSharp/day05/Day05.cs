using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day05;

public class Day05(bool isTest = false) : BaseDay("05", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int idx = 0;
        int counter = 0;
        int[] offsets = [.. File.ReadAllLines(InputPath).Select(v => int.Parse(v))];
        while (idx >= 0 && idx < offsets.Length)
        {
            counter++;
            idx += offsets[idx]++;
        }
        return counter;
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
        int idx = 0;
        int counter = 0;
        int[] offsets = [.. File.ReadAllLines(InputPath).Select(v => int.Parse(v))];
        int offoffset = 1;
        while (idx >= 0 && idx < offsets.Length)
        {
            counter++;
            offoffset = 1;

            int increment = offsets[idx];
            if (increment >= 3)
                offoffset = -1;
            offsets[idx] += offoffset;

            idx += increment;
        }
        return counter;
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
