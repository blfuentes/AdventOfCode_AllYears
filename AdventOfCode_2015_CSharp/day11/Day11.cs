using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Parsers.ClrPrivate;
using System.ComponentModel;

namespace AdventOfCode_2015_CSharp.day11;

public class Day11(bool isTest = false) : BaseDay("11", isTest)
{
    static bool ThreeFollowing(char[] password)
    {
        for (int i = 0; i < password.Length - 2; i++)
        {
            if (password[i + 2] - password[i + 1] == 1 && password[i + 1] - password[i] == 1)
                return true;
        }

        return false;
    }

    static readonly char[] forbidden = ['i', 'o', 'l'];
    static bool NotForbidden(char[] password)
    {
        return !password.Intersect(forbidden).Any();
    }

    static bool ContainPairs(char[] password)
    {
        int pairs = 0;
        char prev = '-';
        char current;
        for (int i = 0; i < password.Length; i++)
        {
            current = password[i];
            if (i > 0 && current == prev)
            {
                pairs++;
                prev = '-';
            }
            else
                prev = current;

            if (pairs == 2)
                return true;
        }

        return false;
    }

    void Increment(char[] passwword)
    {
        bool overflow = true;
        for (int i = passwword.Length - 1; overflow; i--)
        {
            char tmpChar = ++passwword[i];
            if (tmpChar > 'z')
            {
                passwword[i] = 'a';
                overflow = true;
            }
            else
            {
                passwword[i] = tmpChar;
                overflow = false;
            }
        }
    }

    private void GeneratePassword(char[] password)
    {
        do
        {
            Increment(password);
            while (!NotForbidden(password))
            {
                bool first = true;
                for (int i = 0; i < password.Length; i++)
                {
                    if (!first)
                        password[i] = 'a';
                    else
                    {
                        if (forbidden.Contains(password[i]))
                        {
                            if (first)
                            {
                                password[i]++;
                                first = false;
                            }
                        }
                    }
                }
            }
        } while (!ThreeFollowing(password) || !ContainPairs(password));
    }

    #region Part 1

    [Benchmark]
    public string RunPart1()
    {
        char[] password = Content.ToCharArray();
        GeneratePassword(password);

        return string.Join("", password);
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
        char[] password = Content.ToCharArray();
        GeneratePassword(password);
        GeneratePassword(password);
        return string.Join("", password);
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
