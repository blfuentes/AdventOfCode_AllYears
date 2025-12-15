using BenchmarkDotNet.Attributes;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AdventOfCode_2015_CSharp.day12;

public partial class Day12(bool isTest = false) : BaseDay("12", isTest)
{
    #region Part 1
    [GeneratedRegex("-?\\d+")]
    private static partial Regex Numbers();
    private readonly Regex numbers = Numbers();

    [Benchmark]
    public int RunPart1()
    {
        return numbers.Matches(Content).Select(m => int.Parse(m.Value)).Sum();
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
        dynamic document = JsonDocument.Parse(Content);
        int result = 0;
        void WalkJson(JsonElement expando)
        {
            if (expando.ValueKind == JsonValueKind.Object)
            {
                var nestedLevel = expando.EnumerateObject();

                if (nestedLevel.Any(n => n.Value.ToString() == "red"))
                    return;

                foreach (var nested in nestedLevel)
                {
                    ExtractValue(nested.Value);
                }
            }
            else
            {
                foreach (var nested in expando.EnumerateArray())
                {
                    ExtractValue(nested);
                }
            }

            void ExtractValue(JsonElement nested)
            {
                if (nested.ValueKind == JsonValueKind.Array)
                {
                    foreach (var child in nested.EnumerateArray())
                    {
                        if (child.ValueKind == JsonValueKind.Object || child.ValueKind == JsonValueKind.Array)
                        {
                            WalkJson(child);
                        }
                        else
                        {
                            if (child.ValueKind == JsonValueKind.Number)
                            {
                                result += (int)child.GetInt32();
                            }
                        }
                    }
                }
                else if (nested.ValueKind == JsonValueKind.Number)
                {
                    result += (int)nested.GetInt32();
                }
                else if (nested.ValueKind == JsonValueKind.Object)
                {
                    WalkJson(nested);
                }
            }
        }
        WalkJson(document.RootElement);
        return result;
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
