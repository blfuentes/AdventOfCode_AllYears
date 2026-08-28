using System.Data.Common;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day15;

public partial class Day15(bool isTest = false) : BaseDay("15", isTest)
{
    struct Disc
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public int Position { get; set; }
        public int Period { get; set; }

        public readonly bool Aligned(int second) => (Position + second + Id) % Size == 0;
    }

    static int SolveByCongruence(IEnumerable<Disc> discs)
    {
        int time = 0;
        int modulus = 1;

        foreach (var disc in discs)
        {
            int required = ((-disc.Position - disc.Id) % disc.Size + disc.Size) % disc.Size;
            int combinedGcd = Gcd(modulus, disc.Size);

            if ((required - time) % combinedGcd != 0)
                throw new InvalidOperationException("No solution exists for the provided disc collection.");

            int modPart = disc.Size / combinedGcd;
            int inverse = ModularInverse(modulus / combinedGcd, modPart);
            int step = (((required - time) / combinedGcd) * inverse) % modPart;
            if (step < 0)
                step += modPart;

            time += step * modulus;
            modulus = (modulus / combinedGcd) * disc.Size;
            time = ((time % modulus) + modulus) % modulus;
        }

        return ((time % modulus) + modulus) % modulus;
    }

    private static int Gcd(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);

        while (b != 0)
        {
            int remainder = a % b;
            a = b;
            b = remainder;
        }

        return a;
    }

    private static int ModularInverse(int value, int modulus)
    {
        int oldRemainder = modulus;
        int remainder = value % modulus;
        int oldCoefficient = 0;
        int coefficient = 1;

        while (remainder != 0)
        {
            int quotient = oldRemainder / remainder;
            int nextOld = oldCoefficient - quotient * coefficient;
            oldCoefficient = coefficient;
            coefficient = nextOld;

            int nextRemainder = oldRemainder % remainder;
            oldRemainder = remainder;
            remainder = nextRemainder;
        }

        if (oldRemainder != 1)
            throw new InvalidOperationException("The congruence system does not have a modular inverse.");

        if (oldCoefficient < 0)
            oldCoefficient += modulus;

        return oldCoefficient;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        Disc[] discs = [.. File.ReadAllLines(InputPath)
            .Select(line => {
                int[] values = [.. NumbersRegex().Matches(line).Select(v => int.Parse(v.Value))];
                return new Disc()
                {
                    Id = values[0],
                    Size = values[1],
                    Position = values[3]
                };
            })];

        int time = 0;
        while (!discs.All(d => d.Aligned(time))) time++;

        return time;

        // return SolveByCongruence(discs);
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
        List<Disc> discs = [.. File.ReadAllLines(InputPath)
            .Select(line => {
                int[] values = [.. NumbersRegex().Matches(line).Select(v => int.Parse(v.Value))];
                return new Disc()
                {
                    Id = values[0],
                    Size = values[1],
                    Position = values[3]
                };
            })];
        discs.Add(new Disc()
        {
            Id = discs.Count + 1,
            Size = 11,
            Position = 0
        });

        int time = 0;
        while (!discs.All(d => d.Aligned(time))) time++;

        return time;
        // return SolveByCongruence(discs);
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumbersRegex();
    #endregion
}
