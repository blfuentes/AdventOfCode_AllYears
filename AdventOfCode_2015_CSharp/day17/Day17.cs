using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day17;

public class Day17(bool isTest = false) : BaseDay("17", isTest)
{
    #region Part 1

    static IEnumerable<List<(int, int)>> AllCombinations(List<int> initial)
    {
        int totalSpoons = 25;

        IEnumerable<List<(int, int)>> FindCombination(List<int> available, List<(int, int)> current, int remaining)
        {
            if (remaining == 0)
                yield return current;
            else if (remaining < 0)
                yield break;


            for (int i = 0; i < available.Count(); i++)
            {
                foreach (var result in FindCombination(
                    [.. Enumerable.Concat(available[..i], available[(i + 1)..])],
                    [.. current, (i, available[i])],
                    remaining - available[i]))
                {
                    yield return result;
                }
            }
        }

        return FindCombination(initial, [], totalSpoons);
    }

    [Benchmark]
    public int RunPart1()
    {
        var containers = File.ReadAllLines(InputPath).Select(int.Parse).ToList();
        var combinations = AllCombinations(containers);
        return combinations.DistinctBy(c => {
            c.Sort();
            return string.Join("_", c.Select(_ => _.Item1.ToString()));
        }).Count();
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
        return Content.Length;
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
