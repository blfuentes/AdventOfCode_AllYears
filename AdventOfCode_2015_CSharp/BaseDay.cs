using System.Diagnostics;

namespace AdventOfCode_2015_CSharp;

public abstract class BaseDay : ISolver
{
    protected readonly string Day = "00";
    protected readonly Stopwatch StopWatch;
    protected readonly bool IsTest;
    protected string Content;

    protected BaseDay(string day, bool isTest)
    {
        Day = day;
        IsTest = isTest;
        TestInputPath = Path.Combine(AppContext.BaseDirectory, @$"..\..\..\day{Day}/test_input_{Day}.txt");
        DayInputPath = Path.Combine(AppContext.BaseDirectory, @$"..\..\..\day{Day}/day{Day}.txt");
        StopWatch = new Stopwatch();
        InputPath = IsTest ? TestInputPath : DayInputPath;
        Content = File.ReadAllText(InputPath);
    }
    protected string InputPath;

    protected string TestInputPath;

    protected string DayInputPath;

    public abstract string SolvePart1();

    public abstract string SolvePart2();
}
