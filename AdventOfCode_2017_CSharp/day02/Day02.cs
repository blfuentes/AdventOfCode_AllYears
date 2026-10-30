using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode_2017_CSharp.day02;

public partial class Day02 : BaseDay
{
    private readonly string[] _lines;
    public Day02(bool isTest) : base("02", isTest) =>
        _lines = File.ReadAllLines(InputPath);

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int checksum = 0;
        foreach (string line in _lines)
        {
            int[] numbers = [.. NumbersRegex().Matches(line).Select(n => int.Parse(n.Value))];
            (int min, int max) = (int.MaxValue, int.MinValue);
            foreach (int num in numbers)
            {
                if (num < min)
                    min = num;
                if (num > max)
                    max = num;
            }
            checksum += max - min;
        }
        return checksum;
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
        int checksum = 0;
        foreach (string line in _lines)
        {
            int[] numbers = [.. NumbersRegex().Matches(line).Select(n => int.Parse(n.Value))];
            int res = 0;
            bool exit = false;
            foreach (int dividendo in numbers)
            {
                if (!exit)
                {
                    foreach (int divisor in numbers)
                    {
                        if (dividendo > divisor && dividendo != divisor)
                        {
                            res = dividendo % divisor;
                            if (res == 0)
                            {
                                res = dividendo / divisor;
                                exit = true;
                                break;
                            }
                        }
                    }
                }
                if (exit) break;
            }
            checksum += res;
        }
        return checksum;
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumbersRegex();
    #endregion
}
