using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace AdventOfCode_2015_CSharp.day25;

public partial class Day25(bool isTest = false) : BaseDay("25", isTest)
{
    #region Part 1
    [Benchmark]
    public long RunPart1()
    {
        var numbers = ExtractNumbers().Matches(Content);
        (int row, int col) = (int.Parse(numbers[0].Value),  int.Parse(numbers[1].Value));
        static long Next(long value) => value * 252533L % 33554393L;
        long GetValue((int, int) target, long value)
        {
            int initRow = 1, initCol = 1;
            while((initRow, initCol) != target)
            {
                if (initRow == 1)
                {
                    initRow = initCol + 1;
                    initCol = 1;
                } 
                else
                {
                    initRow -= 1;
                    initCol += 1;
                }
                value = Next(value);
            }

            return value;
        }

        return GetValue((row, col), 20151125L);
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
        return Content.Length;
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex ExtractNumbers();
    #endregion
}
