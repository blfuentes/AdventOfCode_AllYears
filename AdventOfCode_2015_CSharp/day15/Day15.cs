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

    static (int, int) GetCookieScore(IEnumerable<(int amount, Ingredient ingredient)> ingredientShares)
    {
        int capacity = Math.Max(0, ingredientShares.Select(a => a.amount * a.ingredient.Capacity).Sum());
        int durability = Math.Max(0, ingredientShares.Select(a => a.amount * a.ingredient.Durability).Sum());
        int flavor = Math.Max(0, ingredientShares.Select(a => a.amount * a.ingredient.Flavor).Sum());
        int texture = Math.Max(0, ingredientShares.Select(a => a.amount * a.ingredient.Texture).Sum());
        int calories = Math.Max(0, ingredientShares.Select(a => a.amount * a.ingredient.Calories).Sum());
        return (calories, capacity * durability * flavor * texture);
    }

    static IEnumerable<List<int>> GetShares(int numOfIngredients)
    {
        int totalSpoons = 100;

        IEnumerable<List<int>> FindCombination(List<int> current, int remaining)
        {
            if (current.Count == numOfIngredients)
            {
                if (remaining == 0)
                {
                    //Console.WriteLine($"Share: {string.Join("-", current.Select(c => c.ToString()))}");
                    yield return current;
                }
                yield break;
            }

            for (int s = 1; s <= remaining; s++)
            {
                foreach (var result in FindCombination(
                    [.. current, s],
                    remaining - s))
                {
                    yield return result;
                }
            }
        }

        return FindCombination([], totalSpoons);
    }

    // ReallySlow...
    //static IEnumerable<List<int>> GetShares()
    //{
    //    for (int i = 0; i < 100; i++)
    //    {
    //        for (int j = 0; j < 100-i; j++)
    //        {
    //            for (int k = 0; k < 100 - i - j; k++)
    //            {
    //                for (int l = 0; l < 100 - i - j - k; l++)
    //                {
    //                    Console.WriteLine($"Share: {i}-{j}-{k}-{l}");
    //                    yield return [i, j, k, l];
    //                }
    //            }
    //        }
    //    }
    //}

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
        int bestTotal = 0;
        var permutations = GetShares(ingredients.Count());
        foreach(var possible in permutations)
        {
            var (_, score) = GetCookieScore(possible.Zip(ingredients));
            if (score > bestTotal)
                bestTotal = score;
        }
        return bestTotal;
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
        int bestTotal = 0;
        var permutations = GetShares(ingredients.Count());
        foreach (var possible in permutations)
        {
            var (calories, score) = GetCookieScore(possible.Zip(ingredients));
            if (score > bestTotal && calories == 500)
                bestTotal = score;
        }
        return bestTotal;
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
