using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day16;

public partial class Day16 : BaseDay
{
    private readonly string[] _moves;

    public Day16(bool isTest = false) : base("16", isTest)
    {
        _moves = [.. Content.Split(",")];
    }

    static void Spin(string move, char[] programs)
    {
        int offset = int.Parse(move.AsSpan(1)) % programs.Length;

        Array.Reverse(programs);
        Array.Reverse(programs, 0, offset);
        Array.Reverse(programs, offset, programs.Length - offset);
    }

    static void Exchange(string move, char[] programs)
    {
        int[] numbers = [.. NumbersRegex().Matches(move).Select(m => int.Parse(m.Value))];
        (programs[numbers[0]], programs[numbers[1]]) = (programs[numbers[1]], programs[numbers[0]]);
    }

    static void Partner(string move, char[] programs)
    {
        (int fromIdx, int toIdx) = (programs.IndexOf(move.AsSpan(1, 1)), programs.IndexOf(move.AsSpan(3, 1)));
        (programs[fromIdx], programs[toIdx]) = (programs[toIdx], programs[fromIdx]);
    }

    #region Part 1
    [Benchmark]
    public string RunPart1()
    {
        char[] programs = IsTest ? [.. "abcde"] : [.. "abcdefghijklmnop"];
        foreach (var move in _moves)
        {
            switch (move[0])
            {
                case 's':
                    Spin(move, programs);
                    break;
                case 'x':
                    Exchange(move, programs);
                    break;
                case 'p':
                    Partner(move, programs);
                    break;
            }
        }

        return string.Join("", programs);
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
    public string RunPart2()
    {
        char[] programs = IsTest ? [.. "abcde"] : [.. "abcdefghijklmnop"];
        long times = 1_000_000_000L;

        var initial = new string(programs);
        var seen = new Dictionary<string, long> { [initial] = 0 };
        var states = new List<string> { initial };

        for (long round = 1; round <= times; round++)
        {
            foreach (var move in _moves)
            {
                switch (move[0])
                {
                    case 's':
                        Spin(move, programs);
                        break;
                    case 'x':
                        Exchange(move, programs);
                        break;
                    case 'p':
                        Partner(move, programs);
                        break;
                }
            }

            var state = new string(programs);

            if (seen.TryGetValue(state, out long cycleStart))
            {
                long cycleLength = round - cycleStart;
                long targetRound = cycleStart + (times - cycleStart) % cycleLength;
                return states[(int)targetRound];
            }

            seen.Add(state, round);
            states.Add(state);
        }

        return new string(programs);
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
