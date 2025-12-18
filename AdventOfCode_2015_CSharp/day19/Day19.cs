using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day19;

public class Day19(bool isTest = false) : BaseDay("19", isTest)
{
    record Replacement
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    IEnumerable<int> IndexOf(string text, string part)
    {
        for (int i = 0; i < text.Length;)
        {
            if (text[i..].StartsWith(part))
            {
                yield return i;
            }
            i++;
        }
        yield break;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        var lines = File.ReadLines(InputPath).ToList();
        IEnumerable<Replacement> replacements =
            lines[..^2]
            .Select(_ => new Replacement { From = _.Split(" => ")[0], To = _.Split(" => ")[1] });
        string initial = lines[^1];
        HashSet<string> molecules = [];
        foreach (var replacement in replacements)
        {
            List<int> allidx = [.. IndexOf(initial, replacement.From)];
            var texted = allidx.Select(_ => _.ToString());
            foreach (var idx in allidx)
            {
                molecules.Add(initial[..idx] + replacement.To + initial[(idx + replacement.From.Length)..]);
            }
        }
        return molecules.Count;
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

    static int ShortestTransformation(string currentState, string target, IEnumerable<Replacement> replacements)
    {
        int counter = 0;
        while (currentState != target)
        {
            foreach (var replacement in replacements)
            {
                var idx = currentState.IndexOf(replacement.To);
                if (idx >= 0)
                {
                    currentState = currentState[..idx] + replacement.From + currentState[(idx + replacement.To.Length)..];
                    counter++;
                }
            }
        }
        return counter;
    }

    [Benchmark]
    public int RunPart2()
    {
        var lines = File.ReadLines(InputPath).ToList();
        IEnumerable<Replacement> replacements =
            lines[..^2]
            .Select(_ => new Replacement { From = _.Split(" => ")[0], To = _.Split(" => ")[1] });

        return ShortestTransformation(lines[^1], "e", replacements);
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
