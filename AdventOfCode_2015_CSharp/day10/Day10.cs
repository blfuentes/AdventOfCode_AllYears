using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day10;

public class Day10(bool isTest = false) : BaseDay("10", isTest)
{
    #region Part 1

    private int[] LookAndSay(int[] number)
    {
        int repeats = 0;
        int current = 0;
        int prev = -1;
        List<int> newNumbers = [];
        for (int i = 0; i < number.Length; i++)
        {
            current = number[i];
            if (i > 0 && current != prev)
            {
                newNumbers.AddRange(repeats, prev);
                repeats = 0;
            }
            ++repeats;
            prev = current;
        }
        newNumbers.AddRange(repeats, current);
        return [.. newNumbers];
    }

    [Benchmark]
    public int RunPart1()
    {
        int reps = 40;

        int[] number = [.. Content.Select(c => c - '0')];
        while (reps-- > 0)
        {
            number = LookAndSay(number);
        }

        return number.Length;
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
        int reps = 50;

        int[] number = [.. Content.Select(c => c - '0')];
        while (reps-- > 0)
        {
            number = LookAndSay(number);
        }
        return number.Length;
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
