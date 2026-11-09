using System.Net.Mime;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day17;

public class Day17 : BaseDay
{
    private readonly int _numSteps;

    public Day17(bool isTest = false) : base("17", isTest)
    {
        _numSteps = int.Parse(Content);
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        List<int> buffer = [0];
        int position = 0;

        for (int value = 1; value <= 2017; value++)
        {
            position = (position + _numSteps) % buffer.Count + 1;
            buffer.Insert(position, value);
        }

        return buffer[position + 1];
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
        int position = 0;
        int valueAfterZero = 0;

        for (int value = 1; value <= 50_000_000; value++)
        {
            position = (position + _numSteps) % value + 1;

            if (position == 1)
                valueAfterZero = value;
        }

        return valueAfterZero;
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
