using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2018_CSharp.day07;

public class Day07 : BaseDay
{
    public struct Node
    {
        public char Id;
    }

    public struct Connection
    {
        public char FromNode;
        public char ToNode;
    }

    public readonly HashSet<char> _nodes;
    public readonly HashSet<Connection> _connections;
    public readonly Dictionary<char, HashSet<char>> _paths;

    public Day07(bool isTest = false) : base("07", isTest)
    {
        _nodes = [];
        _connections = [];
        _paths = [];

        foreach (var line in File.ReadAllLines(InputPath))
        {
            (char initnode, char endnode) = (line.Split(" ")[1][0], line.Split(" ")[7][0]);
            _ = _nodes.Add(initnode);
            _ = _nodes.Add(endnode);
            _connections.Add(new Connection()
            {
                FromNode = initnode,
                ToNode = endnode
            });
            if (!_paths.TryAdd(initnode, [endnode]))
            {
                _paths[initnode].Add(endnode);
            }
        }
    }

    #region stPart 1
    [Benchmark]
    public string RunPart1()
    {
        var roots = _nodes.Where(n => !_connections.Any(c => c.ToNode == n));
        PriorityQueue<char, int> toVisit = new();
        foreach (var root in roots)
            toVisit.Enqueue(root, root);
        List<char> visited = [];
        while (toVisit.TryDequeue(out char current, out _))
        {
            visited.Add(current);
            if (_paths.ContainsKey(current))
            {
                foreach (var next in _paths[current])
                {
                    if (_connections
                        .Where(c => c.ToNode == next)
                        .All(c =>
                            visited.Contains(c.FromNode)
                        ))
                    {
                        if (!visited.Contains(next))
                            toVisit.Enqueue(next, next);
                    }
                }
            }
        }

        return string.Join("", visited);
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
        const int maxWorkers = 5;
        const int normalizer = 4;
        HashSet<char> completed = [];
        Dictionary<char, int> inProgress = [];
        var time = 0;

        while (completed.Count < _nodes.Count)
        {
            var ready = _nodes
                .Where(node => !completed.Contains(node) && !inProgress.ContainsKey(node))
                .Where(node => _connections
                    .Where(connection => connection.ToNode == node)
                    .All(connection => completed.Contains(connection.FromNode)))
                .OrderBy(node => node)
                .Take(maxWorkers - inProgress.Count);

            foreach (var node in ready)
                inProgress.Add(node, node - normalizer);

            foreach (var node in inProgress.Keys.ToArray())
            {
                inProgress[node]--;
                if (inProgress[node] == 0)
                {
                    inProgress.Remove(node);
                    completed.Add(node);
                }
            }

            time++;
        }

        return time;
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
