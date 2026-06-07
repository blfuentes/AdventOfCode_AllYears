using AdventOfCode_2016_CSharp.Common;
using BenchmarkDotNet.Attributes;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode_2016_CSharp.day05;

public class Day05(bool isTest = false) : BaseDay("05", isTest)
{
    private static string GenerateHashData(string doorId, int counter)
    {
        string toTest = Tools.ConcatStrNumber(doorId, counter);
        var hashData = MD5.HashData(Encoding.UTF8.GetBytes(toTest)).ToHexLower();
        return hashData;
    }

    #region Part 1
    [Benchmark]
    public string RunPart1()
    {
        string doorId = Content;

        static string BuildPassword(string doorId)
        {
            StringBuilder hash = new();

            int counter = 0;
            while (hash.Length < 8)
            {
                string hashData = GenerateHashData(doorId, counter);
                if (hashData.StartsWith("00000"))
                {
                    hash.Append(hashData[5]);
                }
                counter++;
            }

            return hash.ToString();
        }

        return BuildPassword(doorId);
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
        string doorId = Content;
        static string BuildPassword(string doorId)
        {
            char[] password = new char[8];
            int counter = 0;
            int added = 0;
            while (added < 8)
            {
                string hashData = GenerateHashData(doorId, counter);
                if (hashData.StartsWith("00000") && 
                    int.TryParse(hashData[5].ToString(), out int position) && 
                    position < 8 && password[position] == '\0')
                {
                    password[position] = hashData[6];
                    added++;
                }
                counter++;
            }

            return new string(password);
        }

        return BuildPassword(doorId);
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
