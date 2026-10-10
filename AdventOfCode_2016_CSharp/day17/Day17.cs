using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode_2016_CSharp.day17;

public class Day17(bool isTest = false) : BaseDay("17", isTest)
{
    #region Part 1
    private const int MaxDimension = 4;
    static string ComputeHash(string input)
    {
        byte[] hashBytes;
        hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    static bool IsOpen(char door)
    {
        return door == 'b' || door == 'c' || door == 'd' || door == 'e' || door == 'f';
    }

    static bool[] OpenDoors(string passcodePath)
    {
        var hashed = ComputeHash(passcodePath);
        return [.. hashed[..4].Select(IsOpen)];

    }

    static char ToDir(int index)
    {
        return index switch
        {
            0 => 'U',
            1 => 'D',
            2 => 'L',
            3 => 'R',
            _ => throw new ArgumentOutOfRangeException("not valid dir")
        };
    }

    static bool ValidPos(Coord pos) =>
        pos.Row >= 0 && pos.Row < MaxDimension &&
        pos.Col >= 0 && pos.Col < MaxDimension;

    readonly record struct Coord(int Row, int Col)
    {
        public static Coord operator +(Coord operand, (int r, int c) d) =>
            new(operand.Row + d.r, operand.Col + d.c);
    }

    readonly record struct CoordInfo(Coord Pos, string Path);

    [Benchmark]
    public string RunPart1()
    {
        CoordInfo startPos = new(new(0, 0), Content);
        Coord endPos = new(MaxDimension - 1, MaxDimension - 1);

        Queue<CoordInfo> queue = [];
        queue.Enqueue(startPos);
        (int r, int c)[] dirs = [(-1, 0), (1, 0), (0, -1), (0, 1)];

        while (queue.TryDequeue(out CoordInfo current))
        {
            if (Equals(current.Pos, endPos))
                return current.Path[Content.Length..];

            var doorStates = OpenDoors(current.Path);
            for (int dIdx = 0; dIdx < 4; dIdx++)
            {
                Coord newPos = current.Pos + dirs[dIdx];
                if (doorStates[dIdx] &&
                    ValidPos(newPos))
                {
                    CoordInfo newState = new(newPos, current.Path + ToDir(dIdx));
                    queue.Enqueue(newState);
                }
            }
        }

        return "";
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
        CoordInfo startPos = new(new(0, 0), Content);
        Coord endPos = new(MaxDimension - 1, MaxDimension - 1);

        Queue<CoordInfo> queue = [];
        queue.Enqueue(startPos);
        (int r, int c)[] dirs = [(-1, 0), (1, 0), (0, -1), (0, 1)];

        List<int> pathLengths = [];

        while (queue.TryDequeue(out CoordInfo current))
        {
            if (Equals(current.Pos, endPos))
            {
                pathLengths.Add(current.Path.Length - Content.Length);
            }
            else
            {
                var doorStates = OpenDoors(current.Path);
                for (int dIdx = 0; dIdx < 4; dIdx++)
                {
                    Coord newPos = current.Pos + dirs[dIdx];
                    if (doorStates[dIdx] &&
                        ValidPos(newPos))
                    {
                        CoordInfo newState = new(newPos, current.Path + ToDir(dIdx));
                        queue.Enqueue(newState);
                    }
                }
            }
        }

        return pathLengths.Max();
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
