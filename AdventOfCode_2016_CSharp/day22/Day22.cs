using System.Collections.Immutable;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day22;

public partial class Day22 : BaseDay
{
    readonly ImmutableArray<GridFile> _files;

    public Day22(bool isTest) : base("22", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        var builder = ImmutableArray.CreateBuilder<GridFile>(lines.Length - 2);

        foreach (var line in lines[2..])
        {
            int[] values = [.. NumbersRegex().Matches(line).Select(m => int.Parse(m.Value))];
            builder.Add(new(values[0], values[1], values[2], values[3], values[4], values[5]));
        }

        _files = builder.MoveToImmutable();
    }

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
        int pairs = 0;
        for (int i = 0; i < _files.Length; i++)
        {
            if (_files[i].Used == 0)
                continue;

            for (int j = 0; j < _files.Length; j++)
            {
                if (i != j && _files[i].Used <= _files[j].Avail)
                    pairs++;
            }
        }

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
        var empty = _files.Single(file => file.Used == 0);
        var movable = _files.Where(file => file.Used <= empty.Size).ToArray();
        var minSize = movable.Min(file => file.Size);
        var maxSize = movable.Max(file => file.Size);

        if (movable.Any(file => file.Used > minSize)
            || _files.Any(file => file.Used > empty.Size && file.Used <= maxSize))
            throw new InvalidOperationException("The grid cannot be modeled as a sliding puzzle.");

        var positions = movable.Select(file => (file.X, file.Y)).ToHashSet();
        var goal = (_files.Where(file => file.Y == 0).Max(file => file.X), 0);
        var start = (Empty: (empty.X, empty.Y), Goal: goal);
        var visited = new HashSet<((int X, int Y) Empty, (int X, int Y) Goal)> { start };
        var queue = new Queue<((int X, int Y) Empty, (int X, int Y) Goal, int Steps)>();
        queue.Enqueue((start.Empty, start.Goal, 0));
        (int X, int Y)[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

        while (queue.TryDequeue(out var state))
        {
            if (state.Goal == (0, 0))
                return state.Steps;

            foreach (var (X, Y) in directions)
            {
                var nextEmpty = (state.Empty.X + X, state.Empty.Y + Y);
                if (!positions.Contains(nextEmpty))
                    continue;

                var nextGoal = nextEmpty == state.Goal ? state.Empty : state.Goal;
                if (visited.Add((nextEmpty, nextGoal)))
                    queue.Enqueue((nextEmpty, nextGoal, state.Steps + 1));
            }
        }

        throw new InvalidOperationException("The goal data cannot reach node (0, 0).");
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
