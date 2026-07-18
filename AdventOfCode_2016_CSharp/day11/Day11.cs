using BenchmarkDotNet.Attributes;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode_2016_CSharp.day11;

public partial class Day11(bool isTest = false) : BaseDay("11", isTest)
{
    enum ComponentType
    {
        Microchip,
        Generator,
        Nothing
    }
    struct Component
    {
        public string Name { get; set; }
        public ComponentType Kind { get; set; }
    };

    struct Floor
    {
        public int Level { get; set; }

        public IList<Component> Components { get; set; }
        public readonly int Size => Components.Count;
    };

    IEnumerable<Floor> ParseContent()
    {
        foreach (var (index, value) in (File.ReadAllLines(InputPath).Select((value, index) => (index, value))))
        {
            // extract the components
            List<Component> components = [];
            foreach (Match comp in CaptureRegex().Matches(value))
            {
                ComponentType kind = comp.Groups[2].Value switch
                {
                    "microchip" => ComponentType.Microchip,
                    "generator" => ComponentType.Generator,
                    _ => ComponentType.Nothing
                };

                components.Add(new()
                {
                    Name = comp.Groups[1].Value.Replace("-compatible", ""),
                    Kind = kind
                });
            }

            yield return new() { Level = index, Components = components };
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        Floor[] containmentArea = [.. ParseContent()];
        int[] elementsByFloor = [.. containmentArea.Select(a => a.Size)];
        int movements = 0;
        int totalElements = elementsByFloor.Sum();
        while (elementsByFloor.Last() != totalElements)
        {
            int currentFloor = 0;
            while (elementsByFloor[currentFloor] == 0)
                currentFloor++;
            movements += 2 * (elementsByFloor[currentFloor] - 1) - 1;
            elementsByFloor[currentFloor + 1] += elementsByFloor[currentFloor];
            elementsByFloor[currentFloor] = 0;
        }
        return movements;
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
        Floor[] containmentArea = [.. ParseContent()];
        containmentArea[0].Components.Add(new() { Name = "elerium", Kind = ComponentType.Generator });
        containmentArea[0].Components.Add(new() { Name = "elerium", Kind = ComponentType.Microchip });
        containmentArea[0].Components.Add(new() { Name = "dilithium", Kind = ComponentType.Generator });
        containmentArea[0].Components.Add(new() { Name = "dilithium", Kind = ComponentType.Microchip });
        int[] elementsByFloor = [.. containmentArea.Select(a => a.Size)];
        int movements = 0;
        int totalElements = elementsByFloor.Sum();
        while (elementsByFloor.Last() != totalElements)
        {
            int currentFloor = 0;
            while (elementsByFloor[currentFloor] == 0)
                currentFloor++;
            movements += 2 * (elementsByFloor[currentFloor] - 1) - 1;
            elementsByFloor[currentFloor + 1] += elementsByFloor[currentFloor];
            elementsByFloor[currentFloor] = 0;
        }
        return movements;
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion

    [GeneratedRegex(@"\b([a-z]+(?:-[a-z]+)?) (generator|microchip|relevant)\b")]
    private static partial Regex CaptureRegex();
}
