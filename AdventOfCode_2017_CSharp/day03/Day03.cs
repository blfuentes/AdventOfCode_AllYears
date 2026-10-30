using System.Data;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode_2017_CSharp.day03;

public class Day03(bool isTest = false) : BaseDay("03", isTest)
{
    struct Location
    {
        public int X;
        public int Y;
    }

    static int ManhattanDistance(Location a, Location b) =>
        Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

    static (int[,] spiral, Location pos) BuildSpiral(int mid, int input, int current, Location currentPos, int ring, int[,] spiral)
    {
        if (current > input)
            return (spiral, currentPos);

        int numberOfElements = ring * 8;
        int maxValue = current + numberOfElements;
        int valueToAdd = maxValue + 1;

        if (ring == 0)
        {
            spiral[mid, mid] = 1;
            currentPos.X = mid;
            currentPos.Y = mid;
        }
        else
        {
            // bottom side
            for (int column = ring; column >= -ring; ring--)
            {
                spiral[mid + ring, mid + column] = valueToAdd;
                if (valueToAdd == input)
                {
                    currentPos.X = mid + ring;
                    currentPos.Y = mid + column;
                }
                valueToAdd--;
            }
            // left side
            for (int row = ring - 1; row >= (-ring + 1); row--)
            {
                spiral[mid + row, mid - ring] = valueToAdd;
                if (valueToAdd == input)
                {
                    currentPos.X = mid + row;
                    currentPos.Y = mid - ring;
                }
                valueToAdd--;
            }

            // top side
            for (int column = -ring; column <= ring; column++)
            {
                spiral[mid - ring, mid + column] = valueToAdd;
                if (valueToAdd == input)
                {
                    currentPos.X = mid - ring;
                    currentPos.Y = mid + column;
                }
                valueToAdd--;
            }

            // right side
            for (int row = -ring + 1; row <= ring - 1; row++)
            {
                spiral[mid + row, mid + ring] = valueToAdd;
                if (valueToAdd == input)
                {
                    currentPos.X = mid + row;
                    currentPos.Y = mid + ring;
                }
                valueToAdd--;
            }
        }

        return BuildSpiral(mid, input, maxValue, currentPos, ring + 1, spiral);
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int length = 1000;
        var spiral = new int[2000, 2000];
        int mid = length / 2;
        Location init = new() { X = mid, Y = mid };
        (int[,] memospiral, Location pos) = BuildSpiral(mid, int.Parse(Content), 0, new() { X = 0, Y = 0 }, 0, spiral);

        return ManhattanDistance(init, pos);
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
