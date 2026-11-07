using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Mathematics;

namespace AdventOfCode_2017_CSharp.day13;

public partial class Day13 : BaseDay
{
    private readonly Dictionary<int, int> _layers;
    public Day13(bool isTest = false) : base("13", isTest)
    {
        _layers = [];
        foreach (var line in File.ReadAllLines(InputPath))
        {
            int[] numbers = [.. NumbersRegex().Matches(line).Select(c => int.Parse(c.Value))];
            _layers.Add(numbers[0], numbers[1]);
        }
    }

    static int PositionAt(int t, int size)
    {
        int p = t % (2 * (size - 1));
        return p < size ? p : 2 * (size - 1) - p;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int tripSeverity = 0;
        foreach (var kvp in _layers)
        {
            var posAtTime = PositionAt(kvp.Key, kvp.Value);
            if (posAtTime == 0)
                tripSeverity += kvp.Key * kvp.Value;
        }
        return tripSeverity;
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
        int delay = 0;
        bool caught = true;
        while (caught)
        {
            caught = false;
            foreach (var kvp in _layers)
            {
                var posAtTime = PositionAt(kvp.Key + delay, kvp.Value);
                if (posAtTime == 0)
                {
                    caught = true;
                    break;
                }
            }
            delay++;
        }

        return delay - 1;
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumbersRegex();
}
