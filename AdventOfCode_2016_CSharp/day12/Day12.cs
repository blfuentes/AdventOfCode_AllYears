using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using Gee.External.Capstone;

namespace AdventOfCode_2016_CSharp.day12;

public class Day12(bool isTest = false) : BaseDay("12", isTest)
{
    sealed class Registers
    {
        public Dictionary<char, int> Register { get; set; }
        public int Get(char r) => Register[r];
        public void Set(char r, int value) => _ = Register.TryAdd(r, value);
    }

    record RegisterValue
    {
        public RegisterValue(string value)
        {
            if (int.TryParse(value, out int v))
            {
                Value = v;
            }
            else
            {
                Registry = value[0];
            }
        }

        public int? Value;
        public char Registry;
    }

    interface IOperation
    {
        int Execute(Registers registers);
    }

    record CopyOp(RegisterValue Source, char Target) : IOperation
    {
        public int Execute(Registers registers)
        {
            registers.Register[Target] = Source.Value ?? registers.Register[Source.Registry];
            return 1;
        }
    }

    record IncOp(char Source) : IOperation
    {
        public int Execute(Registers registers)
        {
            registers.Register[Source]++;
            return 1;
        }
    }

    record DecOp(char Source) : IOperation
    {
        public int Execute(Registers registers)
        {
            registers.Register[Source]--;
            return 1;
        }
    }

    record JnzOp(RegisterValue Source, RegisterValue OffSet) : IOperation
    {
        public int Execute(Registers registers)
        {
            return (Source.Value ?? registers.Register[Source.Registry]) != 0 ?
                OffSet.Value ?? registers.Register[OffSet.Registry] :
                1;
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        List<IOperation> operations = [];
        Registers registers = new()
        {
            Register = new()
            {
                { 'a', 0 },
                { 'b', 0 },
                { 'c', 0 },
                { 'd', 0 }
            }
        };
        int opIdx = 0;

        foreach (var line in File.ReadAllLines(InputPath))
        {
            var parts = line.Split(" ");
            IOperation op = parts[0] switch
            {
                "cpy" => new CopyOp(new RegisterValue(parts[1]), parts[2][0]),
                "inc" => new IncOp(parts[1][0]),
                "dec" => new DecOp(parts[1][0]),
                "jnz" => new JnzOp(new RegisterValue(parts[1]), new RegisterValue(parts[2])),
                _ => throw new InvalidDataException("invalid line")
            };
            operations.Add(op);
        }
        while (opIdx < operations.Count)
        {
            opIdx += operations[opIdx].Execute(registers);
        }
        return registers.Register['a'];
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
        List<IOperation> operations = [];
        Registers registers = new()
        {
            Register = new()
            {
                { 'a', 0 },
                { 'b', 0 },
                { 'c', 1 },
                { 'd', 0 }
            }
        };
        int opIdx = 0;

        foreach (var line in File.ReadAllLines(InputPath))
        {
            var parts = line.Split(" ");
            IOperation op = parts[0] switch
            {
                "cpy" => new CopyOp(new RegisterValue(parts[1]), parts[2][0]),
                "inc" => new IncOp(parts[1][0]),
                "dec" => new DecOp(parts[1][0]),
                "jnz" => new JnzOp(new RegisterValue(parts[1]), new RegisterValue(parts[2])),
                _ => throw new InvalidDataException("invalid line")
            };
            operations.Add(op);
        }
        // int counter = 0;
        // bool found = false;

        while (opIdx < operations.Count)
        {
            // if (!found && counter < 100)
            // {
            //     Console.WriteLine($"Op: {opIdx:D2} - A: {registers.Register['a']:D4} - B: {registers.Register['b']:D4} - C: {registers.Register['c']:D4} - D: {registers.Register['d']:D4} ");
            // }
            opIdx += operations[opIdx].Execute(registers);
            // counter++;
        }
        // Console.WriteLine($"A: {registers.Register['a']:D4} - B: {registers.Register['b']:D4} - C: {registers.Register['c']:D4} - D: {registers.Register['d']:D4} ");

        return registers.Register['a'];
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
