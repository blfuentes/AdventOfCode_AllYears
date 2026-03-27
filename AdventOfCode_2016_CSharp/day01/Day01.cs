using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day01;

public class Day01(bool isTest = false) : BaseDay("01", isTest)
{
    enum Dir { North, South, East, West }

    Dictionary<(string, Dir), Dir> walkMapping =
        new()
        {
            { ("L", Dir.North), Dir.West },
            { ("L", Dir.South), Dir.East },
            { ("L", Dir.West), Dir.South },
            { ("L", Dir.East), Dir.North },
            { ("R", Dir.North), Dir.East },
            { ("R", Dir.South), Dir.West },
            { ("R", Dir.West), Dir.North },
            { ("R", Dir.East), Dir.South },
        };

    Dictionary<Dir, (int DeltaRow, int DeltaCol)> directionDeltas =
        new()
        {
            { Dir.North, (1, 0) },
            { Dir.South, (-1, 0) },
            { Dir.East, (0, 1) },
            { Dir.West, (0, -1) },
        };

    void Walk(int[] pos, int steps, string d, ref Dir dir)
    {
        dir = walkMapping[(d, dir)];
        var (deltaRow, deltaCol) = directionDeltas[dir];
        pos[0] += deltaRow * steps;
        pos[1] += deltaCol * steps;
    }

    private static int Distance(int[] origin, int[] target) => Math.Abs(origin[0] - target[0]) + Math.Abs(origin[1] - target[1]);
    private static int DistancePos((int Row, int Col) origin, (int Row, int Col) target) => Math.Abs(origin.Row - target.Row) + Math.Abs(origin.Col - target.Col);

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int[] current = [0, 0];
        Dir currentDir = Dir.North;

        foreach (var op in Content.Split(", "))
        {
            Walk(current, int.Parse(op[1..]), op[..1], ref currentDir);
        }

        return Distance([0, 0], current);
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

    HashSet<(int, int)> visited = [(0, 0)];

    bool WalkAndRemember(ref (int Row, int Col) pos, int steps, string d, ref Dir dir)
    {
        dir = walkMapping[(d, dir)];
        var (deltaRow, deltaCol) = directionDeltas[dir];

        while (steps > 0)
        {
            pos.Row += deltaRow;
            pos.Col += deltaCol;
            steps--;
            if (visited.Contains(pos))
                return true;
            visited.Add(pos);
        }
        return false;
    }

    [Benchmark]
    public int RunPart2()
    {
        (int Row, int Col) current = (0, 0);
        Dir currentDir = Dir.North;        

        foreach (var op in Content.Split(", "))
        {
            if (WalkAndRemember(ref current, int.Parse(op[1..]), op[..1], ref currentDir))
                break;
        }

        return DistancePos((0, 0), current);
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
