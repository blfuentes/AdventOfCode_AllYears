using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode_2017_CSharp.day10;

public partial class Day10(bool isTest) : BaseDay("10", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int[] lengths = [.. NumbersRegex().Matches(Content).Select(c => int.Parse(c.Value))];
        int numbersSize = IsTest ? 5 : 256;
        int[] numbers = [.. Enumerable.Range(0, numbersSize)];
        (int currentPos, int skipSize) = (0, 0);
        foreach (var length in lengths)
        {
            if (length > numbers.Length)
                continue;

            for (int c = 0; c < length / 2; c++)
            {
                int swapfrom = (currentPos + c) % numbers.Length;
                int swapto = (currentPos - c + length - 1) % numbers.Length;
                (numbers[swapfrom], numbers[swapto]) = (numbers[swapto], numbers[swapfrom]);
            }
            currentPos = (currentPos + length + skipSize) % numbers.Length;
            skipSize++;
        }

        return numbers[0] * numbers[1];
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
        int[] lengths = [.. Content.Select(c => (int)c).Concat([17, 31, 73, 47, 23])];
        int numbersSize = 256;
        int[] numbers = [.. Enumerable.Range(0, numbersSize)];
        (int currentPos, int skipSize) = (0, 0);
        for (var round = 0; round < 64; round++)
        {
            foreach (var length in lengths)
            {
                if (length > numbers.Length)
                    continue;

                for (int c = 0; c < length / 2; c++)
                {
                    int swapfrom = (currentPos + c) % numbers.Length;
                    int swapto = (currentPos - c + length - 1) % numbers.Length;
                    (numbers[swapfrom], numbers[swapto]) = (numbers[swapto], numbers[swapfrom]);
                }
                currentPos = (currentPos + length + skipSize) % numbers.Length;
                skipSize++;
            }
        }
        var denseHash = numbers.Chunk(16).Select(chunk =>
        {
            return chunk.Aggregate(0, (xor, value) => xor ^ value).ToString("X2").ToLowerInvariant();
        });
        return string.Join("", denseHash);
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumbersRegex();
}
