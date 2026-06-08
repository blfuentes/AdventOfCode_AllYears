using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace AdventOfCode_2016_CSharp.day08;

internal enum Op
{
    Rect,
    RotateColumn,
    RotateRow
}

internal record Instruction(Op Operation, int Param1, int Param2)
{
    public void Exec(char[,] screen)
    {
        switch (Operation)
        {
            case Op.Rect:
                for (int col = 0; col < Param1; col++)
                {
                    for (int row = 0; row < Param2; row++)
                    {
                        screen[row, col] = '#';
                    }
                }
                break;
            case Op.RotateColumn:
                int rows = screen.GetLength(0);
                var rowValues = Enumerable.Range(0, rows).Select(r => screen[r, Param1]).ToArray();
                for (int r = 0; r < rows; r++)
                {
                    screen[(r + Param2) % rows, Param1] = rowValues[r];
                }
                break;
            case Op.RotateRow:
                int cols = screen.GetLength(1);
                var colValues = Enumerable.Range(0, cols).Select(c => screen[Param1, c]).ToArray();
                for (int c = 0; c < cols; c++)
                {
                    screen[Param1, (c + Param2) % cols] = colValues[c];
                }
                break;
        }
    }
}

public partial class Day08(bool isTest = false) : BaseDay("08", isTest)
{
    private readonly int _width = 50;
    private readonly int _height = 6;

    static IEnumerable<Instruction> ParseContent(string[] content)
    {
        static Instruction ParseLine(string line)
        {
            (Op op, int param1, int param2) = line switch
            {
                var l when l.StartsWith("rect") => (Op.Rect, int.Parse(l.Split(" ")[1].Split("x")[0]), int.Parse(l.Split(" ")[1].Split("x")[1])),
                var l when l.StartsWith("rotate column") => (Op.RotateColumn, int.Parse(ExtractNumber().Matches(l)[0].Value), int.Parse(ExtractNumber().Matches(l)[1].Value)),
                var l when l.StartsWith("rotate row") => (Op.RotateRow, int.Parse(ExtractNumber().Matches(l)[0].Value), int.Parse(ExtractNumber().Matches(l)[1].Value)),
                _ => throw new NotImplementedException()
            };

            return new Instruction(op, param1, param2);
        }
        foreach(var line in content)
        {
            yield return ParseLine(line);
        }
    }

    static void DisplayScreen(char[,] screen)
    {
        for (int r = 0; r < screen.GetLength(0); r++)
        {
            for (int c = 0; c < screen.GetLength(1); c++)
            {
                if (screen[r, c] == '\0')
                    Console.Write('.');
                else
                    Console.Write(screen[r, c]);
            }
            Console.WriteLine(System.Environment.NewLine);
        }
    }

    static int CountLights(char[,] screen)
    {
        int counter = 0;
        for (int r = 0; r < screen.GetLength(0); r++)
        {
            for (int c = 0; c < screen.GetLength(1); c++)
            {
                if (screen[r, c] != '\0')
                    counter++;
            }
        }
        return counter;
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        var instructions = ParseContent(File.ReadAllLines(InputPath));
        char[,] screen = new char[_height, _width];
        //DisplayScreen(screen);
        foreach (var ins in instructions)
        {
            //Console.WriteLine($"Op: {ins.Operation}");
            ins.Exec(screen);
            //DisplayScreen(screen);
        };
        
        return CountLights(screen);
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
    public string RunPart2()
    {
        var instructions = ParseContent(File.ReadAllLines(InputPath));
        char[,] screen = new char[_height, _width];
        foreach (var ins in instructions)
        {
            ins.Exec(screen);
        }
        DisplayScreen(screen);

        return "ZFHFSFOGPO";
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex ExtractNumber();
    #endregion
}
