using AdventOfCode_2016_CSharp.Common.AssembunnyCode;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day25;

public class Day25 : BaseDay
{
    readonly IOperation[] _operations;

    public Day25(bool isTest) : base("25", isTest)
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
                "out" => new OutOp(parts[1][0]),
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



        const int maxSize = 500;

        int regA = -1;

        int generated = 0;

        while (generated != maxSize)
        {
            regA++;
            // Console.WriteLine($"Trying register a with value {regA}");
            Registers registers = new();
            registers.Register.Add('a', regA);
            registers.Register.Add('b', 0);
            registers.Register.Add('c', 0);
            registers.Register.Add('d', 0);
            List<bool> outputs = [];
            int opIdx = 0;
            while (opIdx < numOfIns && generated < maxSize)
            {
                int offset = _operations[opIdx].Execute(registers);

                if (_operations[opIdx] is IToggle toogle && (opIdx + toogle.Toggle < numOfIns))
                    _operations[opIdx + toogle.Toggle] = _operations[opIdx + toogle.Toggle].Toggle();
                if (_operations[opIdx] is IOutput output)
                {
                    if (output.Output == 0)
                        outputs.Add(false);
                    else if (output.Output == 1)
                        outputs.Add(true);
                    else
                        break;

                    if (outputs.Count > 1 && outputs[^2] == outputs[^1])
                        break;

                    generated++;
                }
                opIdx += offset;
            }
        }

        return regA;
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
        return Content.Length;
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
