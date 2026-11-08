using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day15;

public class Day15 : BaseDay
{
    private const int FactorA = 16807;
    private const int FactorB = 48271;
    private const int Divider = 2147483647;

    private static long _valueA;
    private static long _valueB;

    public Day15(bool isTest = false) : base("15", isTest)
    {
        var lines = File.ReadLines(InputPath);
        _valueA = long.Parse(lines.ElementAt(0).Split(" ")[4]);
        _valueB = long.Parse(lines.ElementAt(1).Split(" ")[4]);
    }


    #region Part 1
    static long NextValue(int factor, long value)
    {
        return value * factor % Divider;
    }

    [Benchmark]
    public static int RunPart1()
    {
        int times = 40_000_000;
        int counter = 0;
        int matches = 0;

        while (counter < times)
        {
            _valueA = NextValue(FactorA, _valueA);
            _valueB = NextValue(FactorB, _valueB);
            if ((_valueA & 0xFFFF) == (_valueB & 0xFFFF))
                matches++;
            counter++;
        }
        return matches;
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
    static long NextValue(int factor, int remainder, long value)
    {
        do
        {
            value = (value * factor) % Divider;
        }
        while (value % remainder != 0);

        return value;
    }

    [Benchmark]
    public static int RunPart2()
    {
        int times = 5_000_000;
        (int remainderA, int remainderB) = (4, 8);

        int counter = 0;
        int matches = 0;

        while (counter < times)
        {
            _valueA = NextValue(FactorA, remainderA, _valueA);
            _valueB = NextValue(FactorB, remainderB, _valueB);

            if ((_valueA & 0xFFFF) == (_valueB & 0xFFFF))
                matches++;
            counter++;
        }
        return matches;
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
