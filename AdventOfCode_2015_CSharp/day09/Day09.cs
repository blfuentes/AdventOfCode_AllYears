using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day09;

public class Day09(bool isTest = false) : BaseDay("09", isTest)
{
    Dictionary<(string, string), int> Distances = [];

    private int HeldKarpRoute()
    {
        int numOfNodes = Distances.Count / 2;

        int subSetCount = 1 << numOfNodes;
        Dictionary<(string, string), int> DP = [];
        Dictionary<(string, string), string> Parents = [];

        // initialize distances and parents
        foreach(var kvp in Distances)
        {
            DP.Add(kvp.Key, int.MaxValue);
            Parents.Add(kvp.Key, "");
        }

        DP[Distances.Keys.First()] = 0;

        return 0;
    }

    #region Part 1

    [Benchmark]
    public int RunPart1()
    {
        foreach(var line in File.ReadAllLines(InputPath))
        {
            var parts = line.Split(' ');
            Distances.Add((parts[0], parts[1]), int.Parse(parts[4]));
            Distances.Add((parts[2], parts[1]), int.Parse(parts[4]));
        }

        return HeldKarpRoute();
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
