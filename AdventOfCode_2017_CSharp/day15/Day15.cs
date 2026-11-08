using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day15;

public class Day15 : BaseDay
{
    private const int FactorA = 16807;
    private const int FactorB = 48271;
    private const int Divider = 2147483647;

    private static long _seedA;
    private static long _seedB;

    public Day15(bool isTest = false) : base("15", isTest)
    {
        var lines = File.ReadLines(InputPath);
        _seedA = long.Parse(lines.ElementAt(0).Split(" ")[4]);
        _seedB = long.Parse(lines.ElementAt(1).Split(" ")[4]);
    }


    #region Part 1
    static long NextValue(int factor, long value)
    {
        long product = value * factor;

        // Since 2^31 ≡ 1 (mod Divider), fold the high bits down.
        long result = (product & Divider) + (product >> 31);
        result = (result & Divider) + (result >> 31);

        return result == Divider ? 0 : result;
    }

    static long Advance(int factor, long seed, int steps)
    {
        long multiplier = factor;
        long result = seed;

        while (steps > 0)
        {
            if ((steps & 1) != 0)
                result = result * multiplier % Divider;

            multiplier = multiplier * multiplier % Divider;
            steps >>= 1;
        }

        return result;
    }

    [Benchmark]
    public static int RunPart1()
    {
        const int times = 40_000_000;
        const int chunkSize = 1_000_000;
        int[] matches = new int[(times + chunkSize - 1) / chunkSize];
        long seedA = _seedA;
        long seedB = _seedB;

        Parallel.For(0, matches.Length, chunk =>
        {
            int start = chunk * chunkSize;
            int end = Math.Min(start + chunkSize, times);
            long valueA = Advance(FactorA, seedA, start);
            long valueB = Advance(FactorB, seedB, start);
            int count = 0;

            for (int i = start; i < end; i++)
            {
                valueA = NextValue(FactorA, valueA);
                valueB = NextValue(FactorB, valueB);
                if ((valueA & 0xFFFF) == (valueB & 0xFFFF))
                    count++;
            }

            matches[chunk] = count;
        });

        return matches.Sum();
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
            value = NextValue(factor, value);
        }
        while ((value & (remainder - 1)) != 0);

        return value;
    }

    static void GenerateFiltered(int factor, int remainder, long seed, ushort[] values)
    {
        long value = seed;
        for (int i = 0; i < values.Length; i++)
        {
            value = NextValue(factor, remainder, value);
            values[i] = (ushort)(value & 0xFFFF);
        }
    }

    [Benchmark]
    public static int RunPart2()
    {
        const int times = 5_000_000;
        ushort[] valuesA = new ushort[times];
        ushort[] valuesB = new ushort[times];
        long seedA = _seedA;
        long seedB = _seedB;

        Parallel.Invoke(
            () => GenerateFiltered(FactorA, 4, seedA, valuesA),
            () => GenerateFiltered(FactorB, 8, seedB, valuesB));

        int matches = 0;
        for (int i = 0; i < times; i++)
        {
            if (valuesA[i] == valuesB[i])
                matches++;
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
