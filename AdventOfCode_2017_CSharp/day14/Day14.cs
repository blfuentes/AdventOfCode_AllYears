using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day14;

public class Day14(bool isTest = false) : BaseDay("14", isTest)
{

    static string CalculateKnotHash(string content)
    {
        int[] lengths = [.. content.Select(c => (int)c).Concat([17, 31, 73, 47, 23])];
        int numbersSize = 256;
        int[] numbers = [.. Enumerable.Range(0, numbersSize)];
        (int currentPos, int skipSize) = (0, 0);
        for (var round = 0; round < 64; round++)
        {
            foreach (var length in lengths)
            {
                if (length > numbers.Length)
                    continue;

                for (int c = 0; c < length / 2; c++)
                {
                    int swapfrom = (currentPos + c) % numbers.Length;
                    int swapto = (currentPos - c + length - 1) % numbers.Length;
                    (numbers[swapfrom], numbers[swapto]) = (numbers[swapto], numbers[swapfrom]);
                }
                currentPos = (currentPos + length + skipSize) % numbers.Length;
                skipSize++;
            }
        }
        var denseHash = numbers.Chunk(16).Select(chunk =>
        {
            return chunk.Aggregate(0, (xor, value) => xor ^ value).ToString("X2").ToLowerInvariant();
        });
        return string.Join("", denseHash);
    }

    static string GetBinaryFromHex(string value)
    {
        return Convert.ToString(Convert.ToInt32(value, 16), 2)
                      .PadLeft(value.Length * 4, '0');
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        bool[][] grid = new bool[128][];
        int result = 0;
        for (int i = 0; i < 128; i++)
        {
            var current = $"{Content}-{i}";
            var knothash = CalculateKnotHash(current);
            string binary = String.Join("", [.. knothash.ToCharArray().Select(c => GetBinaryFromHex(c.ToString()))]);

            //
            grid[i] = new bool[128];
            for (var j = 0; j < 128; j++)
            {
                grid[i][j] = binary[j] == '1';
                result += binary[j] == '1' ? 1 : 0;
            }
        }
        return result;
    }

    public override string SolvePart1()
    {
        StopWatch.Start();
        var result = RunPart1();
        StopWatch.Stop();
        return $"Final result Day {Day} part 1: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }
    #endregion

    static readonly (int r, int c)[] Dirs = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    static HashSet<(int, int)> GetRegion((int x, int y) root, HashSet<(int, int)> available)
    {
        HashSet<(int, int)> visited = [root];
        Stack<(int, int)> toVisit = [];
        HashSet<(int, int)> region = [root];
        toVisit.Push(root);
        while (toVisit.TryPop(out (int r, int c) current))
        {
            foreach ((int dr, int dc) in Dirs)
            {
                var tocheck = (current.r + dr, current.c + dc);
                if (!visited.Contains(tocheck) && available.Contains(tocheck))
                {
                    toVisit.Push(tocheck);
                    region.Add(tocheck);
                }
                visited.Add(tocheck);
            }
        }
        return region;
    }

    #region Part 2
    [Benchmark]
    public int RunPart2()
    {
        bool[][] grid = new bool[128][];
        int regions = 0;
        HashSet<(int, int)> used = [];
        for (int i = 0; i < 128; i++)
        {
            var current = $"{Content}-{i}";
            var knothash = CalculateKnotHash(current);
            string binary = String.Join("", [.. knothash.ToCharArray().Select(c => GetBinaryFromHex(c.ToString()))]);

            //
            grid[i] = new bool[128];
            for (var j = 0; j < 128; j++)
            {
                grid[i][j] = binary[j] == '1';
                if (grid[i][j])
                    used.Add((i, j));
            }
        }
        while (used.Count > 0)
        {
            var current = used.First();
            var tmp = GetRegion(current, used);
            used.ExceptWith(tmp);
            regions++;
        }

        return regions;
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
