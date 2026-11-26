using System.Text;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2018_CSharp.day05;

public class Day05(bool isTest = false) : BaseDay("05", isTest)
{
    static bool CanReact(char a, char b)
    {
        return
            a == (b - 32) || // lowercase -> uppercase
            a == (b + 32); // uppercase -> lowercase
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int idx = 1;
        List<char> polymer = [];
        polymer.Add(Content[0]);
        while (idx < Content.Length)
        {
            if (CanReact(polymer[^1], Content[idx]))
            {
                polymer.RemoveAt(polymer.Count - 1);
            }
            else
            {
                polymer.Add(Content[idx]);
            }
            idx++;
        }
        return polymer.Count;
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
        int minSoFar = int.MaxValue;
        for (int cIdx = 0; cIdx < 35; cIdx++)
        {
            int idx = 1;
            StringBuilder filteredContent = new();
            foreach (char c in Content)
            {
                if (c == 'A' + cIdx || c == 'A' + 32 + cIdx)
                    continue;
                else
                    filteredContent.Append(c);
            }
            string content = filteredContent.ToString();
            List<char> polymer = [];
            polymer.Add(content[0]);
            while (idx < content.Length)
            {
                if (polymer.Count > 0 && CanReact(polymer[^1], content[idx]))
                {
                    polymer.RemoveAt(polymer.Count - 1);
                }
                else
                {
                    polymer.Add(content[idx]);
                }
                idx++;
            }

            minSoFar = Math.Min(polymer.Count, minSoFar);
        }

        return minSoFar;
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
