using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;

namespace AdventOfCode_2016_CSharp.day06;

public class Day06(bool isTest = false) : BaseDay("06", isTest)
{
    #region Part 1
    [Benchmark]
    public string RunPart1()
    {
        string[] messages = File.ReadAllLines(InputPath);
        char[] message = new char[messages.First().Length];

        for (int c = 0; c < message.Length; c++)
        {
            message[c] = messages
                .Select(m => m[c])
                .CountBy(m => m)
                .OrderByDescending(v => v.Value)
                .First()
                .Key;
                
        }
        return new string(message);
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
    public string RunPart2()
    {
        string[] messages = File.ReadAllLines(InputPath);
        char[] message = new char[messages.First().Length];

        for (int c = 0; c < message.Length; c++)
        {
            message[c] = messages
                .Select(m => m[c])
                .CountBy(m => m)
                .OrderBy(v => v.Value)
                .First()
                .Key;

        }
        return new string(message);
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
