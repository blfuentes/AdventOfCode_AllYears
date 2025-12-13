using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day07;

public class Day07(bool isTest = false) : BaseDay("07", isTest)
{
    Dictionary<string, ushort> registers = [];
    Queue<string[]> operations = [];

    bool GetValue(string reg, out ushort value)
    {
        bool found = false;
        value = 0;

        found = ushort.TryParse(reg, out value);
        if (!found)
            found = registers.TryGetValue(reg, out value);

        return found;
    }

    void RunOperations(Dictionary<string, ushort> registers, Queue<string[]> operations)
    {
        while (operations.Count > 0)
        {
            var parts = operations.Dequeue();
            //Console.WriteLine(string.Join(" ", parts));
            switch (parts.Length)
            {
                case 3: // assign value to register
                    if (GetValue(parts[0], out var assign))
                    {
                        if (registers.ContainsKey(parts[2]))
                            registers[parts[2]] = assign;
                        else
                            registers.Add(parts[2], assign);
                    }
                    else
                    {
                        operations.Enqueue(parts);
                    }
                    break;
                case 4: // negate value
                    if (GetValue(parts[1], out ushort negate))
                    {
                        registers[parts[3]] = (ushort)~((int)negate);
                    }
                    else
                        operations.Enqueue(parts);
                    break;
                case 5: // AND, OR, LSHIFT, RSHIFT
                    switch (parts[1])
                    {
                        case "AND":
                            if (GetValue(parts[0], out ushort a) &&
                                GetValue(parts[2], out ushort b))
                            {
                                if (registers.ContainsKey(parts[4]))
                                    registers[parts[4]] = (ushort)(a & b);
                                else
                                    registers.Add(parts[4], (ushort)(a & b));
                            }
                            else
                            {
                                operations.Enqueue(parts);
                            }
                            break;
                        case "OR":
                            if (GetValue(parts[0], out ushort c) &&
                                GetValue(parts[2], out ushort d))
                            {
                                if (registers.ContainsKey(parts[4]))
                                    registers[parts[4]] = (ushort)(c | d);
                                else
                                    registers.Add(parts[4], (ushort)(c | d));
                            }
                            else
                            {
                                operations.Enqueue(parts);
                            }
                            break;
                        case "LSHIFT":
                            if (GetValue(parts[0], out ushort l) &&
                                GetValue(parts[2], out ushort ls))
                            {
                                if (registers.ContainsKey(parts[4]))
                                    registers[parts[4]] = (ushort)(((int)l) << (int)ls);
                                else
                                    registers.Add(parts[4], (ushort)(((int)l) << (int)ls));
                            }
                            else
                            {
                                operations.Enqueue(parts);
                            }
                            break;
                        case "RSHIFT":
                            if (GetValue(parts[0], out ushort r) &&
                                GetValue(parts[2], out ushort rs))
                            {
                                if (registers.ContainsKey(parts[4]))
                                    registers[parts[4]] = (ushort)(((int)r) >> (int)rs);
                                else
                                    registers.Add(parts[4], (ushort)(((int)r) >> (int)rs));
                            }
                            else
                            {
                                operations.Enqueue(parts);
                            }
                            break;
                    }
                    break;
            }
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        foreach (var operation in File.ReadAllLines(InputPath))
        {
            operations.Enqueue(operation.Split(" "));
        }

        RunOperations(registers, operations);
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
        registers = [];
        registers.Add("b", 956);

        operations.Clear();
        foreach (var operation in File.ReadAllLines(InputPath))
        {
            operations.Enqueue(operation.Split(" "));
        }

        RunOperations(registers, operations);
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
