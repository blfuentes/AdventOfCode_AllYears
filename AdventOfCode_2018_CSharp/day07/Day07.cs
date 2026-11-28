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

    #region Part 1
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
        var roots = _nodes.Where(n => !_connections.Any(c => c.ToNode == n));
        PriorityQueue<(char, int), int> toVisit = new();
        int availableWorkers = 2;

        foreach (var root in roots)
        {
            toVisit.Enqueue((root, root - 64), root);
            availableWorkers--;
        }

        List<char> visited = [];
        int time = 0;
        while (toVisit.TryDequeue(out (char c, int r) current, out _))
        {
            if (availableWorkers > 0)
            {
                if (current.r == 0)
                {
                    availableWorkers++;
                    visited.Add(current.c);
                    if (_paths.ContainsKey(current.c))
                    {
                        foreach (var next in _paths[current.c])
                        {
                            if (_connections
                                .Where(c => c.ToNode == next)
                                .All(c =>
                                    visited.Contains(c.FromNode)
                                ))
                            {
                                if (!visited.Contains(next) && availableWorkers > 0)
                                {
                                    availableWorkers--;
                                    toVisit.Enqueue((next, next - 64), next);
                                }
                            }
                        }
                    }
                }
                else
                {
                    toVisit.Enqueue((current.c, current.r - 1), current.c);
                }
            }
            else
            {
                if (current.r > 0)
                {
                    toVisit.Enqueue((current.c, current.r - 1), current.c);
                }
                else
                {
                    visited.Add(current.c);
                    availableWorkers++;
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
