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
        var generated = Step(data);
        while (generated.Length < required) generated = Step(generated);

        return generated;
    }

    static byte[] CalculateCheckSum(string input, int required)
    {
        byte[] data = GenerateData(
            [.. input.Select(c => c == '1' ? (byte)1 : (byte)0)],
            required);

        byte[] used = [.. data[..required]];

        List<byte> checksum = [];
        while (checksum.Count % 2 == 0)
        {
            // Console.WriteLine($"CheckSum length: {checksum.Count}");
            checksum.Clear();
            foreach (var chunk in used.Chunk(2))
            {
                checksum.Add(chunk[0] == chunk[1] ? (byte)1 : (byte)0);
            }
            used = [.. checksum];
        }

        return [.. checksum];
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
