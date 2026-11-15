using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day20;

public partial class Day20 : BaseDay
{
    record struct Point(
        int Id,
        (double X, double Y, double Z) Pos,
        (double X, double Y, double Z) Speed,
        (double X, double Y, double Z) Acc);

    private readonly Point[] _points;

    public Day20(bool isTest = false) : base("20", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        _points = new Point[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            long[] numbers = [.. NumbersRegex().Matches(lines[i]).Select(n => long.Parse(n.Value))];
            _points[i] = new(
                i,
                (numbers[0], numbers[1], numbers[2]),
                (numbers[3], numbers[4], numbers[5]),
                (numbers[6], numbers[7], numbers[8]));
        }
    }

    private static (double X, double Y, double Z) PositionAt(Point point, long time)
    {
        double x = point.Pos.X + point.Speed.X * time + 0.5 * point.Acc.X * time * time;
        double y = point.Pos.Y + point.Speed.Y * time + 0.5 * point.Acc.Y * time * time;
        double z = point.Pos.Z + point.Speed.Z * time + 0.5 * point.Acc.Z * time * time;

        return (x, y, z);
    }

    private static double ManhattanDistance((double x, double y, double z) pointA, (double x, double y, double z) pointB)
    {
        return
            Math.Abs(pointA.x - pointB.x) +
            Math.Abs(pointA.y - pointB.y) +
            Math.Abs(pointA.z - pointB.z);
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        return
            _points
            .Select(p =>
            {
                return (p.Id, Pos: PositionAt(p, 100_000));
            })
            .MinBy(p => ManhattanDistance((0.0, 0.0, 0.0), p.Pos))
            .Id;
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
        HashSet<Point> points = [.. _points];
        for (int tick = 0; tick < 1000; tick++)
        {
            var moved = points.Select(p =>
            {
                var speed = (
                    X: p.Speed.X + p.Acc.X,
                    Y: p.Speed.Y + p.Acc.Y,
                    Z: p.Speed.Z + p.Acc.Z);

                var pos = (
                    X: p.Pos.X + speed.X,
                    Y: p.Pos.Y + speed.Y,
                    Z: p.Pos.Z + speed.Z);

                return p with { Pos = pos, Speed = speed };
            });

            points = [.. moved
                .GroupBy(p => p.Pos)
                .Where(group => group.Count() == 1) // remove particles that collided
                .SelectMany(group => group)];
        }
        return points.Count;
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion

    [GeneratedRegex(@"-?\d+")]
    private static partial Regex NumbersRegex();
}
