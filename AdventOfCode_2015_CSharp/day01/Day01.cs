using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day01;

public class Day01(bool isTest = false) : BaseDay("01", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int result = 0;
        foreach(char c in Content) 
        {
            result += c == '(' ? 1 : -1;
        }
        return result;
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
        int result = 0;
        foreach(var item in Content.Select((v, i) => new {v, i }))
        {
            result += item.v == '(' ? 1 : -1;
            if (result < 0)
                return item.i+1;
        }
        return result;
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
