using System.Diagnostics;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day18;

public class Day18(bool isTest = false) : BaseDay("18", isTest)
{
    #region Part 1

    static bool IsTrap(bool[] tiles)
    {
        if (tiles[0] && tiles[1] && !tiles[2])
            return true;
        if (!tiles[0] && tiles[1] && tiles[2])
            return true;
        if (tiles[0] && !tiles[1] && !tiles[2])
            return true;
        if (!tiles[0] && !tiles[1] && tiles[2])
            return true;

        return false;
    }

    static (bool[], int safes) NextRow(bool[] currentRow)
    {
        bool[] nextRow = new bool[currentRow.Length];
        int safe = 0;
        // most left
        bool isTrap = IsTrap([false, currentRow[0], currentRow[1]]);
        nextRow[0] = false;
        if (isTrap)
            nextRow[0] = true;
        else
            safe++;

        for (int idx = 1; idx < currentRow.Length - 1; idx++)
        {
            isTrap = IsTrap([currentRow[idx - 1], currentRow[idx], currentRow[idx + 1]]);
            nextRow[idx] = false;
            if (isTrap)
                nextRow[idx] = true;
            else
                safe++;
        }

        // most right
        isTrap = IsTrap([currentRow[^2], currentRow[^1], false]);
        nextRow[currentRow.Length - 1] = false;
        if (isTrap)
            nextRow[currentRow.Length - 1] = true;
        else
            safe++;

        return (nextRow, safe);
    }

    [Benchmark]
    public int RunPart1()
    {
        int maxRows = 40;
        bool[][] tiles = new bool[maxRows][];
        tiles[0] = [.. Content.Select(c => c == '^')];
        bool[] currentRow = [.. Content.Select(c => c == '^')];
        int linesCount = 1;
        int safes = currentRow.Count(c => !c);
        while (linesCount < maxRows)
        {
            (currentRow, int s) = NextRow(currentRow);
            safes += s;
            tiles[linesCount] = currentRow;
            linesCount++;
        }
        return safes;
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
        const int maxRows = 400000;
        int width = Content.Length;
        UInt128 mask = (UInt128.One << width) - 1;

        UInt128 row = 0;
        foreach (char c in Content)
            row = (row << 1) | (c == '^' ? UInt128.One : UInt128.Zero);

        int safes = 0;
        for (int r = 0; r < maxRows; r++)
        {
            safes += width - (int)UInt128.PopCount(row);
            row = ((row << 1) ^ (row >> 1)) & mask;
        }
        return safes;
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
