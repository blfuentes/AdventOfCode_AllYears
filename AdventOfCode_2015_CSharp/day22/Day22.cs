using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode_2015_CSharp.day22;

public partial class Day22(bool isTest = false) : BaseDay("22", isTest)
{
    record Spell
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int Heal { get; set; }
        public int Armor { get; set; }
        public int Mana { get; set; }
        public int Duration { get; set; }
        public Spell CopyOf()
        {
            return new Spell()
            {
                Name = Name,
                Cost = Cost,
                Damage = Damage,
                Heal = Heal,
                Armor = Armor,
                Mana = Mana,
                Duration = Duration
            };
        }
    }
    record Player
    {
        public int Id { get; set; }
        public int HitPoints { get; set; }
    }
    record Boss : Player
    {
        public int Damage { get; set; }
    }
    record User : Player
    {
        public int Mana { get; set; }
    }

    readonly List<Spell> Spells = [
        new() { Name = "Magic missile", Cost = 53,  Damage = 4, Heal = 0, Armor = 0, Mana = 0,   Duration = 1 },
        new() { Name = "Drain",         Cost = 73,  Damage = 2, Heal = 2, Armor = 0, Mana = 0,   Duration = 1 },
        new() { Name = "Shield",        Cost = 113, Damage = 0, Heal = 0, Armor = 7, Mana = 0,   Duration = 6 },
        new() { Name = "Poison",        Cost = 173, Damage = 3, Heal = 0, Armor = 0, Mana = 0,   Duration = 6 },
        new() { Name = "Recharge",      Cost = 229, Damage = 0, Heal = 0, Armor = 0, Mana = 101, Duration = 5 }
    ];

    int CheapestWin(User user, Boss boss, bool hardMode = false)
    {
        int Play(bool playerTurn, int best, int spent, int currentMana, int currentHp, IEnumerable<Spell> spells, Boss enemy)
        {
            if (spent >= best)
                return best;

            // hardmode!!
            if (hardMode && playerTurn && currentHp == 1)
                return best;

            // apply effects
            var dotMana = spells.Sum(_ => _.Mana) + currentMana;
            var dotDamage = spells.Sum(_ => _.Damage);
            var dotArmor = spells.Sum(_ => _.Armor);

            // dots damage
            var bossHp = enemy.HitPoints - dotDamage;
            if (bossHp <= 0)
            {
                return spent;
            }

            // consume active spells
            spells = [.. spells.Select(s => s with { Duration = s.Duration - 1 }).Where(s => s.Duration > 0)];

            if (playerTurn)
            {
                var possibleSpells =
                    Spells.Where(s => s.Cost <= dotMana &&
                    !spells.Select(sp => sp.Name).Contains(s.Name))
                    .Select(s => s.CopyOf()).ToList();

                if (possibleSpells.Count == 0)
                {
                    return best;
                }

                var newBest = best;
                foreach (var s in possibleSpells)
                {
                    (int extraDamage, int heal, IEnumerable<Spell> newSpells) =
                        s.Duration == 1 ?
                            (s.Damage, s.Heal, spells) :
                            (0, 0, spells.Append(s));

                    var newSpent = spent + s.Cost;
                    var newMana = dotMana - s.Cost;
                    var newHp = currentHp + heal - (hardMode ? 1 : 0);

                    var newBossHp = bossHp - extraDamage;
                    if (newBossHp <= 0)
                    {
                        newBest = Math.Min(newBest, newSpent);
                    }
                    else
                    {
                        var played = Play(
                                playerTurn: false,
                                newBest,
                                newSpent,
                                newMana,
                                newHp,
                                newSpells,
                                enemy with { HitPoints = newBossHp });
                        newBest = Math.Min(newBest, played);
                    }
                }

                return newBest;
            }
            else
            { // boss turn
                var newUserHp = currentHp - Math.Max(enemy.Damage - dotArmor, 1);

                if (newUserHp <= 0)
                {
                    return best;
                }
                return Play(playerTurn: true,
                    best,
                    spent,
                    dotMana,
                    newUserHp,
                    spells,
                    enemy with { HitPoints = bossHp });
            }
        }

        return Play(playerTurn: true, 999999, 0, user.Mana, user.HitPoints, [], boss);
    }

    #region Part 1

    [Benchmark]
    public int RunPart1()
    {
        var numbers = ExtractNumbers().Matches(Content);
        Boss boss = new()
        {
            Id = 0,
            HitPoints = int.Parse(numbers[0].Value),
            Damage = int.Parse(numbers[1].Value)
        };
        User user = new()
        {
            Id = 1,
            HitPoints = 50,
            Mana = 500
        };

        return CheapestWin(user, boss);
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
        var numbers = ExtractNumbers().Matches(Content);
        Boss boss = new()
        {
            Id = 0,
            HitPoints = int.Parse(numbers[0].Value),
            Damage = int.Parse(numbers[1].Value)
        };
        User user = new()
        {
            Id = 1,
            HitPoints = 50,
            Mana = 500
        };

        return CheapestWin(user, boss, hardMode: true);
    }

    public override string SolvePart2()
    {
        StopWatch.Start();
        var result = RunPart2();
        StopWatch.Stop();
        return $"Final result Day {Day} part 2: {result} in {Utils.FormatTime(StopWatch.ElapsedTicks)}.";
    }

    [GeneratedRegex(@"\d+")]
    private static partial Regex ExtractNumbers();
    #endregion
}
