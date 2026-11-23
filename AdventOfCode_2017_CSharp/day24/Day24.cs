using System.Data;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day24;

public class Day24 : BaseDay
{
    public record Connection(int Id, int PortA, int PortB)
    {
        public bool CanConnect(Connection conn)
        {
            return
                conn.PortA == this.PortA || conn.PortB == this.PortA ||
                conn.PortA == this.PortB || conn.PortB == this.PortB;
        }

        public (bool connects, int? output) CanConnect(int input)
        {
            (bool c, int? o) = input switch
            {
                _ when (input == this.PortA) => (true, this.PortB),
                _ when (input == this.PortB) => (true, this.PortA),
                _ => (false, (int?)null)
            };
            return (c, o);
        }
    }

    private readonly Connection[] _connections;

    public Day24(bool isTest = false) : base("24", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        _connections = new Connection[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            var indexOf = lines[i].IndexOf('/');
            _connections[i] = new Connection(
                i,
                int.Parse(lines[i][..indexOf]),
                int.Parse(lines[i][(indexOf + 1)..])
            );
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        List<List<Connection>> bridges = [];
        int startingPort = 0;
        Stack<(int endingPort, HashSet<Connection> bridge)> toVisit = [];
        toVisit.Push((startingPort, []));

        while (toVisit.TryPop(out (int ePort, HashSet<Connection> currentBridge) t))
        {
            var connectors = _connections
                .Select(c => (c, c.CanConnect(t.ePort)))
                .Where(c => c.Item2.connects);

            if (connectors.Any())
            {
                foreach (var conn in connectors)
                {

                    if (!t.currentBridge.Contains(conn.c))
                    {
                        HashSet<Connection> newBridge = [.. t.currentBridge];
                        _ = newBridge.Add(conn.c);
                        bridges.Add([.. newBridge]);
                        toVisit.Push((conn.Item2.output!.Value, newBridge));
                    }
                }
            }
            else
            {
                bridges.Add([.. t.currentBridge]);
            }
        }

        return bridges.Max(b => b.Sum(bb => bb.PortA + bb.PortB));
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
        List<List<Connection>> bridges = [];
        int startingPort = 0;
        Stack<(int endingPort, HashSet<Connection> bridge)> toVisit = [];
        toVisit.Push((startingPort, []));

        while (toVisit.TryPop(out (int ePort, HashSet<Connection> currentBridge) t))
        {
            var connectors = _connections
                .Select(c => (c, c.CanConnect(t.ePort)))
                .Where(c => c.Item2.connects);

            if (connectors.Any())
            {
                foreach (var conn in connectors)
                {

                    if (!t.currentBridge.Contains(conn.c))
                    {
                        HashSet<Connection> newBridge = [.. t.currentBridge];
                        _ = newBridge.Add(conn.c);
                        bridges.Add([.. newBridge]);
                        toVisit.Push((conn.Item2.output!.Value, newBridge));
                    }
                }
            }
            else
            {
                bridges.Add([.. t.currentBridge]);
            }
        }

        return
            bridges
            .GroupBy(b => b.Count)
            .OrderByDescending(g => g.Key)
            .First()
            .Max(b => b.Sum(bb => bb.PortA + bb.PortB));
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
