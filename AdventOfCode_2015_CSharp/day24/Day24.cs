using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day24;

public class Day24(bool isTest = false) : BaseDay("24", isTest)
{
    static long QuantumEntaglement(IEnumerable<long> packages)
    {
        return packages.Aggregate(1L, (a, b) => a * b);
    }

    static IEnumerable<IEnumerable<T>> Comb<T>(int n, IEnumerable<T> list)
    {
        if (n == 0)
        {
            yield return Enumerable.Empty<T>();
            yield break;
        }
        
        var enumerated = list.ToList();
        if (enumerated.Count == 0)
            yield break;
        
        var x = enumerated[0];
        var xs = enumerated.Skip(1);
        
        foreach (var combo in Comb(n - 1, xs))
            yield return combo.Prepend(x);
        
        foreach (var combo in Comb(n, xs))
            yield return combo;
    }

    #region Part 1
    [Benchmark]
    public long RunPart1()
    {
        var weights = File.ReadLines(InputPath).Select(long.Parse);
        var targetWeight = weights.Sum() / 3;

        return Enumerable.Range(2, weights.Count() / 3 - 2)
            .SelectMany(n => Comb(n, weights))
            .Where(combo => combo.Sum() == targetWeight)
            .GroupBy(combo => combo.Count())
            .MinBy(group => group.Key)!
            .Select(QuantumEntaglement)
            .Min();
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
    public long RunPart2()
    {
        var weights = File.ReadLines(InputPath).Select(long.Parse);
        var targetWeight = weights.Sum() / 4;

        return Enumerable.Range(2, weights.Count() / 4 - 2)
            .SelectMany(n => Comb(n, weights))
            .Where(combo => combo.Sum() == targetWeight)
            .GroupBy(combo => combo.Count())
            .MinBy(group => group.Key)!
            .Select(QuantumEntaglement)
            .Min();
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
