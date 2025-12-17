using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day16;

public class Day16(bool isTest = false) : BaseDay("16", isTest)
{
    record Sue
    {
        public int Id { get; set; }
        public int? Children { get; set; }
        public int? Cats { get; set; }
        public int? Samoyeds { get; set; }
        public int? Pomeranians { get; set; }
        public int? Akitas { get; set; }
        public int? Vizslas { get; set; }
        public int? Goldfish { get; set; }
        public int? Trees { get; set; }
        public int? Cars { get; set; }
        public int? Perfumes { get; set; }

        //bool IEquatable<Sue>.Equals(Sue? toCheck)
    }

    static bool Equivalent(Sue expected, Sue toCheck)
    {
        if (toCheck.Children.HasValue && toCheck.Children != expected.Children)
            return false;
        if (toCheck.Cats.HasValue && toCheck.Cats != expected.Cats)
            return false;
        if (toCheck.Samoyeds.HasValue && toCheck.Samoyeds != expected.Samoyeds)
            return false;
        if (toCheck.Pomeranians.HasValue && toCheck.Pomeranians != expected.Pomeranians)
            return false;
        if (toCheck.Akitas.HasValue && toCheck.Akitas != expected.Akitas)
            return false;
        if (toCheck.Vizslas.HasValue && toCheck.Vizslas != expected.Vizslas)
            return false;
        if (toCheck.Goldfish.HasValue && toCheck.Goldfish != expected.Goldfish)
            return false;
        if (toCheck.Trees.HasValue && toCheck.Trees != expected.Trees)
            return false;
        if (toCheck.Cars.HasValue && toCheck.Cars != expected.Cars)
            return false;
        if (toCheck.Perfumes.HasValue && toCheck.Perfumes != expected.Perfumes)
            return false;
        return true;
    }

    static bool Equivalent2(Sue expected, Sue toCheck)
    {
        if (toCheck.Children.HasValue && toCheck.Children != expected.Children)
            return false;
        if (toCheck.Cats.HasValue && toCheck.Cats <= expected.Cats)
            return false;
        if (toCheck.Samoyeds.HasValue && toCheck.Samoyeds != expected.Samoyeds)
            return false;
        if (toCheck.Pomeranians.HasValue && toCheck.Pomeranians >= expected.Pomeranians)
            return false;
        if (toCheck.Akitas.HasValue && toCheck.Akitas != expected.Akitas)
            return false;
        if (toCheck.Vizslas.HasValue && toCheck.Vizslas != expected.Vizslas)
            return false;
        if (toCheck.Goldfish.HasValue && toCheck.Goldfish >= expected.Goldfish)
            return false;
        if (toCheck.Trees.HasValue && toCheck.Trees <= expected.Trees)
            return false;
        if (toCheck.Cars.HasValue && toCheck.Cars != expected.Cars)
            return false;
        if (toCheck.Perfumes.HasValue && toCheck.Perfumes != expected.Perfumes)
            return false;
        return true;
    }

    private static Sue BuildSue(string line)
    {
        int cutIdx = line.IndexOf(": ");
        var (sueId, sueparts) = (line[..cutIdx], line[(cutIdx + 2)..].Split(", "));
        var tmpSue = new Sue
        {
            Id = int.Parse(sueId.Substring(3))
        };
        foreach (var part in sueparts.Select(sp => sp.Split(":")))
        {
            var value = int.Parse(part[1]);

            switch (part[0])
            {
                case "children":
                    tmpSue.Children = value;
                    break;
                case "cats":
                    tmpSue.Cats = value;
                    break;
                case "samoyeds":
                    tmpSue.Samoyeds = value;
                    break;
                case "pomeranians":
                    tmpSue.Pomeranians = value;
                    break;
                case "akitas":
                    tmpSue.Akitas = value;
                    break;
                case "vizslas":
                    tmpSue.Vizslas = value;
                    break;
                case "goldfish":
                    tmpSue.Goldfish = value;
                    break;
                case "trees":
                    tmpSue.Trees = value;
                    break;
                case "cars":
                    tmpSue.Cars = value;
                    break;
                case "perfumes":
                    tmpSue.Perfumes = value;
                    break;
            }
        }
        return tmpSue;
    }

    #region Part 1

    static Sue? FindSue(Sue expected, IEnumerable<Sue> sueCollection, Func<Sue, Sue, bool> findSue)
    {
        foreach (var toCheck in sueCollection)
        {
            if (findSue(expected, toCheck))
                return toCheck;
        }

        return null;
    }

    [Benchmark]
    public int RunPart1()
    {
        var expectedSue = new Sue
        {
            Id = 0,
            Children = 3,
            Cats = 7,
            Samoyeds = 2,
            Pomeranians = 3,
            Akitas = 0,
            Vizslas = 0,
            Goldfish = 5,
            Trees = 3,
            Cars = 2,
            Perfumes = 1
        };

        var sues = File.ReadLines(InputPath).Select(BuildSue);

        return FindSue(expectedSue, sues, Equivalent).Id;
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
        var expectedSue = new Sue
        {
            Id = 0,
            Children = 3,
            Cats = 7,
            Samoyeds = 3,
            Pomeranians = 3,
            Akitas = 0,
            Vizslas = 0,
            Goldfish = 5,
            Trees = 3,
            Cars = 2,
            Perfumes = 1
        };

        var sues = File.ReadLines(InputPath).Select(BuildSue);

        return FindSue(expectedSue, sues, Equivalent2).Id;
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
