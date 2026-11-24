using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;

namespace AdventOfCode_2018_CSharp.day03;

public partial class Day03 : BaseDay
{
    public struct Claim(int _id, (int c, int r) _region, (int w, int t) _area)
    {
        public int Id = _id;
        public (int Col, int Row) Region = _region;
        public (int Width, int Tall) Area = _area;

        public readonly bool ContainsPoint((int c, int r) Point)
        {
            return
                Point.c >= Region.Col &&
                Point.c <= Region.Col + Area.Width &&
                Point.r >= Region.Row &&
                Point.r <= Region.Row + Area.Tall;

        }
    }

    public readonly Claim[] _claims;

    public Day03(bool isTest = false) : base("03", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        _claims = new Claim[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            int[] numbers = [.. NumbersRegex().Matches(lines[i]).Select(m => int.Parse(m.Value))];
            _claims[i] = new Claim(numbers[0], (numbers[1], numbers[2]), (numbers[3], numbers[4]));
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        Dictionary<(int, int), int> points = [];
        foreach (var claim in _claims)
        {
            for (int w = 0; w < claim.Area.Width; w++)
            {
                for (int t = 0; t < claim.Area.Tall; t++)
                {
                    (int, int) p = (claim.Region.Col + w, claim.Region.Row + t);
                    if (!points.TryAdd(p, 1))
                        points[p]++;
                }
            }
        }
        return points.Values.Count(p => p > 1);
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
        Dictionary<(int, int), int[]> points = [];
        Dictionary<int, HashSet<int>> overlaps = [];
        foreach (var claim in _claims)
        {
            overlaps.Add(claim.Id, []);
            for (int w = 0; w < claim.Area.Width; w++)
            {
                for (int t = 0; t < claim.Area.Tall; t++)
                {
                    (int, int) p = (claim.Region.Col + w, claim.Region.Row + t);
                    if (!points.TryAdd(p, [claim.Id]))
                    {
                        foreach (var o in points[p])
                        {
                            overlaps[o].Add(claim.Id);
                            overlaps[claim.Id].Add(o);
                        }
                    }
                }
            }
        }

        return overlaps.Single(o => o.Value.Count == 0).Key;
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumbersRegex();
}
