using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.AutomatedAnalysis;
using System.Drawing;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode_2016_CSharp.day09;

public partial class Day09(bool isTest = false) : BaseDay("09", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int lengthSoFar = 0;
        int idx = 0;
        while (idx < Content.Length)
        {
            var match = MarkersRegEx().Match(Content[idx..]);
            if (match.Success)
            {
                (int length, int times) = (int.Parse(match.Value.Split("x")[0][1..]), int.Parse(match.Value.Split("x")[1].Replace(")", "")));
                lengthSoFar += match.Index + times * length;
                idx += match.Index + match.Length + length;
            }
            else
            {
                lengthSoFar += Content[idx..].Length;
                idx += lengthSoFar;
            }
        }
        if (lengthSoFar == 0) return Content.Length;
        else return lengthSoFar;
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
    public long RunPart2()
    {
        var toTest = File.ReadAllLines(InputPath);
        static long GetLength(long currentLength , string input)
        {
            var match = MarkersRegEx().Match(input);
            if (match.Success)
            {
                var endIdx = match.Index + match.Value.Length;
                (int size, long times) = (int.Parse(match.Groups["size"].Value), long.Parse(match.Groups["times"].Value));
                return currentLength +
                        (long)match.Index +
                        times * GetLength(0, input.Substring(endIdx, size)) +
                        GetLength(0, input[(endIdx + size)..]);
            }
            return (long)input.Length;
        }
        return GetLength(0, Content);
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }

    [GeneratedRegex(@"\((?<size>\d+)x(?<times>\d+)\)")]
    private static partial Regex MarkersRegEx();
    #endregion
}
