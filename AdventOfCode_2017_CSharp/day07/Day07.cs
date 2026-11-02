using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day07;

public partial class Day07 : BaseDay
{
    private readonly List<Disc> _discs;
    public Day07(bool isTest) : base("07", isTest)
    {
        _discs = BuildDiscTower();
    }

    class Disc(string name, int weight)
    {
        public string Name = name;
        public int Weight = weight;
        public Disc? Parent;
        public List<Disc> Children = [];

        public int TotalWeight => Weight + Children.Sum(c => c.TotalWeight);
    }

    private List<Disc> BuildDiscTower()
    {
        var lines = File.ReadAllLines(InputPath);
        List<Disc> discs = [];
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var sides = line.Split(" -> ");
            var name = sides[0].Split(" ")[0];
            var weight = int.Parse(NumberRegex().Match(sides[0].Split(" ")[1]).Value);
            var childNames = sides.Length == 2 ? sides[1]?.Split(", ") ?? [] : [];
            var disc = discs.FirstOrDefault(d => d?.Name == name);

            if (disc is null)
            {
                disc = new(name, weight);
                discs.Add(disc);
            }
            else
            {
                disc.Weight = weight;
            }

            foreach (var childName in childNames)
            {
                var child = discs.FirstOrDefault(d => d?.Name == childName);
                if (child is null)
                {
                    child = new(childName, 0);
                    discs.Add(child);
                }

                child.Parent = disc;

                disc.Children.Add(child);
            }
        }

        return discs;
    }

    #region Part 1
    [Benchmark]
    public string RunPart1()
    {
        return _discs.First(d => d.Parent is null).Name;
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
        var init = _discs.Find(d => d.Parent is null);

        var children = init.Children;
        bool sameWeight = children.Count <= 1 ||
                children.All(c => c.TotalWeight == children[0].TotalWeight);
        Disc? unbalanced = null;
        int diff = 0;
        while (!sameWeight)
        {
            var groupOfWeights = children.GroupBy(c => c.TotalWeight);
            unbalanced = groupOfWeights
                .Single(g => g.Count() == 1)
                .Single();
            var other = groupOfWeights
                .First(g => g.Count() > 1)
                .First();

            children = unbalanced.Children;
            sameWeight = children.Count <= 1 ||
                children.All(c => c.TotalWeight == children[0].TotalWeight);
            diff = other.TotalWeight - unbalanced.TotalWeight;
        }

        return unbalanced!.Weight + diff;
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumberRegex();
}
