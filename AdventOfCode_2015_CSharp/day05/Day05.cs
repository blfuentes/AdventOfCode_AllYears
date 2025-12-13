using BenchmarkDotNet.Attributes;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AdventOfCode_2015_CSharp.day05;

public partial class Day05(bool isTest = false) : BaseDay("05", isTest)
{
    #region Part 1
    [GeneratedRegex("[aeiou]")]
    private static partial Regex ThreeVowelsRegex();

    private readonly Regex threeVowels = ThreeVowelsRegex();
    bool HasThreeVowels(string word)
    {
        return threeVowels.Count(word) >= 3;
    }

    private static bool HasRepeatedChar(string mov)
    {
        for (int i = 0; i < mov.Length - 1; i++)
        {
            if (mov[i] == mov[i + 1])
                return true;
        }
        return false;
    }

    List<string> notValid = ["ab", "cd", "pq", "xy"];
    bool DoesNotContain(string mov)
    {
        return !notValid.Any(p => mov.Contains(p));
    }

    [Benchmark]
    public int RunPart1()
    {
        var words = File.ReadLines(InputPath);
        ConcurrentBag<string> valid = [];
        Parallel.ForEach(words, (word) =>
        {
            if (HasThreeVowels(word) && HasRepeatedChar(word) && DoesNotContain(word))
                valid.Add(word);
        });
        return valid.Count;
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

    private static bool TwoNotOverlapped(string word)
    {
        for (int i = 0; i < word.Length - 3; i++)
        {
            for (int j = i + 2; j < word.Length - 1; j++)
            {
                if (word[i] == word[j] && word[i + 1] == word[j + 1])
                    return true;
            }
        }

        return false;
    }

    private static bool RepeatedWithMid(string word)
    {
        for (int i = 0; i < word.Length - 2; i++)
        {
            if (word[i] == word[i + 2])
                return true;
        }
        return false;
    }

    [Benchmark]
    public int RunPart2()
    {
        var words = File.ReadLines(InputPath);
        ConcurrentBag<string> valid = [];
        Parallel.ForEach(words, (word) =>
        {
            if (TwoNotOverlapped(word) && RepeatedWithMid(word))
                valid.Add(word);
        });
        return valid.Count;
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
