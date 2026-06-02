using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day04;

public class Day04(bool isTest = false) : BaseDay("04", isTest)
{
    struct Room
    {
        public string Name { get; set; }
        public int SectorId { get; set; }
        public string Checksum { get; set; }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        List<Room> rooms = [.. File.ReadAllLines(InputPath).Select(line =>
        {
            var parts = line.Split(['[', ']'], StringSplitOptions.RemoveEmptyEntries);
            var nameAndSectorId = parts[0].Split('-');
            var name = string.Join("-", nameAndSectorId.Take(nameAndSectorId.Length - 1));
            var sectorId = int.Parse(nameAndSectorId.Last());
            var checksum = parts[1];
            return new Room { Name = name, SectorId = sectorId, Checksum = checksum };
        })];

        return rooms.Sum(r =>
        {
            return r.Name
                .ToCharArray()
                .Where(c => c != '-')
                .CountBy(c => c)
                .OrderByDescending(g => g.Value)
                .ThenBy(g => g.Key)
                .Take(5)
                .Select(g => g.Key)
                .SequenceEqual(r.Checksum.ToCharArray()) ? r.SectorId : 0;
        });
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
        List<Room> rooms = [.. File.ReadAllLines(InputPath).Select(line =>
        {
            var parts = line.Split(['[', ']'], StringSplitOptions.RemoveEmptyEntries);
            var nameAndSectorId = parts[0].Split('-');
            var name = string.Join("-", nameAndSectorId.Take(nameAndSectorId.Length - 1));
            var sectorId = int.Parse(nameAndSectorId.Last());
            var checksum = parts[1];
            return new Room { Name = name, SectorId = sectorId, Checksum = checksum };
        })];

        return rooms.First(r =>
        {
            return r.Name.ToCharArray().Select(c =>
            {
                if (c == '-')
                {
                    return ' ';
                }
                else
                {
                    return (char)(((c - 'a' + r.SectorId) % 26) + 'a');
                }
            }).SequenceEqual("northpole object storage".ToCharArray());
        }).SectorId;
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
