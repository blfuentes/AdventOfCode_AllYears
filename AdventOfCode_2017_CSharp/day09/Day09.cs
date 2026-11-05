using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode_2017_CSharp.day09;

public class Day09(bool isTest = false) : BaseDay("09", isTest)
{
    enum Mode
    {
        NotGarbage,
        Garbage,
        Cancelled
    }
    record State(int Level = 0, Mode Mode = Mode.NotGarbage, int Score = 0, int Garbage = 0);

    static State Step(State current, char next) => (current.Mode, next) switch
    {
        (Mode.Garbage, '!') => current with { Mode = Mode.Cancelled },
        (Mode.Garbage, '>') => current with { Mode = Mode.NotGarbage },
        (Mode.Garbage, _) => current with { Garbage = current.Garbage + 1 },

        (Mode.Cancelled, _)
            or (Mode.NotGarbage, '<') => current with { Mode = Mode.Garbage },

        (Mode.NotGarbage, '{') => current with { Level = current.Level + 1 },
        (Mode.NotGarbage, '}') => current with
        {
            Level = current.Level - 1,
            Score = current.Score + current.Level
        },

        _ => current
    };

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        var currentState = Content.Aggregate(new State(), Step);

        return currentState.Score;
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
        var currentState = Content.Aggregate(new State(), Step);

        return currentState.Garbage;
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
