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
        var root = _nodes.Single(n => !_connections.Any(c => c.ToNode == n));
        Queue<char> toVisit = [];
        toVisit.Enqueue(root);
        HashSet<char> visited = [];
        while (toVisit.TryDequeue(out char current))
        {
            visited.Add(current);
            if (_paths.ContainsKey(current))
            {
                foreach (var next in _paths[current].Order())
                {
                    if (!visited.Contains(next))
                        toVisit.Enqueue(next);
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
        return Content.Length;
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
