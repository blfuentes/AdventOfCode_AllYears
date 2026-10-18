using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode_2016_CSharp.day19;

public class Day19(bool isTest = false) : BaseDay("19", isTest)
{
    #region Part 1

    [Benchmark]
    public int RunPart1()
    {
        Queue<int> elves = [];
        int numOfElves = int.Parse(Content);
        for (int edx = 1; edx <= numOfElves; edx++)
            elves.Enqueue(edx);

        while (elves.Count > 1 && elves.TryDequeue(out int current))
        {
            elves.TryDequeue(out int next);
            elves.Enqueue(current);
        }

        return elves.Peek();
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
    public long RunPart2()
    {
        int n = int.Parse(Content);
        long powerOfThree = 1;

        while (powerOfThree * 3 <= n)
            powerOfThree *= 3;

        long winner = n == powerOfThree
            ? n
            : n <= 2 * powerOfThree
                ? n - powerOfThree
                : 2L * n - 3 * powerOfThree;

        return winner;
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
