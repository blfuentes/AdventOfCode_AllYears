using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace AdventOfCode_2016_CSharp.day07;

public partial class Day07(bool isTest = false) : BaseDay("07", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        static bool HasABBA(string input)
        {
            for (int i = 0; i < input.Length - 3; i++)
            {
                if (input[i] != input[i + 1] && 
                    input[i] == input[i + 3] && input[i + 1] == input[i + 2])
                    return true;
            }
            return false;

        }
        static bool SupportTLS(string ip)
        {
            var hypernetSections = HypernetRegex().Matches(ip).Select(m => m.Value);
            if (hypernetSections.Any(HasABBA))
            {
                return false;
            }
            return HasABBA(ip);
        }

        return File.ReadAllLines(InputPath)
            .Count(SupportTLS);
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
        static bool IsABA(string input)
        {
            return input[0] != input[1] && input[0] == input[2];
        }
        
        static bool IsBAB(string aba, string input)
        {
            for(int i = 0; i < input.Length - 3; i++)
            {
                if (input[i] == aba[1] && input[i + 1] == aba[0] && input[i + 2] == aba[1])
                    return true;
            }
            return false;
        }

        static bool SupportSSL(string ip)
        {
            var hypernetSections = HypernetRegex().Matches(ip).Select(m => m.Value);
            var filtered = HypernetRegex().Replace(ip, "[]");
            for(int i = 0; i < filtered.Length - 2; i++)
            {
                if (IsABA(filtered.Substring(i, 3)) && hypernetSections.Any(h => IsBAB(filtered.Substring(i, 3), h)))
                    return true;
            }
            return false;
        }

        return File.ReadAllLines(InputPath)
            .Count(SupportSSL);
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }

    [GeneratedRegex(@"\[(\w)*\]")]
    private static partial Regex HypernetRegex();
    #endregion
}
