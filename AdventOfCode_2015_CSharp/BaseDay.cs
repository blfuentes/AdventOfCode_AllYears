using System.Diagnostics;

namespace AdventOfCode_2015_CSharp;

public abstract class BaseDay : ISolver
{
    protected readonly string Day = "00";
    protected readonly Stopwatch StopWatch;
    protected readonly string Content;

    protected BaseDay(string day, bool isTest)
    {
        Day = day;
        TestInputPath = Path.Combine(AppContext.BaseDirectory, @$"..\..\..\day{Day}/test_input_{Day}.txt");
        DayInputPath = Path.Combine(AppContext.BaseDirectory, @$"..\..\..\day{Day}/day{Day}.txt");
        StopWatch = new Stopwatch();
        Content = isTest ? File.ReadAllText(TestInputPath) : File.ReadAllText(DayInputPath);
    }

    protected string TestInputPath;

    protected string DayInputPath;

    public abstract string SolvePart1();

    public abstract string SolvePart2();
}
