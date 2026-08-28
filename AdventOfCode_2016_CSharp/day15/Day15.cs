using System.Data.Common;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day15;

public partial class Day15(bool isTest = false) : BaseDay("15", isTest)
{
    struct Disc
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public int Position { get; set; }
        public int Period { get; set; }

        public readonly bool Aligned(int second) => (Position + second + Id) % Size == 0;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        Disc[] discs = [.. File.ReadAllLines(InputPath)
            .Select(line => {
                int[] values = [.. NumbersRegex().Matches(line).Select(v => int.Parse(v.Value))];
                return new Disc()
                {
                    Id = values[0],
                    Size = values[1],
                    Position = values[3]
                };
            })];

        int time = 0;
        while (!discs.All(d => d.Aligned(time))) time++;

        return time;
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
        List<Disc> discs = [.. File.ReadAllLines(InputPath)
            .Select(line => {
                int[] values = [.. NumbersRegex().Matches(line).Select(v => int.Parse(v.Value))];
                return new Disc()
                {
                    Id = values[0],
                    Size = values[1],
                    Position = values[3]
                };
            })];
        discs.Add(new Disc()
        {
            Id = discs.Count + 1,
            Size = 11,
            Position = 0
        });

        int time = 0;
        while (!discs.All(d => d.Aligned(time))) time++;

        return time;
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
