using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day12;

public partial class Day12 : BaseDay
{
    struct Node(int id, int[] nodes)
    {
        public int Id = id;
        public int[] Nodes = nodes;
    }


    private readonly Dictionary<int, Node> _programs;
    public Day12(bool isTest = false) : base("12", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        _programs = [];

        foreach (var line in lines)
        {
            int[] numbers = [.. NumbersRegex().Matches(line).Select(c => int.Parse(c.Value))];
            _programs.Add(numbers[0], new(numbers[0], numbers[1..]));
        }
    }

    HashSet<int> GetConnectedPrograms(int root)
    {
        HashSet<int> visited = [];
        visited.Add(root);
        Stack<int> toVisit = new([root]);
        while (toVisit.TryPop(out int current))
        {
            foreach (var connection in _programs[current].Nodes)
            {
                if (visited.Add(connection))
                    toVisit.Push(connection);

            }
        }

        return visited;
    }

    #region Part 1

    [Benchmark]
    public int RunPart1()
    {
        return GetConnectedPrograms(0).Count;
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
        HashSet<int> nodes = [.. _programs.Keys];
        int numOfGroups = 0;
        while (nodes.Count > 0 && (nodes.FirstOrDefault() is int current))
        {
            var currentGroup = GetConnectedPrograms(current);
            numOfGroups++;
            nodes.ExceptWith(currentGroup);

        }
        return numOfGroups;
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
