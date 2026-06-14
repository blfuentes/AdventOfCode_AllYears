using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day10;

public class Day10(bool isTest = false) : BaseDay("10", isTest)
{
    enum DestType { Bot, Output };

    record BaseInstruction();
    record BotInstruction : BaseInstruction
    {
        public int Bot { get; set; }
        public (DestType Dest, int Value) Low { get; set; }
        public (DestType Dest, int Value) High { get; set; }
    }
    record ValueInstruction : BaseInstruction
    {
        public int Value { get; set; }
        public int Bot { get; set; }
    }

    class Bot()
    {
        public int Id { get; set; }
        public int? LowValue { get; set; }
        public int? HighValue { get; set; }

        public void ReceiveValue(int value)
        {
            if (!LowValue.HasValue)
            {
                LowValue = value;
            }
            else
            {
                if(LowValue < value)
                {
                    HighValue = value;
                }
                else
                {
                    HighValue = LowValue;
                    LowValue = value;
                }
            }
        }

        public bool IsComplete => LowValue.HasValue && HighValue.HasValue;
    }

    // Parser indexes
    const int TypeIndex = 0;
    const int ValueIndex = 1;
    const int BotIndex = 5;
    const int LowDestIndex = 5;
    const int LowValueIndex = 6;
    const int HighDestIndex = 10;
    const int HighValueIndex = 11;

    static IEnumerable<BaseInstruction> ParseContent(string[] lines)
    {
        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            BaseInstruction instruction;
            if (parts.Length == 6)
            {
                // Value instruction
                instruction = new ValueInstruction()
                {
                    Value = int.Parse(parts[ValueIndex]),
                    Bot = int.Parse(parts[BotIndex])
                };
            }
            else
            {
                // Bot instruction
                DestType lowType = parts[LowDestIndex] == "bot" ? DestType.Bot : DestType.Output;
                int lowValue = int.Parse(parts[LowValueIndex]);
                DestType highType = parts[HighDestIndex] == "bot" ? DestType.Bot : DestType.Output;
                int highValue = int.Parse(parts[HighValueIndex]);
                instruction = new BotInstruction()
                {
                    Bot = int.Parse(parts[ValueIndex]),
                    Low = (lowType, lowValue),
                    High = (highType, highValue)
                };
            }
            yield return instruction;
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        Queue<BaseInstruction> queue = new();
        Dictionary<int, Bot> bots = [];
        Dictionary<int, int> output = [];

        foreach (var ins in ParseContent(File.ReadAllLines(InputPath)))
            queue.Enqueue(ins);

        while (queue.TryDequeue(out var ins))
        {
            if (ins is ValueInstruction valueIns)
            {
                if (!bots.TryGetValue(valueIns.Bot, out Bot? value))
                {
                    value = new Bot() { Id = valueIns.Bot };
                    bots.Add(valueIns.Bot, value);
                }
                value.ReceiveValue(valueIns.Value);
            }
            else if (ins is BotInstruction botIns)
            {                
                if (bots.TryGetValue(botIns.Bot, out Bot? value) && value.IsComplete)
                {
                    if (botIns.Low.Dest == DestType.Bot)
                    {
                        if(!bots.TryGetValue(botIns.Low.Value, out Bot? lowBot))
                        {
                            bots.Add(botIns.Low.Value, new Bot() { Id = botIns.Low.Value });
                        }
                        bots[botIns.Low.Value].ReceiveValue(bots[botIns.Bot].LowValue.Value);
                    }
                    else
                    {
                        output.TryAdd(botIns.Low.Value, value.LowValue.Value);
                    }
                    if (botIns.High.Dest == DestType.Bot)
                    {
                        if (!bots.TryGetValue(botIns.High.Value, out Bot? highBot))
                        {
                            bots.Add(botIns.High.Value, new Bot() { Id = botIns.High.Value });
                        }
                        bots[botIns.High.Value].ReceiveValue(bots[botIns.Bot].HighValue.Value);
                    }
                    else
                    {
                        output.TryAdd(botIns.High.Value, value.HighValue.Value);
                    }
                }
                else
                {
                    queue.Enqueue(botIns);
                }
            }
        }

        return bots.Values.First(bot => bot.LowValue == 17 && bot.HighValue == 61).Id;
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
        Queue<BaseInstruction> queue = new();
        Dictionary<int, Bot> bots = [];
        Dictionary<int, int> output = [];

        foreach (var ins in ParseContent(File.ReadAllLines(InputPath)))
            queue.Enqueue(ins);

        while (queue.TryDequeue(out var ins))
        {
            if (ins is ValueInstruction valueIns)
            {
                if (!bots.TryGetValue(valueIns.Bot, out Bot? value))
                {
                    value = new Bot() { Id = valueIns.Bot };
                    bots.Add(valueIns.Bot, value);
                }
                value.ReceiveValue(valueIns.Value);
            }
            else if (ins is BotInstruction botIns)
            {
                if (bots.TryGetValue(botIns.Bot, out Bot? value) && value.IsComplete)
                {
                    if (botIns.Low.Dest == DestType.Bot)
                    {
                        if (!bots.TryGetValue(botIns.Low.Value, out Bot? lowBot))
                        {
                            bots.Add(botIns.Low.Value, new Bot() { Id = botIns.Low.Value });
                        }
                        bots[botIns.Low.Value].ReceiveValue(bots[botIns.Bot].LowValue.Value);
                    }
                    else
                    {
                        output.TryAdd(botIns.Low.Value, value.LowValue.Value);
                    }
                    if (botIns.High.Dest == DestType.Bot)
                    {
                        if (!bots.TryGetValue(botIns.High.Value, out Bot? highBot))
                        {
                            bots.Add(botIns.High.Value, new Bot() { Id = botIns.High.Value });
                        }
                        bots[botIns.High.Value].ReceiveValue(bots[botIns.Bot].HighValue.Value);
                    }
                    else
                    {
                        output.TryAdd(botIns.High.Value, value.HighValue.Value);
                    }
                }
                else
                {
                    queue.Enqueue(botIns);
                }
            }
        }

        return output[0] * output[1] * output[2];
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
