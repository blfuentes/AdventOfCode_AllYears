using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day16;

public class Day16(bool isTest = false) : BaseDay("16", isTest)
{
    #region Part 1

    static byte[] Step(ReadOnlySpan<byte> data)
    {
        // create copy and reverse
        var b = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
            b[i] = (byte)(1 - data[data.Length - 1 - i]);

        var result = new byte[data.Length * 2 + 1];
        data.CopyTo(result);
        result[data.Length] = 0;
        b.CopyTo(result.AsSpan(data.Length + 1));

        return result;
    }

    static byte[] GenerateData(ReadOnlySpan<byte> data, int required)
    {
        var generated = data.ToArray();
        while (generated.Length < required)
            generated = Step(generated);

        return generated;
    }

    static byte[] CalculateCheckSum(string input, int required)
    {
        var data = new byte[input.Length];
        for (var i = 0; i < input.Length; i++)
            data[i] = input[i] == '1' ? (byte)1 : (byte)0;

        var used = GenerateData(data, required);
        if (used.Length > required)
        {
            var trimmed = new byte[required];
            Array.Copy(used, trimmed, required);
            used = trimmed;
        }

        while (used.Length % 2 == 0)
        {
            var next = new byte[used.Length / 2];
            for (var i = 0; i < used.Length; i += 2)
                next[i / 2] = (byte)(used[i] == used[i + 1] ? 1 : 0);
            used = next;
        }

        return used;
    }



    [Benchmark]
    public string RunPart1()
    {
        var result = CalculateCheckSum(Content, 272);
        return string.Join("", result);
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
        var result = CalculateCheckSum(Content, 35651584);
        return string.Join("", result);
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
