using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day20;

public class Day20(bool isTest = false) : BaseDay("20", isTest)
{
    #region Part 1
    //static int FindFirstHouse(int target)
    //{
    //    Dictionary<int, int> presents = [];

    //    int house = 1;
    //    bool notFound = true;
    //    while (notFound)
    //    {
    //        int current = 0;
    //        for (int elf = house; elf > 0; elf--)
    //        {
    //            if (house % elf == 0)
    //                current += elf * 10;
    //            if (current >= target)
    //            {
    //                return house;
    //            }
    //        }
    //        if (house % 1000 == 0)
    //            Console.WriteLine($"House {house} got {current} presents.");
    //        house++;
    //    }
    //    return -1;
    //}

    static int FindHouse(int target)
    {
        int house = 1;
        bool notFound = true;
        static IEnumerable<int> GetElves(int house)
        {
            for (int elf = 1; elf < Math.Sqrt(house); elf++)
            {
                if (house % elf == 0)
                {
                    yield return house / elf;
                    yield return elf;
                }
            }
            yield break;
        }
        while (notFound)
        {
            var presents = GetElves(house).Distinct().Sum() * 10;

            if (presents >= target)
                return house;
            house++;
        }
        return -1;
    }

    [Benchmark]
    public int RunPart1()
    {
        return FindHouse(int.Parse(Content));
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
    static int FindHouse2(int target)
    {
        int house = 1;
        bool notFound = true;
        static IEnumerable<(int number, int factor)> GetElves(int house)
        {
            for (int elf = 1; elf < Math.Sqrt(house); elf++)
            {
                if (house % elf == 0)
                {
                    yield return (house, house / elf);
                    yield return (house, elf);
                }
            }
            yield break;
        }
        while (notFound)
        {
            var presents = GetElves(house)
                .Where(comp => comp.number / comp.factor <= 50)
                .Select(_ => _.factor)
                .Distinct().Sum() * 11;

            if (presents >= target)
                return house;
            house++;
        }
        return -1;
    }

    [Benchmark]
    public int RunPart2()
    {
        return FindHouse2(int.Parse(Content));
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
