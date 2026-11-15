using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Disassemblers;

namespace AdventOfCode_2017_CSharp.day22;

public class Day22 : BaseDay
{
    private readonly bool[][] _map;
    private readonly HashSet<(int, int)> _infections = [];
    private readonly (int Row, int Col) _initPosition;

    static (int dr, int dc) TurnLeft((int r, int c) dir)
    {
        return dir switch
        {
            (0, 1) => (-1, 0),
            (0, -1) => (1, 0),
            (1, 0) => (0, 1),
            (-1, 0) => (0, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), "invalid direction")
        };
    }

    static (int dr, int dc) TurnRight((int r, int c) dir)
    {
        return dir switch
        {
            (0, 1) => (1, 0),
            (0, -1) => (-1, 0),
            (1, 0) => (0, -1),
            (-1, 0) => (0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), "invalid direction")
        };
    }

    public Day22(bool isTest = false) : base("22", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        int mid = lines.Length / 2;
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines.Length; col++)
            {
                if (lines[row][col] == '#')
                {
                    _infections.Add((row - mid, col - mid));
                }
            }
        }
        _initPosition = (mid, mid);
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int numOfBurst = 0;
        int maxBurst = 10000;
        int infected = 0;
        (int r, int c) current = (0, 0);
        (int dr, int dc) dir = (-1, 0);

        while (numOfBurst++ < maxBurst)
        {
            dir = _infections.Contains(current) ? TurnRight(dir) : TurnLeft(dir);
            if (_infections.Contains(current))
            {
                _infections.Remove(current);
            }
            else
            {
                _infections.Add(current);
                infected++;
            }
            current = (current.r + dir.dr, current.c + dir.dc);
        }

        return infected;
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
        int numOfBurst = 0;
        int maxBurst = 10000000;
        int infected = 0;
        (int r, int c) current = (0, 0);
        (int dr, int dc) dir = (-1, 0);

        HashSet<(int, int)> weakened = [];
        HashSet<(int, int)> flagged = [];

        while (numOfBurst++ < maxBurst)
        {
            if (_infections.Remove(current))
            {
                flagged.Add(current);
                dir = TurnRight(dir);
            }
            else if (weakened.Remove(current))
            {
                _infections.Add(current);
                infected++;
            }
            else if (flagged.Remove(current))
            {
                dir = (dir.dr * -1, dir.dc * -1);
            }
            else
            {
                weakened.Add(current);
                dir = TurnLeft(dir);
            }
            current = (current.r + dir.dr, current.c + dir.dc);
        }

        return infected;
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
