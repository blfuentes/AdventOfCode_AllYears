using BenchmarkDotNet.Attributes;

namespace AdventOfCode_2017_CSharp.day18;

public class Day18 : BaseDay
{
    public abstract record Operation
    {
        public abstract (long offset, long? recoveredSound) Execute(
            Dictionary<char, long> registers,
            ref long? lastPlayedSound);
        public abstract long ExecuteAlternative(
            Dictionary<char, long> registers,
            Queue<long> myQueue,
            Queue<long> otherQueue);

    }

    public record Snd : Operation
    {
        private readonly char _register;
        private readonly int? _value;

        public Snd(string frecuency)
        {
            if (int.TryParse(frecuency, out int f))
            {
                _value = f;
            }
            else
            {
                _register = frecuency[0];
            }
        }

        public override (long offset, long? recoveredSound) Execute(
            Dictionary<char, long> registers,
            ref long? lastPlayedSound)
        {
            lastPlayedSound = _value ?? registers[_register];
            return (1, null);
        }

        public override long ExecuteAlternative(
            Dictionary<char, long> registers,
            Queue<long> myQueue,
            Queue<long> otherQueue)
        {
            otherQueue.Enqueue(_value ?? registers[_register]);
            return 1;
        }
    }

    public record SetOp : Operation
    {
        private readonly char _registerTarget;
        private readonly char _registerOrigin;
        private readonly int? _value;

        public SetOp(string reg1, string reg2)
        {
            _registerTarget = reg1[0];
            if (int.TryParse(reg2, out int v))
            {
                _value = v;
            }
            else
            {
                _registerOrigin = reg2[0];
            }
        }

        public override (long offset, long? recoveredSound) Execute(
            Dictionary<char, long> registers,
            ref long? lastPlayedSound)
        {
            registers[_registerTarget] = _value ?? registers[_registerOrigin];
            return (1, null);
        }

        public override long ExecuteAlternative(
            Dictionary<char, long> registers,
            Queue<long> myQueue,
            Queue<long> otherQueue)
        {
            registers[_registerTarget] = _value ?? registers[_registerOrigin];

            return 1;
        }
    }

    public record Add : Operation
    {
        private readonly char _registerTarget;
        private readonly char _registerOrigin;
        private readonly int? _value;

        public Add(string reg1, string reg2)
        {
            _registerTarget = reg1[0];
            if (int.TryParse(reg2, out int v))
            {
                _value = v;
            }
            else
            {
                _registerOrigin = reg2[0];
            }
        }

        public override (long offset, long? recoveredSound) Execute(
            Dictionary<char, long> registers,
            ref long? lastPlayedSound)
        {
            registers[_registerTarget] += _value ?? registers[_registerOrigin];
            return (1, null);
        }

        public override long ExecuteAlternative(
            Dictionary<char, long> registers,
            Queue<long> myQueue,
            Queue<long> otherQueue)
        {
            registers[_registerTarget] += _value ?? registers[_registerOrigin];

            return 1;
        }
    }

    public record Mul : Operation
    {
        private readonly char _registerTarget;
        private readonly char _registerOrigin;
        private readonly int? _value;

        public Mul(string reg1, string reg2)
        {
            _registerTarget = reg1[0];
            if (int.TryParse(reg2, out int v))
            {
                _value = v;
            }
            else
            {
                _registerOrigin = reg2[0];
            }
        }

        public override (long offset, long? recoveredSound) Execute(
            Dictionary<char, long> registers,
            ref long? lastPlayedSound)
        {
            registers[_registerTarget] *= _value ?? registers[_registerOrigin];
            return (1, null);
        }

        public override long ExecuteAlternative(
            Dictionary<char, long> registers,
            Queue<long> myQueue,
            Queue<long> otherQueue)
        {
            registers[_registerTarget] *= _value ?? registers[_registerOrigin];

            return 1;
        }
    }

    public record Mod : Operation
    {
        private readonly char _registerTarget;
        private readonly char _registerOrigin;
        private readonly int? _value;

        public Mod(string reg1, string reg2)
        {
            _registerTarget = reg1[0];
            if (int.TryParse(reg2, out int v))
            {
                _value = v;
            }
            else
            {
                _registerOrigin = reg2[0];
            }
        }

        public override (long offset, long? recoveredSound) Execute(
            Dictionary<char, long> registers,
            ref long? lastPlayedSound)
        {
            registers[_registerTarget] %= _value ?? registers[_registerOrigin];
            return (1, null);
        }

        public override long ExecuteAlternative(
            Dictionary<char, long> registers,
            Queue<long> myQueue,
            Queue<long> otherQueue)
        {
            registers[_registerTarget] %= _value ?? registers[_registerOrigin];

            return 1;
        }
    }

    public record Rcv : Operation
    {
        private readonly char _register;
        private readonly int? _value;

        public Rcv(string reg)
        {
            if (int.TryParse(reg, out int v))
            {
                _value = v;
            }
            else
            {
                _register = reg[0];
            }
        }


        public override (long offset, long? recoveredSound) Execute(
            Dictionary<char, long> registers,
            ref long? lastPlayedSound)
        {
            long? recoveredSound = (_value ?? registers[_register]) != 0
                ? lastPlayedSound
                : null;
            return (1, recoveredSound);
        }

        public override long ExecuteAlternative(
            Dictionary<char, long> registers,
            Queue<long> myQueue,
            Queue<long> otherQueue)
        {
            if (myQueue.TryDequeue(out long v))
            {
                registers[_register] = v;
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public record Jgz : Operation
    {
        private readonly char _offsetRegister;
        private readonly char _checkRegister;
        private readonly int? _offsetValue;
        private readonly int? _checkValue;

        public Jgz(string reg1, string reg2)
        {
            if (int.TryParse(reg1, out int check))
            {
                _checkValue = check;
            }
            else
            {

                _checkRegister = reg1[0];
            }

            if (int.TryParse(reg2, out int offset))
            {
                _offsetValue = offset;
            }
            else
            {
                _offsetRegister = reg2[0];
            }
        }

        public override (long offset, long? recoveredSound) Execute(
            Dictionary<char, long> registers,
            ref long? lastPlayedSound)
        {
            bool doJump = (_checkValue ?? registers[_checkRegister]) > 0;
            long offset = doJump ? (_offsetValue ?? registers[_offsetRegister]) : 1;

            return (offset, null);
        }

        public override long ExecuteAlternative(
            Dictionary<char, long> registers,
            Queue<long> myQueue,
            Queue<long> otherQueue)
        {
            bool doJump = (_checkValue ?? registers[_checkRegister]) > 0;
            return doJump ? (_offsetValue ?? registers[_offsetRegister]) : 1;
        }
    }

    private readonly Operation[] _operations;


    public Day18(bool isTest = false) : base("18", isTest)
    {
        var lines = File.ReadAllLines(InputPath);
        _operations = new Operation[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            var parts = lines[i].Split(" ");
            Operation op = parts[0] switch
            {
                "snd" => new Snd(parts[1]),
                "set" => new SetOp(parts[1], parts[2]),
                "add" => new Add(parts[1], parts[2]),
                "mul" => new Mul(parts[1], parts[2]),
                "mod" => new Mod(parts[1], parts[2]),
                "rcv" => new Rcv(parts[1]),
                "jgz" => new Jgz(parts[1], parts[2]),
                _ => throw new ArgumentOutOfRangeException("op", "invalid operation")
            };
            _operations[i] = op;
        }
    }

    #region Part 1
    [Benchmark]
    public long RunPart1()
    {
        Dictionary<char, long> _registers = [];
        for (int i = 0; i < 26; i++)
        {
            _registers.Add((char)('a' + i), 0);
        }

        long opIdx = 0;
        long? lastPlayedSound = null;
        while (opIdx >= 0 && opIdx < _operations.Length)
        {
            (long offset, long? recoveredSound) =
                _operations[opIdx].Execute(_registers, ref lastPlayedSound);
            opIdx += offset;
            if (recoveredSound.HasValue)
            {
                return recoveredSound.Value;
            }
        }

        return 0;
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
    public long RunPart2()
    {
        Dictionary<char, long> _registersA = [];
        Dictionary<char, long> _registersB = [];

        for (int i = 0; i < 26; i++)
        {
            _registersA.Add((char)('a' + i), 0);
            _registersB.Add((char)('a' + i), 0);
        }

        _registersB['p'] = 1;

        long opAIdx = 0;
        long opBIdx = 0;
        long timesB = 0;

        Queue<long> queueA = [];
        Queue<long> queueB = [];

        bool deadlock = false;

        while (!deadlock)
        {
            // A operating
            long offsetA = _operations[opAIdx].ExecuteAlternative(_registersA, queueA, queueB);
            opAIdx += offsetA;

            // B operating
            long offsetB = _operations[opBIdx].ExecuteAlternative(_registersB, queueB, queueA);
            if (_operations[opBIdx] is Snd)
                timesB++;
            opBIdx += offsetB;

            deadlock = offsetA == 0 && offsetB == 0;
        }
        return timesB;
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
