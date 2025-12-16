using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace AdventOfCode_2015_CSharp.day15;

public partial class Day15(bool isTest = false) : BaseDay("15", isTest)
{
    [GeneratedRegex("-?\\d+")]
    private static partial Regex NumbersRegex();

    private readonly Regex numbers = NumbersRegex();

    record Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Durability { get; set; }
        public int Flavor { get; set; }
        public int Texture { get; set; }
        public int Calories { get; set; }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        var ingredients = File.ReadAllLines(InputPath)
            .Select((line, i) =>
            {
                var matches = numbers.Matches(line);
                return new Ingredient
                {
                    Id = i,
                    Name = line.Split(":")[0],
                    Capacity = int.Parse(matches[0].Value),
                    Durability = int.Parse(matches[1].Value),
                    Flavor = int.Parse(matches[2].Value),
                    Texture = int.Parse(matches[3].Value),
                    Calories = int.Parse(matches[4].Value)
                };
            });
        return ingredients.Count();
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
