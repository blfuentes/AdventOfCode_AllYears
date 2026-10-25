using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day01;

public class Day01(bool isTest = false) : BaseDay("01", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int counter = 0;
        int step = Content.Length / 2;
        for (int cIdx = 0; cIdx < Content.Length; cIdx++)
        {
            if (Content[cIdx] == Content[(cIdx + 1) % Content.Length])
                counter += Content[cIdx] - 48;
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
        int counter = 0;
        int step = Content.Length / 2;
        for (int cIdx = 0; cIdx < Content.Length; cIdx++)
        {
            if (Content[cIdx] == Content[(cIdx + step) % Content.Length])
                counter += Content[cIdx] - 48;
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
