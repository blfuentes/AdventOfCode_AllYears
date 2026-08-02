using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day13;

public class Day13 : BaseDay
{
    public Day13(bool isTest = false) : base("13", isTest)
    {
        OfDesFavNumber = int.Parse(Content);
    }

    private static int OfDesFavNumber;

    private static readonly List<(int dx, int dy)> diffs = [
    (-1, 0), (1, 0), (0, -1), (0, 1)
    ];

    static int NumOfOnes(string value)
    {
        int counter = 0;
        for (int idx = 0; idx < value.Length; idx++)
        {
            if (value[idx] == '1') counter++;
        }
        return counter;
    }
    static bool IsWall((int X, int Y) pos)
    {
        int val = OfDesFavNumber + (pos.X * pos.X + 3 * pos.X + 2 * pos.X * pos.Y + pos.Y + pos.Y * pos.Y);
        var binary = Common.Tools.ToBinary32(val);
        return NumOfOnes(binary) % 2 != 0;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int FindPath((int X, int Y) start, (int X, int Y) goal)
        {
            HashSet<(int, int)> visited = [];
            Queue<(int x, int y)> queue = [];
            Dictionary<(int, int), (int, int)> parents = [];

            visited.Add(start);
            queue.Enqueue(start);

            while (queue.TryDequeue(out (int x, int y) current))
            {
                if (current == goal) break;

                foreach (var (dx, dy) in diffs)
                {
                    var (npx, npy) = (current.x + dx, current.y + dy);
                    if (
                        npx < 0 || npy < 0 ||
                        visited.Contains((npx, npy)) || IsWall((npx, npy)))
                        continue;
                    visited.Add((npx, npy));
                    parents.Add((npx, npy), current);
                    queue.Enqueue((npx, npy));
                }
            }

            // reconstruct path
            int counter = 0;
            while (parents.TryGetValue(goal, out (int, int) prev))
            {
                counter++;
                goal = prev;
            }
            return counter;
        }

        (int, int) goal = IsTest ? (7, 4) : (31, 39);

        return FindPath((1, 1), goal);
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
        int counter = 50;
        (int x, int y) = (1, 1);
        HashSet<(int, int)> visited = [];
        Queue<(int, int)> queue = [];

        queue.Enqueue((1, 1));
        visited.Add((1, 1));

        while (counter > 0)
        {
            List<(int, int)> toExplore = [];
            while (queue.TryDequeue(out (int x, int y) result))
            {
                foreach (var (dx, dy) in diffs)
                {
                    var (npx, npy) = (result.x + dx, result.y + dy);
                    if (
                        npx >= 0 && npy >= 0 &&
                        !IsWall((npx, npy)) &&
                        !visited.Contains((npx, npy))
                    )
                    {
                        visited.Add((npx, npy));
                        toExplore.Add((npx, npy));
                    }
                }
            }
            toExplore.ForEach(e => queue.Enqueue(e));
            counter--;
        }
        return visited.Count;
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
