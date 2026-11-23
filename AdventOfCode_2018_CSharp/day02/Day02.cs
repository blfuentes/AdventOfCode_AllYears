using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace AdventOfCode_2018_CSharp.day02;

public class Day02(bool isTest = false) : BaseDay("02", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int hasTwo = 0, hasThree = 0;
        string[] ids = File.ReadAllLines(InputPath);

        foreach (var id in ids)
        {
            var groups = id.ToCharArray().CountBy(a => a);
            if (groups.Any(g => g.Value == 2))
                hasTwo++;
            if (groups.Any(g => g.Value == 3))
                hasThree++;
        }

        return hasTwo * hasThree;
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
        var lines = File.ReadAllLines(InputPath);
        bool found = false;
        foreach (var line1 in lines)
        {
            if (found) break;
            foreach (var line2 in lines)
            {
                if (found) break;
                if (line1 != line2)
                {
                    List<char> common = [];
                    int count = 0;
                    foreach (var (l1, l2) in line1.Zip(line2))
                    {
                        if (count > 1)
                            break;
                        if (l1 != l2)
                        {
                            count++;
                        }
                        else
                        {
                            common.Add(l1);
                        }
                    }
                    if (count == 1)
                    {
                        return new string([.. common]);
                    }
                }
            }
        }
        return "";
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
