using AdventOfCode_2016_CSharp.Common;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day02;


public class Day02(bool isTest = false) : BaseDay("02", isTest)
{
    #region Part 1
    Dictionary<(int row, int col), int> KeypadMapping = new()
    {
        { (-1, -1), 1 },
        { (-1, 0), 2 },
        { (-1, 1), 3 },
        { (0, -1), 4 },
        { (0, 0), 5 },
        { (0, 1), 6 },
        { (1, -1), 7 },
        { (1, 0), 8 },
        { (1, 1), 9 },
    };
    [Benchmark]
    public long RunPart1()
    {
        var content = File.ReadAllLines(InputPath);

        GridPos currentPos = new(0, 0);
        int maxExp = content.Length - 1;
        long code = 0;

        foreach(var line in content)
        {
            foreach(var move in line)
            {
                Direction tmp = new(move);
                currentPos += tmp.ToOffset();
                GridPos.Normalize(ref currentPos);
            }
            code += KeypadMapping[currentPos] * (long)Math.Pow(10, maxExp--);
        }
        return code;
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
    Dictionary<(int row, int col), char> KeypadNewMapping = new()
    {
        { (-2, 0), '1' },
        { (-1, -1), '2' },
        { (-1, 0), '3' },
        { (-1, 1), '4' },
        { (0, -2), '5' },
        { (0, -1), '6' },
        { (0, 0), '7' },
        { (0, 1), '8' },
        { (0, 2), '9' },
        { (1, -1), 'A' },
        { (1, 0), 'B' },
        { (1, 1), 'C' },
        { (2, 0), 'D' }
    };
    [Benchmark]
    public string RunPart2()
    {
        GridPos currentPos = new(0, -2);
        var sb = new System.Text.StringBuilder();

        foreach (var line in File.ReadLines(InputPath))
        {
            ReadOnlySpan<char> span = line.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                Direction tmp = new(span[i]);
                GridPos tmpPos = currentPos + tmp.ToOffset();
                if (KeypadNewMapping.ContainsKey(tmpPos))
                {
                    currentPos = tmpPos;
                }
            }
            sb.Append(KeypadNewMapping[currentPos]);
        }
        return sb.ToString();
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
