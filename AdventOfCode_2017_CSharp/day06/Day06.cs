using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day06;

public partial class Day06(bool isTest = false) : BaseDay("06", isTest)
{
    private sealed class MemoryBlocksComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[]? x, int[]? y) =>
            ReferenceEquals(x, y) ||
            (x is not null && y is not null && x.AsSpan().SequenceEqual(y));

        public int GetHashCode(int[] values)
        {
            var hash = new HashCode();
            foreach (var value in values)
                hash.Add(value);
            return hash.ToHashCode();
        }
    }

    private static readonly MemoryBlocksComparer Comparer = new();

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
        HashSet<int[]> seen = new(Comparer);

        int count = 0;
        while (seen.Add((int[])memoryBlocks.Clone()))
        {
            Cycle();
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
        HashSet<int[]> seen = new(Comparer); ;

        int count = 0;
        int times = 0;
        while (times < 2)
        {
            seen.Add((int[])memoryBlocks.Clone());
            Cycle();
            count++;
            if (seen.Contains(memoryBlocks))
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
