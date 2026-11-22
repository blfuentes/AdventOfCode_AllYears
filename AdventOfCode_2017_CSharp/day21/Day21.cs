using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace AdventOfCode_2017_CSharp.day21;

public class Day21 : BaseDay
{
    private readonly char[][] _initPattern = [
        [..".#."],
        [.."..#"],
        [.."###"]
    ];

    private readonly Dictionary<string, char[][]> _enhacementRules;

    static char[][] Rotate90Clockwise(char[][] m)
    {
        var matrix = m.Select(row => (char[])row.Clone()).ToArray();
        int n = matrix.Length;

        // Transpose
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                (matrix[i][j], matrix[j][i]) = (matrix[j][i], matrix[i][j]);
            }
        }

        // Reverse each row
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n / 2; j++)
            {
                (matrix[i][j], matrix[i][n - 1 - j]) =
                    (matrix[i][n - 1 - j], matrix[i][j]);
            }
        }

        return matrix;
    }

    static char[][] FlipHorizontal(char[][] m)
    {
        var matrix = m.Select(row => (char[])row.Clone()).ToArray();
        int n = m.GetLength(0);

        for (int i = 0; i < n / 2; i++)
        {
            for (int j = 0; j < n; j++)
            {
                (matrix[i][j], matrix[n - i - 1][j]) =
                    (matrix[n - i - 1][j], matrix[i][j]);
            }
        }

        return matrix;
    }

    static char[][] FlipVertical(char[][] m)
    {
        var matrix = m.Select(row => (char[])row.Clone()).ToArray();
        int n = m.GetLength(0);

        for (int j = 0; j < n / 2; j++)
        {
            for (int i = 0; i < n; i++)
            {
                (matrix[i][j], matrix[i][n - j - 1]) =
                    (matrix[i][n - j - 1], matrix[i][j]);
            }
        }

        return matrix;
    }

    static char[][] ReflectMainDiagonal(char[][] m)
    {
        int n = m.Length;
        char[][] result = new char[n][];

        for (int i = 0; i < n; i++)
        {
            result[i] = new char[n];
            for (int j = 0; j < n; j++)
                result[i][j] = m[j][i];
        }

        return result;
    }

    static List<char[][]> BuildPatterns(string[] pattern)
    {
        char[][] original = [.. pattern.Select(row => ((string)row.Clone()).ToCharArray())];
        char[][] turn90 = Rotate90Clockwise(original);
        char[][] turn180 = Rotate90Clockwise(turn90);
        char[][] turn270 = Rotate90Clockwise(turn180);

        char[][] flipHorizontal = FlipHorizontal(original);
        char[][] flipVertical = FlipVertical(original);

        char[][] antiDiagonal = FlipHorizontal(Rotate90Clockwise(original));
        char[][] mainDiagonal = ReflectMainDiagonal(original);

        return [
            original,
            turn90,
            turn180,
            turn270,
            flipHorizontal,
            flipVertical,
            antiDiagonal,
            mainDiagonal
        ];
    }
    public Day21(bool isTest = false) : base("21", isTest)
    {
        var lines = File.ReadAllLines(InputPath);

        _enhacementRules = [];

        foreach (var line in lines)
        {
            (string left, string right) = (line.Split(" => ")[0], line.Split(" => ")[1]);
            var leftparts = left.Split("/");
            char[][] rightrule = [.. right.Split("/").Select(r => r.ToCharArray())];
            var patterns = BuildPatterns(leftparts).Select(KeyOf);
            foreach (var pattern in patterns.Distinct())
            {
                _enhacementRules.Add(pattern, rightrule);
            }
        }
    }

    static string KeyOf(char[][] matrix)
    {
        string[] parts = [.. matrix.Select(m => new string(m))];
        return string.Join("", parts);
    }

    static List<char[][]> BreakPixels(char[][] pixels)
    {
        int length = pixels[0].Length;
        int separator = length % 2 == 0 ? 2 : 3;

        List<char[][]> sections = [];

        for (int iS = 0; iS < length / separator; iS++)
        {
            for (int jS = 0; jS < length / separator; jS++)
            {
                char[][] sec = new char[separator][];
                for (int i = 0; i < separator; i++)
                {
                    sec[i] = new char[separator];
                    for (int j = 0; j < separator; j++)
                    {
                        sec[i][j] =
                            pixels[i + iS * separator][j + jS * separator];
                    }
                }
                sections.Add(sec);
            }
        }

        return sections;
    }

    char[][] FindPixeles(char[][] pixeles)
    {
        return _enhacementRules[KeyOf(pixeles)];
    }

    char[][] Iteration(char[][] pattern)
    {
        var brokenPixeles = BreakPixels(pattern);
        List<char[][]> mutations = [];
        foreach (var pixeles in brokenPixeles)
        {
            mutations.Add(FindPixeles(pixeles));
        }

        var dimension = (int)Math.Sqrt(mutations.Count);
        var sectionSize = mutations[0].Length;
        var size = dimension * mutations[0].Length;
        char[][] iteration = new char[size][];
        List<char[][][]> chunks = [.. mutations.Chunk(dimension)];
        for (int dr = 0; dr < chunks.Count; dr++)
        {
            for (int dc = 0; dc < chunks.Count; dc++)
            {
                for (int r = 0; r < sectionSize; r++)
                {
                    if (iteration[r + dr * sectionSize] is null)
                        iteration[r + dr * sectionSize] = new char[size];

                    for (int c = 0; c < sectionSize; c++)
                    {
                        iteration[r + dr * sectionSize][c + dc * sectionSize] =
                            chunks[dr][dc][r][c];
                    }
                }
            }
        }

        return iteration;
    }

    static void PrintGrid(char[][] pixeles)
    {
        for (int i = 0; i < pixeles.Length; i++)
        {
            Console.WriteLine($"{new string(pixeles[i])}");
        }
    }

    static int CountPixels(char[][] pixels)
    {
        return pixels.Sum(p => p.Count(c => c == '#'));
    }

    #region Part 1
    [Benchmark]
    public int RunPart1()
    {
        int iterations = 1;
        var mutation = _initPattern;
        while (iterations <= 5)
        {
            mutation = Iteration(mutation);
            iterations++;
        }
        return CountPixels(mutation);
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
        int iterations = 1;
        var mutation = _initPattern;
        while (iterations <= 18)
        {
            mutation = Iteration(mutation);
            iterations++;
        }

        return CountPixels(mutation);
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
