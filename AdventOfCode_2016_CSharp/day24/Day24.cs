using System.Collections.Immutable;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day24;

public class Day24 : BaseDay
{
    readonly struct Location(char id, int row, int col)
    {
        public readonly char Id = id;
        public readonly int Row = row;
        public readonly int Col = col;

        public bool Free => Id != '#';
        public bool Equals(Location location) => Row == location.Row && Col == location.Col;
    }

    static IEnumerable<Location[]> GetRoutes(
        IReadOnlyList<Location> points, char startId, bool fixedStart = true, bool closed = true)
    {
        if (points.Count == 0) yield break;

        if (fixedStart)
        {
            // Put the start at index 0, then permute only the rest
            var arr = points.ToArray();
            int s = Array.FindIndex(arr, p => p.Id == startId);
            if (s < 0) throw new ArgumentException($"Start '{startId}' not found");
            (arr[0], arr[s]) = (arr[s], arr[0]);

            foreach (var _ in Permute(arr, 1))
                yield return closed ? Close(arr) : arr.ToArray();
        }
        else
        {
            var arr = points.ToArray();
            foreach (var _ in Permute(arr, 0))
                yield return closed ? Close(arr) : arr.ToArray();
        }
    }

    // Permutes arr[from..] in place using swap/backtrack. The yielded value is a dummy;
    // the caller reads arr right away.
    static IEnumerable<bool> Permute(Location[] arr, int from)
    {
        if (from >= arr.Length - 1) { yield return true; yield break; }

        for (int i = from; i < arr.Length; i++)
        {
            (arr[from], arr[i]) = (arr[i], arr[from]);
            foreach (var x in Permute(arr, from + 1))
                yield return x;
            (arr[from], arr[i]) = (arr[i], arr[from]);
        }
    }

    // Copy and append the start to close the loop
    static Location[] Close(Location[] arr)
    {
        var route = new Location[arr.Length + 1];
        arr.CopyTo(route, 0);
        route[^1] = arr[0];
        return route;
    }

    readonly Dictionary<(int row, int col), Location> _map = [];
    readonly ImmutableArray<Location> _checkPoints;

    public Day24(bool isTest = false) : base("24", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        (int maxRows, int maxCols) = (lines.Length, lines[0].Length);
        var builder = ImmutableArray.CreateBuilder<Location>();

        for (int rIdx = 0; rIdx < maxRows; rIdx++)
        {
            for (int cIdx = 0; cIdx < maxCols; cIdx++)
            {
                char id = lines[rIdx][cIdx];
                _map.Add((rIdx, cIdx), new(id, rIdx, cIdx));
                if (id != '#' && id != '.')
                    builder.Add(_map[(rIdx, cIdx)]);
            }
        }
        _checkPoints = builder.ToImmutableArray<Location>();
    }

    readonly Dictionary<(char, char), int> _distances = [];

    int Distance(Location start, Location end)
    {
        var key = start.Id <= end.Id ? (start.Id, end.Id) : (end.Id, start.Id);
        if (!_distances.TryGetValue(key, out int distance))
        {
            distance = ComputeDistance(start, end);
            _distances[key] = distance;
        }
        return distance;
    }

    int ComputeDistance(Location start, Location end)
    {
        HashSet<(int, int)> visited = [(start.Row, start.Col)];
        Queue<(Location loc, int steps)> queue = [];
        queue.Enqueue((start, 0));
        (int dr, int dc)[] dn = [(-1, 0), (1, 0), (0, -1), (0, 1)];

        while (queue.TryDequeue(out var cur))
        {
            if (cur.loc.Equals(end)) return cur.steps;
            foreach (var (dr, dc) in dn)
            {
                var key = (cur.loc.Row + dr, cur.loc.Col + dc);
                if (_map.TryGetValue(key, out var n) && n.Free && visited.Add(key))
                    queue.Enqueue((n, cur.steps + 1));
            }
        }
        return int.MaxValue;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        var possibleWays = GetRoutes(_checkPoints, '0', fixedStart: true, closed: false);

        int minDistance = int.MaxValue;
        foreach (var route in possibleWays)
        {
            int currentDistance = 0;
            foreach (var (a, b) in route.Zip(route.Skip(1)))
            {
                currentDistance += Distance(a, b);
                if (currentDistance > minDistance)
                    break;
            }

            minDistance = Math.Min(minDistance, currentDistance);
        }
        return minDistance;
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
        var possibleWays = GetRoutes(_checkPoints, '0', fixedStart: true, closed: true);

        int minDistance = int.MaxValue;
        foreach (var route in possibleWays)
        {
            int currentDistance = 0;
            foreach (var (a, b) in route.Zip(route.Skip(1)))
            {
                currentDistance += Distance(a, b);
                if (currentDistance > minDistance)
                    break;
            }

            minDistance = Math.Min(minDistance, currentDistance);
        }
        return minDistance;
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
