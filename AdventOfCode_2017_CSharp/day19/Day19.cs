using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day19;

public class Day19 : BaseDay
{
    readonly char[][] map;

    public Day19(bool isTest = false) : base("19", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        map = new char[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            map[i] = [.. lines[i]];
        }
    }

    (int Row, int Col) NextDir((int dr, int dc) dir, (int row, int col) pos)
    {
        char c = map[pos.row][pos.col];
        var newdir = c switch
        {
            '|' when dir == (1, 0) => (1, 0),
            '|' when dir == (-1, 0) => (-1, 0),
            '-' when dir == (1, 0) => (1, 0),
            '-' when dir == (-1, 0) => (-1, 0),
            '+' when dir == (1, 0) && map[pos.row][pos.col - 1] != ' ' => (0, -1),
            '+' when dir == (1, 0) && map[pos.row][pos.col + 1] != ' ' => (0, 1),
            '+' when dir == (-1, 0) && map[pos.row][pos.col - 1] != ' ' => (0, -1),
            '+' when dir == (-1, 0) && map[pos.row][pos.col + 1] != ' ' => (0, 1),
            '+' when dir == (0, 1) && map[pos.row - 1][pos.col] != ' ' => (-1, 0),
            '+' when dir == (0, 1) && map[pos.row + 1][pos.col] != ' ' => (1, 0),
            '+' when dir == (0, -1) && map[pos.row - 1][pos.col] != ' ' => (-1, 0),
            '+' when dir == (0, -1) && map[pos.row + 1][pos.col] != ' ' => (1, 0),
            _ => dir,
        };

        return newdir;
    }

    #region Part 1
    [Benchmark]
    public string RunPart1()
    {
        List<char> result = [];
        int column = map[0].IndexOf('|');
        (int row, int col) pos = (0, column);
        (int dr, int dc) dir = (1, 0);
        while (map[pos.row][pos.col] != ' ')
        {
            dir = NextDir(dir, pos);
            pos = (pos.row + dir.dr, pos.col + dir.dc);
            char c = map[pos.row][pos.col];
            if (c != '|' && c != '-' && c != '+' && c != ' ')
                result.Add(c);
        }

        return new string([.. result]);
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
        int column = map[0].IndexOf('|');
        (int row, int col) pos = (0, column);
        (int dr, int dc) dir = (1, 0);
        int counter = 0;
        while (map[pos.row][pos.col] != ' ')
        {
            dir = NextDir(dir, pos);
            pos = (pos.row + dir.dr, pos.col + dir.dc);
            counter++;
        }
        return counter;
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
