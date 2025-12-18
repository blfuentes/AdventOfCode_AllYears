using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day17;

public class Day17(bool isTest = false) : BaseDay("17", isTest)
{
    static IEnumerable<HashSet<int>> AllCombinations(List<int> initial)
    {
        int totalSize = 150;

        static IEnumerable<HashSet<int>> FindCombination(List<int> available, HashSet<int> current, int remaining)
        {
            if (remaining == 0)
            {
                //Console.WriteLine($"Found: {string.Join("-", current.Select(_ => _.Id.ToString()))}");
                yield return current;
            }
            else if (remaining < 0)
                yield break;


            for (int i = 0; i < available.Count; i++)
            {
                foreach (var result in FindCombination(
                    [.. available[(i + 1)..]],
                    [.. current, available[i]],
                    remaining - available[i]))
                {
                    yield return result;
                }
            }
        }

        return FindCombination(initial, [], totalSize);
    }

    #region Part 1

    [Benchmark]
    public int RunPart1()
    {
        var containers = File.ReadAllLines(InputPath).Select(int.Parse);
        return AllCombinations([.. containers]).Count();
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
        var containers = File.ReadAllLines(InputPath).Select(int.Parse);
        return AllCombinations([..containers])
                .GroupBy(s => s.Count)
                .MinBy(_ => _.Key)!.Count();
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
