using System.Collections.Immutable;
using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;

namespace AdventOfCode_2016_CSharp.day20;

public class Day20(bool isTest = false) : BaseDay("20", isTest)
{
    const uint MaxValue = 4294967295;

    #region Part 1
    readonly struct BlockedRange(uint start, uint end)
    {
        public readonly uint Start = start;
        public readonly uint End = end;
    }
    [Benchmark]
    public long RunPart1()
    {
        var lines = File.ReadAllLines(InputPath);
        var builder = ImmutableArray.CreateBuilder<BlockedRange>(lines.Length);

        foreach (var line in lines)
        {
            int dash = line.IndexOf('-');
            ReadOnlySpan<char> s = line;
            builder.Add(new(uint.Parse(s[..dash]), uint.Parse(s[(dash + 1)..])));
        }
        builder.Sort(static (a, b) => a.Start.CompareTo(b.Start));

        ImmutableArray<BlockedRange> ranges = builder.MoveToImmutable();
        uint min = 0;

        foreach (var r in ranges)
        {
            if (r.Start > min)
                break;
            min = Math.Max(min, r.End + 1);
        }

        return min;
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
    public ulong RunPart2()
    {
        var lines = File.ReadAllLines(InputPath);
        var builder = ImmutableArray.CreateBuilder<BlockedRange>(lines.Length);

        foreach (var line in lines)
        {
            int dash = line.IndexOf('-');
            ReadOnlySpan<char> s = line;
            builder.Add(new(uint.Parse(s[..dash]), uint.Parse(s[(dash + 1)..])));
        }
        builder.Sort(static (a, b) => a.Start.CompareTo(b.Start));

        ImmutableArray<BlockedRange> ranges = builder.MoveToImmutable();
        ulong min = 0, free = 0;

        foreach (var r in ranges)
        {
            if (r.Start > min)
                free += r.Start - min;

            min = Math.Max(min, (ulong)r.End + 1);
        }

        if (min <= MaxValue)
            free += (ulong)MaxValue - min + 1;

        return free;
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
