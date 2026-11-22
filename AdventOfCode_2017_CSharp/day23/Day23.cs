using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AdventOfCode_2017_CSharp.day23;

public class Day23 : BaseDay
{
    public abstract record Operation
    {
        public char _reg1;
        public char _reg2;
        public int? _value1;
        public int? _value2;

        public Operation(string input1, string input2)
        {
            if (int.TryParse(input1, out int value1))
                _value1 = value1;
            else
                _reg1 = input1[0];

            if (int.TryParse(input2, out int value2))
                _value2 = value2;
            else
                _reg2 = input2[0];
        }

        public abstract long Execute(
            Dictionary<char, long> registers);
    }

    public record SetOperation(string Input1, string Input2)
        : Operation(Input1, Input2)
    {
        public override long Execute(Dictionary<char, long> registers)
        {
            registers[_reg1] = _value2 ?? registers[_reg2];
            return 1;
        }
    }

    public record SubOperation(string Input1, string Input2)
        : Operation(Input1, Input2)
    {
        public override long Execute(Dictionary<char, long> registers)
        {
            registers[_reg1] -= _value2 ?? registers[_reg2];
            return 1;
        }
    }

    public record MulOperation(string Input1, string Input2)
        : Operation(Input1, Input2)
    {
        public override long Execute(Dictionary<char, long> registers)
        {
            registers[_reg1] *= _value2 ?? registers[_reg2];
            return 1;
        }
    }

    public record JnzOperation(string Input1, string Input2)
        : Operation(Input1, Input2)
    {
        public override long Execute(Dictionary<char, long> registers)
        {
            return ((_value1 ?? registers[_reg1]) != 0) ? (_value2 ?? registers[_reg2]) : 1;
        }
    }

    private readonly Operation[] _instructions;

    public Day23(bool isTest = false) : base("23", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        _instructions = new Operation[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            var parts = lines[i].Split(" ");
            Operation op = parts[0] switch
            {
                "set" => new SetOperation(parts[1], parts[2]),
                "sub" => new SubOperation(parts[1], parts[2]),
                "mul" => new MulOperation(parts[1], parts[2]),
                "jnz" => new JnzOperation(parts[1], parts[2]),
                _ => throw new ArgumentOutOfRangeException(parts[0], "Invalid operation")
            };
            _instructions[i] = op;
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        Dictionary<char, long> registers =
            Enumerable.Range('a', 8).ToDictionary(e => (char)e, e => 0L);
        long opIdx = 0;
        int counter = 0;
        while (opIdx < _instructions.Length)
        {
            if (_instructions[opIdx] is MulOperation)
                counter++;
            opIdx += _instructions[opIdx].Execute(registers);
        }
        return counter;
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
    public long RunPart2()
    {
        Dictionary<char, long> registers =
            Enumerable.Range('a', 8).ToDictionary(e => (char)e, e => 0L);
        registers['a'] = 1;
        registers['b'] = 81;
        registers['c'] = registers['b'];
        if (registers['a'] != 0)
        {
            registers['b'] = registers['b'] * 100 + 100_000;
            registers['c'] = registers['b'] + 17_000;
        }
        bool firstRun = true;
        while (firstRun || registers['g'] != 0)
        {
            firstRun = false;
            registers['f'] = 1;
            registers['d'] = 2;
            registers['e'] = 2;
            registers['d'] = 2;
            bool doContinue = true;
            while (registers['d'] * registers['d'] <= registers['b'] && doContinue)
            {
                if (registers['b'] % registers['d'] == 0)
                {
                    registers['f'] = 0;
                    doContinue = false;
                }
                registers['d']++;
            }
            if (registers['f'] == 0)
                registers['h']++;
            registers['g'] = registers['b'] - registers['c'];
            registers['b'] += 17;
        }
        return registers['h'];
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
