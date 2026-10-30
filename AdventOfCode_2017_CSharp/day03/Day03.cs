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

    #region Part 1
    static (int[,] spiral, Location pos) BuildSpiral(int mid, int input, int result, Location currentPos, int ring, int[,] spiral)
    {
        if (result > input)
            return (spiral, currentPos);

        int numberOfElements = ring * 8;
        int maxValue = result + numberOfElements;
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
            for (int column = ring; column >= -ring; column--)
            {
                int xvalue = mid + ring;
                int yvalue = mid + column;

                UpdateSpiral(input, ref currentPos, spiral, ref valueToAdd, xvalue, yvalue);
            }
            // left side
            for (int row = ring - 1; row >= (-ring + 1); row--)
            {
                int xvalue = mid + row;
                int yvalue = mid - ring;

                UpdateSpiral(input, ref currentPos, spiral, ref valueToAdd, xvalue, yvalue);
            }

            // top side
            for (int column = -ring; column <= ring; column++)
            {
                int xvalue = mid - ring;
                int yvalue = mid + column;

                UpdateSpiral(input, ref currentPos, spiral, ref valueToAdd, xvalue, yvalue);
            }

            // right side
            for (int row = -ring + 1; row <= ring - 1; row++)
            {
                int xvalue = mid + row;
                int yvalue = mid + ring;

                UpdateSpiral(input, ref currentPos, spiral, ref valueToAdd, xvalue, yvalue);
            }
        }

        return BuildSpiral(mid, input, maxValue, currentPos, ring + 1, spiral);
    }

    private static void UpdateSpiral(int input, ref Location currentPos, int[,] spiral, ref int valueToAdd, int xvalue, int yvalue)
    {
        spiral[xvalue, yvalue] = valueToAdd;
        if (valueToAdd == input)
        {
            currentPos.X = xvalue;
            currentPos.Y = yvalue;
        }
        valueToAdd--;
    }
    [Benchmark]
    public int RunPart1()
    {
        int length = 1000;
        var spiral = new int[length, length];
        int mid = length / 2;
        Location init = new() { X = mid, Y = mid };
        (_, Location pos) = BuildSpiral(mid, int.Parse(Content), 0, new() { X = 0, Y = 0 }, 0, spiral);

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
    static int ValueOfLocation(Location pos, int[,] spiral)
    {
        int value = 0;
        for (int row = -1; row <= 1; row++)
        {
            for (int col = -1; col <= 1; col++)
            {
                value += spiral[row + pos.X, col + pos.Y];
            }
        }
        return value;
    }

    static int BuildSpiralInc(int mid, int input, int result, int ring, int[,] spiral)
    {
        if (result > input)
            return result;

        if (ring == 0)
            spiral[mid, mid] = 1;
        else
        {
            // right side
            for (int row = ring - 1; row >= (-ring + 1); row--)
            {
                int xvalue = mid + row;
                int yvalue = mid + ring;

                UpdateSpiral(input, ref result, spiral, xvalue, yvalue);
            }

            // top side
            for (int column = ring; column >= -ring; column--)
            {
                int xvalue = mid - ring;
                int yvalue = mid + column;

                UpdateSpiral(input, ref result, spiral, xvalue, yvalue);
            }

            // left side
            for (int row = -ring + 1; row <= (ring - 1); row++)
            {
                int xvalue = mid + row;
                int yvalue = mid - ring;

                UpdateSpiral(input, ref result, spiral, xvalue, yvalue);
            }

            // bottom side
            for (int column = -ring; column <= ring; column++)
            {
                int xvalue = mid + ring;
                int yvalue = mid + column;

                UpdateSpiral(input, ref result, spiral, xvalue, yvalue);
            }
        }

        return BuildSpiralInc(mid, input, result, ring + 1, spiral);
    }

    private static void UpdateSpiral(int input, ref int result, int[,] spiral, int xvalue, int yvalue)
    {
        int valueToAdd = ValueOfLocation(new() { X = xvalue, Y = yvalue }, spiral);
        spiral[xvalue, yvalue] = valueToAdd;
        if (valueToAdd > input && result == 0)
            result = valueToAdd;
    }

    [Benchmark]
    public int RunPart2()
    {
        int length = 1000;
        var spiral = new int[length, length];
        int mid = length / 2;
        int result = BuildSpiralInc(mid, int.Parse(Content), 0, 0, spiral);

        return result;
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
