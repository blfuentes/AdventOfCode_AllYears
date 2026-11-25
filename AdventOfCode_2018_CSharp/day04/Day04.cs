using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2018_CSharp.day04;

public partial class Day04 : BaseDay
{
    public record Sleep
    {
        public DateTime Start;
        public DateTime End;
    }
    public record Guard(int Id)
    {
        public List<Sleep> Sleeps = [];
    }

    static DateTime BuildShiftTime(int[] numbers)
    {
        return
            new DateTime(
                numbers[0],
                numbers[1],
                numbers[2],
                numbers[3],
                numbers[4],
                0);
    }

    private readonly Dictionary<int, Guard> _guards;

    public Day04(bool isTest = false) : base("04", isTest)
    {
        _guards = [];
        int currentGuard = 0;
        var lines = File.ReadAllLines(InputPath).Order();
        bool isAwake = false;

        foreach (var line in lines)
        {
            int[] numbers = [.. NumbersRegex().Matches(line).Select(m => int.Parse(m.Value))];
            if (numbers.Length > 5)
            {
                currentGuard = numbers[5];
                _ = _guards.TryAdd(currentGuard, new Guard(currentGuard));

                isAwake = true;
            }
            else
            {
                if (isAwake)
                {
                    var newShift = new Sleep()
                    {
                        Start = BuildShiftTime(numbers)
                    };
                    _guards[currentGuard].Sleeps.Add(newShift);

                    isAwake = false;
                }
                else
                {
                    var lastSleep = _guards[currentGuard].Sleeps.Last();
                    lastSleep.End = BuildShiftTime(numbers);

                    isAwake = true;
                }
            }
        }
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        Dictionary<int, int> times = [];
        foreach (var guard in _guards)
        {
            times.Add(guard.Key, 0);
            foreach (var shift in guard.Value.Sleeps.Where(s => s.End > s.Start))
            {
                times[guard.Key] += (shift.End - shift.Start).Minutes;
            }
        }
        var mostSleep = times.MaxBy(t => t.Value);
        var minutesCounter = new int[60];
        foreach (var shift in _guards[mostSleep.Key].Sleeps.Where(s => s.End > s.Start))
        {
            var minutes = (shift.End - shift.Start).Minutes;
            var hours = minutes / 60;
            var restminutes = minutes - (hours * 60);
            if (hours > 0)
            {
                for (int i = 0; i < 60; i++)
                {
                    minutesCounter[shift.Start.Minute + i % 60] += hours * 60;
                }
            }
            for (int i = 0; i < restminutes; i++)
            {
                minutesCounter[shift.Start.Minute + i % 60]++;
            }
        }

        var maxMinute = minutesCounter.IndexOf(minutesCounter.Max());
        return mostSleep.Key * maxMinute;
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
        Dictionary<int, int[]> minutesByGuard = [];
        foreach (var guard in _guards)
        {
            var minutesCounter = new int[60];
            minutesByGuard.Add(guard.Key, minutesCounter);
            foreach (var shift in guard.Value.Sleeps.Where(s => s.End > s.Start))
            {
                var minutes = (shift.End - shift.Start).Minutes;
                var hours = minutes / 60;
                var restminutes = minutes - (hours * 60);

                for (int i = 0; i < restminutes; i++)
                    minutesCounter[shift.Start.Minute + i % 60]++;
            }
        }
        var (id, times, minute) =
            minutesByGuard.Select(g =>
            {
                var maxMinuteCount = g.Value.Max();
                var maxMinute = g.Value.IndexOf(maxMinuteCount);
                (int id, int times, int minute) mbg = (g.Key, maxMinuteCount, maxMinute);
                return mbg;
            })
            .MaxBy(mbg => mbg.times);

        return id * minute;
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
    private static partial Regex NumbersRegex();
}
