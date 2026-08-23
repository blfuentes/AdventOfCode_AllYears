using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day14;

public class Day14(bool isTest = false) : BaseDay("14", isTest)
{
    #region Part 1
    static (bool, char) ThreeInARow(string input)
    {
        for (int i = 0; i < input.Length - 2; i++)
        {
            if (input[i] == input[i + 1] && input[i + 1] == input[i + 2])
                return (true, input[i]);
        }
        return (false, '\0');
    }
    static bool FiveInARow(string input, char check)
    {
        for (int i = 0; i < input.Length - 4; i++)
        {
            if (check == input[i] &&
                input[i] == input[i + 1] &&
                input[i + 1] == input[i + 2] &&
                input[i + 2] == input[i + 3] &&
                input[i + 3] == input[i + 4])
                return true;
        }
        return false;
    }
    [Benchmark]
    public int RunPart1()
    {
        byte[] hashBytes;
        string hash = "";
        bool found = false;
        int initIdx = 0;
        int numOfKeys = 0;
        Dictionary<int, string> computedHashes = [];
        Dictionary<int, string> threeInRowHashes = [];
        Dictionary<int, string> keys = [];

        while (numOfKeys < 64)
        {
            if (!computedHashes.TryGetValue(initIdx, out hash) &&
                !threeInRowHashes.TryGetValue(initIdx, out hash))
            {
                hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(Content + initIdx.ToString()));
                hash = Convert.ToHexString(hashBytes).ToLowerInvariant();
                computedHashes.Add(initIdx, hash);
            }
            (bool foundThree, char value) = ThreeInARow(hash);
            int nextInitIdx = 0;
            if (foundThree)
            {
                found = false;
                threeInRowHashes.TryAdd(initIdx, hash);
                int counter = 1;
                while (counter <= 1000 && !found)
                {
                    int key = initIdx + counter;
                    if (!computedHashes.TryGetValue(key, out hash) &&
                        !threeInRowHashes.TryGetValue(key, out hash))
                    {
                        hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(Content + key.ToString()));
                        hash = Convert.ToHexString(hashBytes).ToLowerInvariant();
                        computedHashes.Add(key, hash);
                    }
                    (foundThree, _) = ThreeInARow(hash);
                    if (foundThree)
                    {
                        threeInRowHashes.TryAdd(key, hash);
                        if (nextInitIdx == 0)
                        {
                            nextInitIdx = key;
                        }
                        if (FiveInARow(hash, value))
                        {
                            // Console.WriteLine($"Adding key {++numOfKeys} at index {initIdx}");
                            keys.TryAdd(initIdx, hash);
                            numOfKeys++;
                            found = true;
                        }
                    }
                    counter++;
                }
            }
            if (numOfKeys < 64)
                initIdx = nextInitIdx > 0 ? nextInitIdx : (initIdx + 1);
        }
        return initIdx;
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
