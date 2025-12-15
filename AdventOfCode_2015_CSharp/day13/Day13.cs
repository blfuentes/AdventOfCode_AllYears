using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2015_CSharp.day13;

public class Day13(bool isTest = false) : BaseDay("13", isTest)
{
    Dictionary<string, int> People = [];
    int[,] Happiness;

    #region Part 1
    private (int maxHappiness, List<int> tour) HeldKarpRouteMaxHappiness()
    {
        int numOfNodes = People.Count;
        int subSetCount = 1 << numOfNodes;
        const int NEG_INFINITY = int.MinValue / 2;

        int[,] dp = new int[subSetCount, numOfNodes];
        int[,] parents = new int[subSetCount, numOfNodes];

        for (int i = 0; i < subSetCount; i++)
        {
            for (int j = 0; j < numOfNodes; j++)
            {
                dp[i, j] = NEG_INFINITY;
                parents[i, j] = -1;
            }
        }

        dp[1, 0] = 0;

        for (int mask = 1; mask < subSetCount; mask++)
        {
            if ((mask & 1) == 0)
                continue;

            for (int j = 0; j < numOfNodes; j++)
            {
                if ((mask & (1 << j)) == 0)
                    continue;

                if (dp[mask, j] == NEG_INFINITY)
                    continue;

                for (int next = 0; next < numOfNodes; next++)
                {
                    if ((mask & (1 << next)) != 0)
                        continue;

                    int newMask = mask | (1 << next);
                    int happiness = Happiness[j, next] + Happiness[next, j];
                    int newCost = dp[mask, j] + happiness;

                    if (newCost > dp[newMask, next])
                    {
                        dp[newMask, next] = newCost;
                        parents[newMask, next] = j;
                    }
                }
            }
        }

        int fullMask = subSetCount - 1;
        int maxHappiness = NEG_INFINITY;
        int lastPerson = -1;

        for (int j = 1; j < numOfNodes; j++)
        {
            if (dp[fullMask, j] == NEG_INFINITY)
                continue;

            int totalHappiness = dp[fullMask, j] + Happiness[j, 0] + Happiness[0, j];
            
            if (totalHappiness > maxHappiness)
            {
                maxHappiness = totalHappiness;
                lastPerson = j;
            }
        }

        List<int> tour = [];
        int currentMask = fullMask;
        int currentPerson = lastPerson;

        while (currentPerson != 0 && currentPerson != -1)
        {
            tour.Add(currentPerson);
            int prevPerson = parents[currentMask, currentPerson];
            currentMask ^= (1 << currentPerson);
            currentPerson = prevPerson;
        }

        tour.Add(0);
        tour.Reverse();

        return (maxHappiness, tour);
    }

    [Benchmark]
    public int RunPart1()
    {
        Dictionary<(string, string), int> happiness = [];
        foreach (var line in File.ReadAllLines(InputPath))
        {
            var parts = line[..^1].Split(' ');
            var (peopleFrom, peopleTo, happy) = (parts[0], parts[10], int.Parse(parts[3]) * (parts[2] == "gain" ? 1 : -1));
            People.TryAdd(peopleFrom, People.Count);
            People.TryAdd(peopleTo, People.Count);
            happiness.TryAdd((peopleFrom, peopleTo), happy);
        }
        Happiness = new int[People.Count, People.Count];
        foreach (var kvp in happiness)
        {
            int i = People[kvp.Key.Item1];
            int j = People[kvp.Key.Item2];
            Happiness[i, j] = kvp.Value;
        }

        var (maxHappiness, _) = HeldKarpRouteMaxHappiness();
        return maxHappiness;
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
        Dictionary<(string, string), int> happiness = [];
        foreach (var line in File.ReadAllLines(InputPath))
        {
            var parts = line[..^1].Split(' ');
            var (peopleFrom, peopleTo, happy) = (parts[0], parts[10], int.Parse(parts[3]) * (parts[2] == "gain" ? 1 : -1));
            People.TryAdd(peopleFrom, People.Count);
            People.TryAdd(peopleTo, People.Count);
            happiness.TryAdd((peopleFrom, peopleTo), happy);
        }
        People.TryAdd("ME", People.Count);
        Happiness = new int[People.Count, People.Count];
        foreach (var kvp in happiness)
        {
            int i = People[kvp.Key.Item1];
            int j = People[kvp.Key.Item2];
            Happiness[i, j] = kvp.Value;
        }

        var (maxHappiness, _) = HeldKarpRouteMaxHappiness();
        return maxHappiness;
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
