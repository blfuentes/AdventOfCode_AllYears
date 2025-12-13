using BenchmarkDotNet.Attributes;
using System.Security.Cryptography;

namespace AdventOfCode_2015_CSharp.day04;

public class Day04(bool isTest = false) : BaseDay("04", isTest)
{
    MD5 _hasher = MD5.Create();

    private byte[] CreateMD5(string myText)
    {
        return _hasher.ComputeHash(System.Text.Encoding.ASCII.GetBytes(myText ?? ""));
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int current = -1;
        byte[] hash;
        while(true)
        {
            hash = CreateMD5($"{Content}{current}");
            if (hash[0] == hash[1] && hash[0] == 0)
            {
                var tmp = string.Join("", Enumerable.Range(0, hash.Length).Select(i => hash[i].ToString("x2")));
                if (tmp.StartsWith("00000"))
                    break;
            }
            current++;
        }
        return current;
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
        int current = -1;
        byte[] hash;
        do
        {
            current++;
            hash = CreateMD5($"{Content}{current}");
        } while (hash[0] != hash[1] || hash[1] != hash[2] || hash[2] != 0);
        return current;
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
