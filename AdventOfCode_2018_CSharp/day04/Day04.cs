using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2018_CSharp.day04;

public partial class Day04 : BaseDay
{
    public record Shift
    {
        public DateTime Start;
        public DateTime End;
    }
    public record Guard(int Id)
    {
        public DateTime BeginShift;
        public List<Shift> Shifts = [];
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
                if (_guards.TryAdd(currentGuard, new Guard(currentGuard)))
                {
                    _guards[currentGuard].BeginShift = BuildShiftTime(numbers);
                    _guards[currentGuard].Shifts = [
                        new Shift(){
                            Start = _guards[currentGuard].BeginShift
                        }];
                }
                else
                {
                    var newShift = new Shift()
                    {
                        Start = BuildShiftTime(numbers)
                    };
                    _guards[currentGuard].Shifts.Add(newShift);

                }
                isAwake = true;
            }
            else
            {
                if (isAwake)
                {
                    var lastShift = _guards[currentGuard].Shifts.Last();
                    lastShift.End = BuildShiftTime(numbers);
                    isAwake = false;
                }
                else
                {
                    var newShift = new Shift()
                    {
                        Start = BuildShiftTime(numbers)
                    };
                    _guards[currentGuard].Shifts.Add(newShift);

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
            foreach (var shift in guard.Value.Shifts.Where(s => s.End > s.Start))
            {
                times[guard.Key] += (shift.End - shift.Start).Minutes;
            }
        }
        var mostSleep = times.MaxBy(t => t.Value);
        var minutesCounter = new int[60];
        foreach (var shift in _guards[mostSleep.Key].Shifts.Where(s => s.End > s.Start))
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
        var maxMinute = minutesCounter.Max();
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

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumbersRegex();
}
