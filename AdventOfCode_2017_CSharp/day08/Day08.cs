using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day08;

public class Day08 : BaseDay
{
    private readonly Instruction[] _instructions;
    private readonly Dictionary<string, int> _registryValues = [];

    bool GreaterThan(string reg, int value) => _registryValues[reg] > value;
    bool LesserThan(string reg, int value) => _registryValues[reg] < value;
    bool GreaterOrEqualThan(string reg, int value) => _registryValues[reg] >= value;
    bool LesserOrEqualThan(string reg, int value) => _registryValues[reg] <= value;
    bool EqualTo(string reg, int value) => _registryValues[reg] == value;
    bool DifferentThan(string reg, int value) => _registryValues[reg] != value;

    void Increment(string reg, int value) => _registryValues[reg] += value;
    void Decrement(string reg, int value) => _registryValues[reg] -= value;

    class Instruction
    {
        public string Register { get; set; }
        public Func<int> Operation { get; set; }

        public Func<bool> Condition { get; set; }
    }

    public Day08(bool isTest) : base("08", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        _instructions = new Instruction[lines.Length];

        for (int idx = 0; idx < lines.Length; idx++)
        {
            var parts = lines[idx].Split(" ");
            (string reg1, int val1, string reg2, int val2) =
                (parts[0], int.Parse(parts[2]), parts[4], int.Parse(parts[6]));
            Action<string, int> op = parts[1] switch
            {
                "inc" => Increment,
                "dec" => Decrement,
                _ => throw new ArgumentException("invalid operation")
            };
            Func<string, int, bool> cond = parts[5] switch
            {
                ">" => GreaterThan,
                "<" => LesserThan,
                ">=" => GreaterOrEqualThan,
                "<=" => LesserOrEqualThan,
                "==" => EqualTo,
                "!=" => DifferentThan,
                _ => throw new ArgumentException("invalid condition")
            };

            _registryValues.TryAdd(reg1, 0);
            _registryValues.TryAdd(reg2, 0);

            _instructions[idx] = new()
            {
                Register = reg1,
                Operation = () =>
                {
                    op(reg1, val1);
                    return _registryValues[reg1];
                },
                Condition = () => cond(reg2, val2)
            };
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        foreach (var ins in _instructions)
        {
            if (ins.Condition())
                ins.Operation();
        }
        return _registryValues.Values.Max();
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
        int maxValue = int.MinValue;
        foreach (var ins in _instructions)
        {
            if (ins.Condition())
                maxValue = Math.Max(maxValue, ins.Operation());
        }
        return maxValue;
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
