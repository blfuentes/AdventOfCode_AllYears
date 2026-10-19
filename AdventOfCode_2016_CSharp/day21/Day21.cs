using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2016_CSharp.day21;

public class Day21 : BaseDay
{
    abstract record Operation
    {
        public abstract void Apply(Span<char> password);
        public abstract void Undo(Span<char> password);

        protected static void RotateLeft(Span<char> password, int steps)
        {
            int shift = steps % password.Length;
            password[..shift].Reverse();
            password[shift..].Reverse();
            password.Reverse();
        }

        protected static void Move(Span<char> password, int from, int to)
        {
            char c = password[from];
            if (from < to)
                password[(from + 1)..(to + 1)].CopyTo(password[from..]);
            else
                password[to..from].CopyTo(password[(to + 1)..]);
            password[to] = c;
        }
    }
    record SwapPosOp(int PosFrom, int PosTo) : Operation
    {
        public override void Apply(Span<char> password)
        {
            (password[PosFrom], password[PosTo]) = (password[PosTo], password[PosFrom]);
        }

        public override void Undo(Span<char> password)
        {
            (password[PosTo], password[PosFrom]) = (password[PosFrom], password[PosTo]);
        }
    }
    record SwapLetterOp(char LetterFrom, char LetterTo) : Operation
    {
        public override void Apply(Span<char> password)
        {
            (int posFrom, int posTo) = (password.IndexOf(LetterFrom), password.IndexOf(LetterTo));
            (password[posFrom], password[posTo]) = (password[posTo], password[posFrom]);
        }

        public override void Undo(Span<char> password)
        {
            (int posFrom, int posTo) = (password.IndexOf(LetterFrom), password.IndexOf(LetterTo));
            (password[posTo], password[posFrom]) = (password[posFrom], password[posTo]);
        }
    }

    record RotateStepsOp(bool Left, int Steps) : Operation
    {
        public override void Apply(Span<char> password)
        {
            int n = password.Length;
            RotateLeft(password, Left ? Steps : n - Steps % n);
        }

        public override void Undo(Span<char> password)
        {
            int n = password.Length;
            RotateLeft(password, Left ? n - Steps % n : Steps);
        }
    }

    record RotateIndexOp(char Letter) : Operation
    {
        public override void Apply(Span<char> password)
        {
            int index = password.IndexOf(Letter);
            int steps = 1 + index + ((index >= 4) ? 1 : 0);
            int n = password.Length;
            RotateLeft(password, n - steps % n);   // always right rotation
        }

        public override void Undo(Span<char> password)
        {
            int n = password.Length;
            Span<char> candidate = stackalloc char[n];
            Span<char> check = stackalloc char[n];
            for (int k = 0; k < n; k++)
            {
                password.CopyTo(candidate);
                RotateLeft(candidate, k);
                candidate.CopyTo(check);
                Apply(check);
                if (check.SequenceEqual(password))
                {
                    candidate.CopyTo(password);
                    return;
                }
            }
        }
    }

    record ReverseOp(int PosFrom, int PosTo) : Operation
    {
        public override void Apply(Span<char> password) =>
            password[PosFrom..(PosTo + 1)].Reverse();

        public override void Undo(Span<char> password) => Apply(password);
    }

    record MoveOp(int Pos, int TargetPos) : Operation
    {
        public override void Apply(Span<char> password) => Move(password, Pos, TargetPos);

        public override void Undo(Span<char> password) => Move(password, TargetPos, Pos);
    }

    private readonly string _startText;
    private readonly string _scrambledText;

    readonly Operation[] _operations;

    static Operation ParseLine(ReadOnlySpan<char> line)
    {
        if (line.StartsWith("swap position"))        // "swap position X with position Y"
            return new SwapPosOp(line[14] - '0', line[^1] - '0');
        if (line.StartsWith("swap letter"))          // "swap letter X with letter Y"
            return new SwapLetterOp(line[12], line[^1]);
        if (line.StartsWith("rotate based"))         // "... position of letter X"
            return new RotateIndexOp(line[^1]);
        if (line.StartsWith("rotate"))               // "rotate left 1 step"
        {
            bool left = line[7] == 'l';
            int steps = line[left ? 12 : 13] - '0';
            return new RotateStepsOp(left, steps);
        }
        if (line.StartsWith("reverse"))              // "reverse positions X through Y"
            return new ReverseOp(line[18] - '0', line[^1] - '0');
        return new MoveOp(line[14] - '0', line[^1] - '0'); // "move position X to position Y"
    }

    public Day21(bool isTest) : base("21", isTest)
    {
        _startText = isTest ? "abcde" : "abcdefgh";
        _scrambledText = isTest ? "decab" : "fbgdceah";
        List<Operation> ops = [];
        foreach (var line in Content.AsSpan().EnumerateLines())
            if (!line.IsEmpty)
                ops.Add(ParseLine(line));
            else
                break;

        _operations = [.. ops];
    }

    #region Part 1

    [Benchmark]
    public string RunPart1()
    {
        Span<char> password = [.. _startText];
        foreach (var op in _operations)
            op.Apply(password);

        return password.ToString();
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
        Span<char> password = [.. _scrambledText];
        for (int i = _operations.Length - 1; i >= 0; i--)
            _operations[i].Undo(password);

        return password.ToString();
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
