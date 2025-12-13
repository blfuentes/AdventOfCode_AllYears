using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using System.Text.RegularExpressions;

namespace AdventOfCode_2015_CSharp.day06;

public partial class Day06(bool isTest = false) : BaseDay("06", isTest)
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex NumbersRegex();
    private readonly Regex numbersRegex = NumbersRegex();

    #region Part 1
    private static void TurnOp(bool[,] grid, (int X, int Y) from, (int X, int Y) to, bool state)
    {
        for (int x = from.X; x <= to.X; x++)
        {
            for (int y = from.Y; y <= to.Y; y++)
            {
                grid[x, y] = state;
            }
        }
    }

    private static void Toggle(bool[,] grid, (int X, int Y) from, (int X, int Y) to)
    {
        for (int x = from.X; x <= to.X; x++)
        {
            for (int y = from.Y; y <= to.Y; y++)
            {
                grid[x, y] = !grid[x, y];
            }
        }
    }

    [Benchmark]
    public int RunPart1()
    {
        var instructions = File.ReadAllLines(InputPath);
        bool[,] grid = new bool[1000, 1000];
        int[] numbers = new int[4];
        foreach (var instruction in instructions)
        {
            var matches = numbersRegex.Matches(instruction);
            for (int i = 0; i < matches.Count; i++)
            {
                numbers[i] = int.Parse(matches[i].Value);
            }
            switch (instruction)
            {
                case string s when s.StartsWith("turn on"):
                    TurnOp(grid, (numbers[0], numbers[1]), (numbers[2], numbers[3]), true);
                    break;
                case string s when s.StartsWith("turn off"):
                    TurnOp(grid, (numbers[0], numbers[1]), (numbers[2], numbers[3]), false);
                    break;
                case string s when s.StartsWith("toggle"):
                    Toggle(grid, (numbers[0], numbers[1]), (numbers[2], numbers[3]));
                    break;
            };
        }
        return grid.Cast<bool>().Where(c => c).Count();
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
    private static void BrightnessOp(ref long brightness, int[,] grid, (int X, int Y) from, (int X, int Y) to, bool state)
    {
        for (int x = from.X; x <= to.X; x++)
        {
            for (int y = from.Y; y <= to.Y; y++)
            {
                switch (grid[x, y])
                {
                    case int b when b > 0 && !state:
                        grid[x, y]--;
                        brightness--;
                        break;
                    case int b when b == 0 && !state:
                        break;
                    default:
                        grid[x, y] = Math.Max(0, grid[x, y] + (state ? 1 : -1));
                        brightness += state ? 1 : -1;
                        break;
                }
                
            }
        }
    }

    private static void BrightnessToggle(ref long brightness, int[,] grid, (int X, int Y) from, (int X, int Y) to)
    {
        for (int x = from.X; x <= to.X; x++)
        {
            for (int y = from.Y; y <= to.Y; y++)
            {
                grid[x, y] += 2;
                brightness += 2;
            }
        }
    }

    [Benchmark]
    public long RunPart2()
    {
        var instructions = File.ReadAllLines(InputPath);
        int[,] grid = new int[1000, 1000];
        int[] numbers = new int[4];
        long brightness = 0;
        foreach (var instruction in instructions)
        {
            var matches = numbersRegex.Matches(instruction);
            for (int i = 0; i < matches.Count; i++)
            {
                numbers[i] = int.Parse(matches[i].Value);
            }
            switch (instruction)
            {
                case string s when s.StartsWith("turn on"):
                    BrightnessOp(ref brightness, grid, (numbers[0], numbers[1]), (numbers[2], numbers[3]), true);
                    break;
                case string s when s.StartsWith("turn off"):
                    BrightnessOp(ref brightness, grid, (numbers[0], numbers[1]), (numbers[2], numbers[3]), false);
                    break;
                case string s when s.StartsWith("toggle"):
                    BrightnessToggle(ref brightness, grid, (numbers[0], numbers[1]), (numbers[2], numbers[3]));
                    break;
            }
            ;
        }

        return brightness;
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
