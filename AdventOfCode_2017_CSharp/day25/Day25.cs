using System.Globalization;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;

namespace AdventOfCode_2017_CSharp.day25;

public partial class Day25 : BaseDay
{
    private readonly char _startState;
    private readonly int _steps;

    public record Behavior(int Value, int Dir, char NextState);

    public record State(char Id, Behavior When0, Behavior When1);

    private readonly Dictionary<char, State> _states;

    public Day25(bool isTest) : base("25", isTest)
    {
        var lines = File.ReadAllLines(InputPath);

        var headerSection = lines[..2];
        List<string[]> sectionStates = [.. lines[3..].Chunk(10)];

        _startState = headerSection[0][^2];
        _steps = int.Parse(NumberRegex().Match(headerSection[1]).Value);

        _states = [];
        foreach (var section in sectionStates)
        {
            char id = section[0][^2];

            // When 0
            int valueWhen0 = int.Parse(NumberRegex().Match(section[2]).Value);
            int dirWhen0 = section[3].Contains("right") ? 1 : -1;
            char stateWhen0 = section[4][^2];
            Behavior when0 = new(valueWhen0, dirWhen0, stateWhen0);

            // When 1
            int valueWhen1 = int.Parse(NumberRegex().Match(section[6]).Value);
            int dirWhen1 = section[7].Contains("right") ? 1 : -1;
            char stateWhen1 = section[8][^2];
            Behavior when1 = new(valueWhen1, dirWhen1, stateWhen1);

            _states.Add(id, new State(id, when0, when1));
        }

    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        HashSet<int> posWithOnes = [];
        (char currentState, int currentPos) = (_startState, 0);

        int step = 0;
        while (step++ < _steps)
        {
            var stateRules = _states[currentState];
            if (posWithOnes.Contains(currentPos))
            {
                if (stateRules.When1.Value == 1)
                    posWithOnes.Add(currentPos);
                else
                    posWithOnes.Remove(currentPos);

                currentState = stateRules.When1.NextState;
                currentPos += stateRules.When1.Dir;
            }
            else
            {
                if (stateRules.When0.Value == 1)
                    posWithOnes.Add(currentPos);
                else
                    posWithOnes.Remove(currentPos);

                currentState = stateRules.When0.NextState;
                currentPos += stateRules.When0.Dir;
            }
        }

        return posWithOnes.Count;
    }

    public override string SolvePart1()
    {
        StopWatch.Start();
        var result = RunPart1();
        StopWatch.Stop();
        return $"Final result Day {Day} part 1: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion    

    [GeneratedRegex(@"\d+")]
    private partial Regex NumberRegex();

    public override string SolvePart2()
    {
        throw new NotImplementedException();
    }
}
