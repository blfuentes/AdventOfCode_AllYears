using System.Diagnostics;
using AdventOfCode_2016_CSharp.Common.AssembunnyCode;
using BenchmarkDotNet.Attributes;
using Gee.External.Capstone;

namespace AdventOfCode_2016_CSharp.day12;

public class Day12(bool isTest = false) : BaseDay("12", isTest)
{
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
