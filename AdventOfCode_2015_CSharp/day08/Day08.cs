using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace AdventOfCode_2015_CSharp.day08;

public class Day08(bool isTest = false) : BaseDay("08", isTest)
{
    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        var lines = File.ReadAllLines(InputPath);
        int[] lengths = new int[lines.Length];
        int[] counter = new int[lines.Length];
        char[] toScapeSingle = ['\\', '"', 'x'];
        //var replacePattern = @"\\x(\w){2}";
        for (int i = 0; i < lines.Length; i++)
        {
            lengths[i] = lines[i].Length;
            // Cleaner but slower...
            //counter[i] = Regex.Replace(lines[i][1..^1]
            //    .Replace("\\\"", ".")
            //    .Replace("\\\\", "."), replacePattern, ".").Length;
            //var current = lines[i].ToCharArray()[1..^1];
            var current = lines[i][1..^1].AsSpan();
            bool escape = false;
            byte escapeByte = 0;
            for (int j = 0; j < current.Length; j++)
            {
                var c = current[j];
                if (escape)
                {
                    if (toScapeSingle.Contains(c) || escapeByte > 0)
                    {
                        if (c == 'x')
                        {
                            escapeByte = 1;
                            continue;
                        }
                        else
                        {
                            if (escapeByte > 0)
                            {
                                escapeByte++;
                                if (escapeByte == 3)
                                {
                                    escape = false;
                                    counter[i]++;
                                    escapeByte = 0;
                                }
                            }
                            else
                            {
                                counter[i]++;
                                escape = false;
                            }
                        }
                    }
                }
                else
                {
                    if (c == '\\')
                        escape = true;
                    else
                        counter[i]++;
                }
            }
        }

        return lengths.Sum() - counter.Sum();
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
        var lines = File.ReadAllLines(InputPath);
        int[] lengths = new int[lines.Length];
        int[] counter = new int[lines.Length];
        char[] toScapeSingle = ['\\', '"'];
        for (int i = 0; i < lines.Length; i++)
        {
            lengths[i] = lines[i].Length;
            counter[i] = 2;
            var current = lines[i].AsSpan();
            for (int j = 0; j < current.Length; j++)
            {
                if (toScapeSingle.Contains(current[j]))
                    counter[i] += 2;
                else
                    counter[i] += 1;
            }
        }
        return counter.Sum() - lengths.Sum();
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
