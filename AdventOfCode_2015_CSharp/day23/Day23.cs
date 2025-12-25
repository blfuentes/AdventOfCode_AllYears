using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day23;

public class Day23(bool isTest = false) : BaseDay("23", isTest)
{
    internal enum OperationKind
    {
        Hlf,
        Tpl,
        Inc,
        Jmp,
        Jie,
        Jio
    }

    record Operation
    {
        public OperationKind Op{ get; set; }
    }
    record RegOperation : Operation
    {
        public string Register { get; set; }
    }
    record OffsetOperation : Operation
    {
        public string Register { get; set; }
        public int Offset { get; set; }
    }

    readonly List<Operation> Operations = [];
    readonly Dictionary<string, int> Registers = [];

    void ParseContent()
    {
        foreach(var line in File.ReadAllLines(InputPath))
        {
            Operation op = (line.Split(" ")) switch
            {
                [var first, .. var rest] when first == "hlf" =>
                    new RegOperation { Op = OperationKind.Hlf, Register = rest[0] },
                [var first, .. var rest] when first == "tpl" =>
                    new RegOperation { Op = OperationKind.Tpl, Register = rest[0] },
                [var first, .. var rest] when first == "inc" =>
                    new RegOperation { Op = OperationKind.Inc, Register = rest[0] },
                [var first, .. var rest] when first == "jmp" =>
                    new OffsetOperation { Op = OperationKind.Jmp, Offset = int.Parse(rest[0])},
                [var first, .. var rest] when first == "jie" =>
                    new OffsetOperation
                    {
                        Op = OperationKind.Jie,
                        Register = rest[0].Split(",")[0],
                        Offset = int.Parse(rest[1])
                    },
                [var first, .. var rest] when first == "jio" =>
                    new OffsetOperation
                    {
                        Op = OperationKind.Jio,
                        Register = rest[0].Split(",")[0],
                        Offset = int.Parse(rest[1])
                    },
                _ => throw new ArgumentOutOfRangeException("invalid operation")
            };
            Operations.Add(op);
        }
    }

    int ExecuteOp(int currentOpIdx)
    {
        var op = Operations[currentOpIdx];
        currentOpIdx = 1;
        RegOperation regOp;
        OffsetOperation offSetOp;
        string rKey;
        switch (op.Op)
        {
            case OperationKind.Hlf:
                regOp = (op as RegOperation);
                Registers[regOp.Register] /= 2;
                break;
            case OperationKind.Tpl:
                regOp = (op as RegOperation);
                Registers[regOp.Register] *= 3;
                break;
            case OperationKind.Inc:
                regOp = (op as RegOperation);
                Registers[regOp.Register]++;
                break;
            case OperationKind.Jmp:
                offSetOp = (op as OffsetOperation);
                currentOpIdx = offSetOp!.Offset;
                break;
            case OperationKind.Jie:
                offSetOp = (op as OffsetOperation);
                rKey = offSetOp.Register;
                currentOpIdx = Registers[rKey] % 2 == 0 ? offSetOp.Offset : currentOpIdx;
                break;
            case OperationKind.Jio:
                offSetOp = (op as OffsetOperation);
                rKey = offSetOp.Register;
                currentOpIdx = Registers[rKey] == 1 ? offSetOp.Offset : currentOpIdx;
                break;
        }
        return currentOpIdx;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        ParseContent();
        Registers.Add("a", 0);
        Registers.Add("b", 0);
        int opIdx = 0;
        while(opIdx < Operations.Count)
        {
            opIdx += ExecuteOp(opIdx);
        }
        return Registers["b"];
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
        ParseContent();
        Registers.Add("a", 1);
        Registers.Add("b", 0);
        int opIdx = 0;
        while (opIdx < Operations.Count)
        {
            opIdx += ExecuteOp(opIdx);
        }
        return Registers["b"];
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
