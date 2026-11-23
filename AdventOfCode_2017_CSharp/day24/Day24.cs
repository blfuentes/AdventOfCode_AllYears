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
            var connectors = _connections.Where(c => c.PortA == t.ePort || c.PortB == t.ePort);
            if (connectors.Any())
            {
                foreach (var conn in connectors)
                {
                    toVisit.Push()
                }
            }
            else
            {
                bridges.Add([.. t.currentBridge]);
            }
        }

        return Content.Length;
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
