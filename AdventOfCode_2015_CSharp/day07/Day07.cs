using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day07;

public class Day07(bool isTest = false) : BaseDay("07", isTest)
{
    Dictionary<string, ushort> registers = [];
    readonly Queue<string[]> operations = new();

    readonly Func<ushort, ushort, ushort> And = (a, b) => (ushort)(a & b);
    readonly Func<ushort, ushort, ushort> Or = (a, b) => (ushort)(a | b);
    readonly Func<ushort, ushort, ushort> LShift = (a, b) => (ushort)(a << b);
    readonly Func<ushort, ushort, ushort> RShift = (a, b) => (ushort)(a >> b);

    bool IsWired(string reg, out ushort value)
    {
        bool found = ushort.TryParse(reg, out value);
        if (!found)
            found = registers.TryGetValue(reg, out value);

        return found;
    }

    void AssignValue(string key, ushort value)
    {
        if (!registers.TryAdd(key, value))
            registers[key] = value;
    }

    void RunOperations(Queue<string[]> operations, ushort? @override = null)
    {
        while (operations.Count > 0)
        {
            var parts = operations.Dequeue();
            string resultKey = parts[^1];
            switch (parts.Length)
            {
                case 3: // assign value to register
                    if (resultKey == "b" && @override.HasValue)
                    {
                        AssignValue(resultKey, @override.Value);
                    }
                    else
                    {
                        if (IsWired(parts[0], out var value))
                            AssignValue(resultKey, value);
                        else
                            operations.Enqueue(parts);
                    }
                    break;
                case 4: // negate value
                    if (IsWired(parts[1], out ushort negate))
                        AssignValue(resultKey, (ushort)~((int)negate));
                    else
                        operations.Enqueue(parts);
                    break;
                case 5: // AND, OR, LSHIFT, RSHIFT
                    if (IsWired(parts[0], out ushort a) &&
                        IsWired(parts[2], out ushort b))
                    {
                        var op = parts[1] switch
                        {
                            "AND" => And,
                            "OR" => Or,
                            "LSHIFT" => LShift,
                            "RSHIFT" => RShift,
                            _ => throw new ArgumentException("unexpected!")
                        };
                        AssignValue(resultKey, op(a, b));
                    }
                    else
                        operations.Enqueue(parts);
                    break;
            }
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        foreach (var operation in File.ReadAllLines(InputPath))
            operations.Enqueue(operation.Split(" "));

        RunOperations(operations);
        return registers["a"]; 
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
        ushort initialA = (ushort)RunPart1();
        registers = [];
        operations.Clear();
        
        foreach (var operation in File.ReadAllLines(InputPath))
            operations.Enqueue(operation.Split(" "));
        
        RunOperations(operations, initialA);
        return registers["a"];
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
