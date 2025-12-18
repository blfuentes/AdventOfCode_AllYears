using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.Common;

namespace AdventOfCode_2015_CSharp.day18;

public class Day18(bool isTest = false) : BaseDay("18", isTest)
{
    int Dimension { get; set; }
    bool[,] Grid { get; set; }
    readonly List<(int dx, int dy)> deltas =
        [
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, -1),
            (0, 1),
            (1, -1),
            (1, 0),
            (1, 1)
        ];

    bool InRange((int x, int y) pos)
    {
        return pos.x >= 0 && pos.x < Dimension && pos.y >= 0 && pos.y < Dimension;
    }

    bool SetState((int x, int y) pos)
    {
        var neighborsOn =
            deltas
            .Select(d => (d.dx + pos.x, d.dy + pos.y))
            .Where(InRange)
            .Where(pos => Grid[pos.Item1, pos.Item2])
            .Count();
        if (Grid[pos.x, pos.y])
        {
            return neighborsOn == 2 || neighborsOn == 3;
        }
        else
        {
            return neighborsOn == 3;
        }
    }

    #region Part 1

    int Run(int steps)
    {
        bool[,] newStates = new bool[Dimension, Dimension];
        while (steps > 0)
        {
            for(int x = 0; x < Dimension; x++)
            {
                for(int y = 0; y < Dimension; y++)
                {
                    newStates[x, y] = SetState((x, y));
                }
            }
            for (int x = 0; x < Dimension; x++)
            {
                for (int y = 0; y < Dimension; y++)
                {
                    Grid[x, y] = newStates[x, y];
                }
            }
            steps--;
        }
        return Grid.Cast<bool>().Where(_ => _).Count();
    }

    void PrintGrid()
    {
        for (int x = 0; x < Dimension; x++)
        {
            for (int y = 0; y < Dimension; y++)
            {
                Console.Write($"{(Grid[x, y] ? '#' : '.')}");
            }
            Console.Write($"{System.Environment.NewLine}");
        }
    }

    [Benchmark]
    public int RunPart1()
    {
        var lines = File.ReadLines(InputPath);
        Dimension = lines.Count();
        Grid = new bool[Dimension, Dimension];
        for (int i = 0; i < Dimension; i++)
        {
            for (int j = 0; j < Dimension; j++)
                Grid[i, j] = lines.ElementAt(i)[j] == '#';
        }

        return Run(100);
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

    bool IsCorner((int x, int y) pos)
    {
        bool topLeft = pos.x == 0 && pos.y == 0;
        bool topRight = pos.x == 0 && pos.y == Dimension - 1;
        bool bottomLeft = pos.x == Dimension - 1 && pos.y == 0;
        bool bottomRight = pos.x == Dimension - 1 && pos.y == Dimension - 1;

        return topLeft || topRight || bottomLeft || bottomRight;
    }

    int Run2(int steps)
    {
        bool[,] newStates = new bool[Dimension, Dimension];
        while (steps > 0)
        {
            for (int x = 0; x < Dimension; x++)
            {
                for (int y = 0; y < Dimension; y++)
                {
                    newStates[x, y] = SetState((x, y));
                }
            }
            for (int x = 0; x < Dimension; x++)
            {
                for (int y = 0; y < Dimension; y++)
                {
                    Grid[x, y] = IsCorner((x, y)) || newStates[x, y];
                }
            }
            steps--;
        }
        return Grid.Cast<bool>().Where(_ => _).Count();
    }

    [Benchmark]
    public int RunPart2()
    {
        var lines = File.ReadLines(InputPath);
        Dimension = lines.Count();
        Grid = new bool[Dimension, Dimension];
        for (int i = 0; i < Dimension; i++)
        {
            for (int j = 0; j < Dimension; j++)
                Grid[i, j] = IsCorner((i, j)) || lines.ElementAt(i)[j] == '#';
        }

        return Run2(100);
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
