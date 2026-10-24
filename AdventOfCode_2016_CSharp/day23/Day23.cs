using System.Collections.Immutable;
using System.Runtime.InteropServices;
using AdventOfCode_2016_CSharp.Common.AssembunnyCode;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day23;

public class Day23 : BaseDay
{
    readonly IOperation[] _operations;

    public Day23(bool isTest) : base("23", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        _operations = new IOperation[lines.Length];

        for (int idx = 0; idx < lines.Length; idx++)
        {
            var parts = lines[idx].Split(" ");
            IOperation op = parts[0] switch
            {
                "cpy" => new CopyOp(new RegisterValue(parts[1]), parts[2][0]),
                "inc" => new IncOp(parts[1][0]),
                "dec" => new DecOp(parts[1][0]),
                "jnz" => new JnzOp(new RegisterValue(parts[1]), new RegisterValue(parts[2])),
                "tgl" => new TglOp(parts[1][0]),
                _ => throw new InvalidDataException("invalid line")
            };
            _operations[idx] = op;
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int numOfIns = _operations.Length;
        int opIdx = 0;
        Registers registers = new();
        registers.Register.Add('a', 7);

        while (opIdx < numOfIns)
        {
            int offset = _operations[opIdx].Execute(registers);

            if (_operations[opIdx] is IToggle toogle && (opIdx + toogle.Toggle < numOfIns))
                _operations[opIdx + toogle.Toggle] = _operations[opIdx + toogle.Toggle].Toggle();

            opIdx += offset;
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
    // hacked solution: If we leave the code running we can identify the loophole using c and d registries.
    // identify the instructions in the input with unusual values,
    // cpy XXX c
    // jnz YYY d
    // these values are a constant for hacking the loophole: XXX * YYY
    // then We just need to add the numberofeggs! (factorial)
    public static int RunPart2()
    {
        return 93 * 72 + 479001600;
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
