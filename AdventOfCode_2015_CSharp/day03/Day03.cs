using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day03;

public class Day03(bool isTest = false) : BaseDay("03", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        (int initX, int initY) = (0, 0);
        HashSet<(int, int)> visited = [];
        visited.Add((initX, initY));
        foreach(var mov in Content)
        {
            switch (mov)
            {
                case '^': initY--; break;
                case 'v': initY++; break;
                case '<': initX--; break;
                case '>': initX++; break;
            }
            visited.Add((initX, initY));
        }

        return visited.Count;
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
        List<(int x, int y)> dealers = [(0, 0), (0, 0)];
        HashSet<(int, int)> visited = [];
        visited.Add((0, 0));
        foreach (var idx in Enumerable.Range(0, Content.Length))
        {
            switch (Content[idx])
            {
                case '^': dealers[idx % 2] = (dealers[idx % 2].x, dealers[idx % 2].y - 1); break;
                case 'v': dealers[idx % 2] = (dealers[idx % 2].x, dealers[idx % 2].y + 1); ; break;
                case '<': dealers[idx % 2] = (dealers[idx % 2].x - 1, dealers[idx % 2].y); break;
                case '>': dealers[idx % 2] = (dealers[idx % 2].x +  1, dealers[idx % 2].y); break;
            }
            visited.Add(dealers[idx % 2]);
        }

        return visited.Count;
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
