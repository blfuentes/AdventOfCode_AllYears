using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2018_CSharp.day06;

public partial class Day06 : BaseDay
{
    struct Place
    {
        public char Id;
        public (int X, int Y) Pos;
    }

    private readonly Place[] _places;

    private readonly int minX;
    private readonly int maxX;
    private readonly int minY;
    private readonly int maxY;

    public Day06(bool isTest = false) : base("06", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        _places = new Place[lines.Length];

        (minX, maxX) = (int.MaxValue, int.MinValue);
        (minY, maxY) = (int.MaxValue, int.MinValue);

        for (int i = 0; i < lines.Length; i++)
        {
            int[] numbers = [.. NumbersRegex().Matches(lines[i]).Select(m => int.Parse(m.Value))];
            _places[i] = new Place()
            {
                Id = (char)('A' + i),
                Pos = (numbers[0], numbers[1])
            };
            (minX, maxX) = (Math.Min(minX, numbers[0]) - 1, Math.Max(maxX, numbers[0]) + 1);
            (minY, maxY) = (Math.Min(minY, numbers[1]) - 1, Math.Max(maxY, numbers[1]) + 1);
        }
    }

    static int ManhattanDistance((int x, int y) a, (int x, int y) b) =>
        Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        Dictionary<Place, HashSet<(int, int)>> posByPlace = [];
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                var minDistance =
                    _places
                    .GroupBy(p => ManhattanDistance(p.Pos, (x, y)))
                    .MinBy(g => g.Key);

                if (minDistance!.Count() == 1)
                {
                    if (!posByPlace.TryAdd(minDistance!.First(), [(x, y)]))
                    {
                        posByPlace[minDistance!.First()].Add((x, y));
                    }
                }
            }
        }
        var area = posByPlace
            .Where(p =>
                !p.Value.Any(c =>
                    c.Item1 == minX || c.Item1 == maxX ||
                    c.Item2 == minY || c.Item2 == maxY)
            )
            .Max(p => p.Value.Count);
        return area;
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
        Dictionary<(int, int), int> distancesByCoord = [];
        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                var sumOfDistances =
                    _places.Sum(p => ManhattanDistance(p.Pos, (x, y)));

                if (sumOfDistances < 10_000)
                    distancesByCoord.Add((x, y), sumOfDistances);
            }
        }
        return distancesByCoord.Count;
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
