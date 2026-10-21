using System.Collections.Immutable;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day22;

public partial class Day22(bool isTest = false) : BaseDay("22", isTest)
{
    readonly struct GridFile(int x, int y, int size, int used, int avail, int usePer)
    {
        readonly public int X = x;
        readonly public int Y = y;
        readonly public int Size = size;
        readonly public int Used = used;
        readonly public int Avail = avail;
        readonly public int UsePer = usePer;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        var lines = File.ReadAllLines(InputPath);
        var builder = ImmutableArray.CreateBuilder<GridFile>(lines.Length - 2);

        foreach (var line in lines[2..])
        {
            int[] values = [.. NumbersRegex().Matches(line).Select(m => int.Parse(m.Value))];
            builder.Add(new(values[0], values[1], values[2], values[3], values[4], values[5]));
        }

        ImmutableArray<GridFile> files = builder.MoveToImmutable();

        int pairs = 0;
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Used == 0)
                continue;

            for (int j = 0; j < files.Length; j++)
            {
                if (i != j && files[i].Used <= files[j].Avail)
                    pairs++;
            }
        }

        int maxavail = files.Max(f => f.Avail);
        int used = files.Where(f => f.Size > 0).Min(f => f.Size);

        Console.WriteLine($"Max Avail: {maxavail} - used: {used}");

        return pairs;
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
    private static partial Regex NumbersRegex();
    #endregion
}
