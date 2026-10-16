using System.Diagnostics;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day18;

public class Day18(bool isTest = false) : BaseDay("18", isTest)
{
    #region Part 1

    static bool IsTrap(char[] tiles)
    {
        if (tiles[0] == '^' && tiles[1] == '^' && tiles[2] != '^')
            return true;
        if (tiles[0] != '^' && tiles[1] == '^' && tiles[2] == '^')
            return true;
        if (tiles[0] == '^' && tiles[1] != '^' && tiles[2] != '^')
            return true;
        if (tiles[0] != '^' && tiles[1] != '^' && tiles[2] == '^')
            return true;

        return false;
    }

    static (char[], int safes) NextRow(char[] currentRow)
    {
        char[] nextRow = new char[currentRow.Length];
        int safe = 0;
        // most left
        bool isTrap = IsTrap(['0', currentRow[0], currentRow[1]]);
        nextRow[0] = '.';
        if (isTrap)
            nextRow[0] = '^';
        else
            safe++;

        for (int idx = 1; idx < currentRow.Length - 1; idx++)
        {
            isTrap = IsTrap([currentRow[idx - 1], currentRow[idx], currentRow[idx + 1]]);
            nextRow[idx] = '.';
            if (isTrap)
                nextRow[idx] = '^';
            else
                safe++;
        }

        // most right
        isTrap = IsTrap([currentRow[^2], currentRow[^1], '0']);
        nextRow[currentRow.Length - 1] = '.';
        if (isTrap)
            nextRow[currentRow.Length - 1] = '^';
        else
            safe++;

        return (nextRow, safe);
    }

    [Benchmark]
    public int RunPart1()
    {
        int maxRows = 40;
        char[][] tiles = new char[maxRows][];
        tiles[0] = [.. Content];
        char[] currentRow = [.. Content];
        int linesCount = 1;
        int safes = currentRow.Count(c => c == '.');
        Console.WriteLine($"{string.Join("", currentRow)}");
        while (linesCount < maxRows)
        {
            (currentRow, int s) = NextRow(currentRow);
            safes += s;
            Console.WriteLine($"{string.Join("", currentRow)}");
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
        int maxRows = 400000;
        char[][] tiles = new char[maxRows][];
        tiles[0] = [.. Content];
        char[] currentRow = [.. Content];
        int linesCount = 1;
        int safes = currentRow.Count(c => c == '.');
        // Console.WriteLine($"{string.Join("", currentRow)}");
        while (linesCount < maxRows)
        {
            // if (linesCount % 200 == 0)
            //     Console.WriteLine($"Creating row... {linesCount}");
            (currentRow, int s) = NextRow(currentRow);
            safes += s;
            // Console.WriteLine($"{string.Join("", currentRow)}");
            tiles[linesCount] = currentRow;
            linesCount++;
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
