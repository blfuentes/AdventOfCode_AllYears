using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day06;

public partial class Day06(bool isTest = false) : BaseDay("06", isTest)
{
    private int[] memoryBlocks = [];
    (int value, int idx) FindMaxValueIndex()
    {
        (int value, int idx) = (int.MinValue, -1);
        for (int i = 0; i < memoryBlocks.Length; i++)
        {
            if (memoryBlocks[i] > value)
            {
                (value, idx) = (memoryBlocks[i], i);
            }
        }
        return (value, idx);
    }

    void Cycle()
    {
        (int value, int idx) = FindMaxValueIndex();
        memoryBlocks[idx] = 0;
        int rounds = value / memoryBlocks.Length;
        int remaining = value % memoryBlocks.Length;
        if (rounds > 0)
        {
            for (int dx = 0; dx < memoryBlocks.Length; dx++)
            {
                memoryBlocks[(idx + 1 + dx) % memoryBlocks.Length] += rounds;
            }
        }
        while (remaining > 0)
        {
            idx++;
            memoryBlocks[idx % memoryBlocks.Length]++;
            remaining--;
        }
    }
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        memoryBlocks = [.. NumbersRegex().Matches(Content).Select(m => int.Parse(m.Value))];
        HashSet<string> seen = [];
        string key = String.Join("-", memoryBlocks.Select(m => $"{m}"));
        int count = 0;
        while (!seen.Contains(key))
        {
            seen.Add(key);
            Cycle();
            key = String.Join("-", memoryBlocks.Select(m => $"{m}"));
            count++;
        }

        return count;
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
        memoryBlocks = [.. NumbersRegex().Matches(Content).Select(m => int.Parse(m.Value))];
        HashSet<string> seen = [];

        string key = String.Join("-", memoryBlocks.Select(m => $"{m}"));
        int count = 0;
        int times = 0;
        while (times < 2)
        {
            seen.Add(key);
            Cycle();
            key = String.Join("-", memoryBlocks.Select(m => $"{m}"));
            count++;
            if (seen.Contains(key))
            {
                if (times < 1)
                {
                    count = 0;
                    seen.Clear();
                }
                times++;
            }
        }

        return count;
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
