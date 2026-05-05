using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace AdventOfCode_2016_CSharp.day03;

public partial class Day03(bool isTest = false) : BaseDay("03", isTest)
{
    static bool IsValidTriangle(int[] sides)
    {
        return sides.Length == 3 &&
            (
            sides[0] + sides[1] > sides[2] &&
            sides[0] + sides[2] > sides[1] &&
            sides[1] + sides[2] > sides[0]
            );
    }
    static int[] ExtractTriangle(string s)
    {
        return [.. regex.Matches(s).Select(m => int.Parse(m.Groups[0].Value))];
    }
    private static readonly Regex regex = ExtractNumbers();

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        return File.ReadAllLines(InputPath)
            .Select(ExtractTriangle)
            .Count(IsValidTriangle);
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
        var parts = File.ReadAllLines(InputPath)
            .Select(ExtractTriangle);
        
        return 
            parts.Select(p => p[0]).Chunk(3).Count(IsValidTriangle) +
            parts.Select(p => p[1]).Chunk(3).Count(IsValidTriangle) +
            parts.Select(p => p[2]).Chunk(3).Count(IsValidTriangle);
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }

    [GeneratedRegex("\\d+")]
    private static partial Regex ExtractNumbers();
    #endregion
}
