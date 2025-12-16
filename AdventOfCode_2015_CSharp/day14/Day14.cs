using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Stacks.Formats;

namespace AdventOfCode_2015_CSharp.day14;

public class Day14(bool isTest = false) : BaseDay("14", isTest)
{
    record Reindeer
    {
        public string Name { get; set; }
        public int Speed { get; set; }
        public int Running { get; set; }
        public int Resting { get; set; }
    }

    static int GetPosition(Reindeer reindeer, int second)
    {
        int cycles = second / (reindeer.Running +  reindeer.Resting);
        int remaining = second % (reindeer.Running + reindeer.Resting);
        int distance = cycles * reindeer.Running * reindeer.Speed + reindeer.Speed * Math.Min(remaining, reindeer.Running);
        return distance;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        var reindeers =
            File.ReadAllLines(InputPath).Select(line =>
            {
                return new Reindeer
                {
                    Name = line.Split(" ")[0],
                    Speed = int.Parse(line.Split(" ")[3]),
                    Running = int.Parse(line.Split(" ")[6]),
                    Resting = int.Parse(line.Split(" ")[13])
                };
            });
        return reindeers.Select(n => GetPosition(n, 2503)).Max();
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
        var reindeers =
            File.ReadAllLines(InputPath).Select(line =>
            {
                return new Reindeer
                {
                    Name = line.Split(" ")[0],
                    Speed = int.Parse(line.Split(" ")[3]),
                    Running = int.Parse(line.Split(" ")[6]),
                    Resting = int.Parse(line.Split(" ")[13])
                };
            });
        int[] points = new int[reindeers.Count()];
        IEnumerable<(int id, int distance)> results;
        for (int c = 1; c <= 2503; c++)
        {
            results = reindeers.Select((r, i) => (i, GetPosition(r, c)));
            int position = results.MaxBy(r => r.distance).distance;
            foreach(var (id, distance) in results)
            {
                if (distance == position)
                    points[id]++;
            }
        }
        return points.Max();
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
