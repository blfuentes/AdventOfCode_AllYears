using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace AdventOfCode_2015_CSharp.day21;

public class Day21(bool isTest = false) : BaseDay("21", isTest)
{
    List<Weapon> Weapons = [
        new Weapon() { Name = "Dagger", Cost = 8, Damage= 4, Armor = 0 },
        new Weapon() { Name = "Shortsword", Cost = 10, Damage= 5, Armor = 0 },
        new Weapon() { Name = "Warhammer", Cost = 25, Damage= 6, Armor = 0 },
        new Weapon() { Name = "Longsword", Cost = 40, Damage= 7, Armor = 0 },
        new Weapon() { Name = "Greataxe", Cost = 74, Damage= 8, Armor = 0 },
    ];
    List<Chest> Armors = [
        new Chest() { Name = "Leather", Cost = 13, Damage= 0, Armor = 1 },
        new Chest() { Name = "Chainmail", Cost = 31, Damage= 0, Armor = 2 },
        new Chest() { Name = "Splintmail", Cost = 53, Damage= 0, Armor = 3 },
        new Chest() { Name = "Bandedmail", Cost = 75, Damage= 0, Armor = 4 },
        new Chest() { Name = "Platemail", Cost = 102, Damage= 0, Armor = 5 },
    ];
    List<Ring> Rings = [
        new Ring() { Name = "Damage +1", Cost = 25, Damage= 1, Armor = 0 },
        new Ring() { Name = "Damage +2", Cost = 50, Damage= 2, Armor = 0 },
        new Ring() { Name = "Damage +3", Cost = 100, Damage= 3, Armor = 0 },
        new Ring() { Name = "Defense +1", Cost = 20, Damage= 0, Armor = 1 },
        new Ring() { Name = "Defense +2", Cost = 40, Damage= 0, Armor = 2 },
        new Ring() { Name = "Defense +3", Cost = 80, Damage= 0, Armor = 3 },
    ];

    record Item
    {
        public string Name { get; set; }
    }
    record Weapon : Item
    {
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
    }

    record Chest : Item
    {
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
    }

    record Ring : Item
    {
        public int Cost { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
    }

    record Player
    {
        public int Id { get; set; }
        public int HitPoints { get; set; }
        public int Damage { get; set; }
        public int Armor { get; set; }
    }

    record Equipment
    {
        public Weapon Weapon { get; set; }
        public Chest? Armor { get; set; }
        public List<Ring> Rings { get; set; }

        public int PriceValue =>
            Weapon.Cost +
            (Armor?.Cost ?? 0) +
            Rings.Select(r => r.Cost).Sum();

        public int ArmorValue => (Armor?.Armor ?? 0) + Rings.Select(r => r.Armor).Sum();
        public int DamageValue => Weapon.Damage + Rings.Select(r => r.Damage).Sum();
    }

    #region Part 1
    Player GetWinner(Player user, Player boss)
    {
        while (true)
        {
            boss.HitPoints -= Math.Max(1, user.Damage - boss.Armor);
            //Console.WriteLine($"User deals {user.Damage}-{boss.Armor}={user.Damage - boss.Armor}. Boss goes down to {boss.HitPoints}");
            if (boss.HitPoints <= 0)
                return user;
            // Boss turn
            user.HitPoints -= Math.Max(1, boss.Damage - user.Armor);
            //Console.WriteLine($"Boss deals {boss.Damage}-{user.Armor}={boss.Damage - user.Armor}. Player goes down to {user.HitPoints}");
            if (user.HitPoints <= 0)
                return boss;
        }
    }

    IEnumerable<IEnumerable<Ring>> GetRings()
    {
        for (int numberOfrings = 0; numberOfrings <= 2; numberOfrings++)
        {
            if (numberOfrings == 0)
            {
                yield return Enumerable.Empty<Ring>();
            }
            else
            {
                for (int r1Idx = 0; r1Idx < Rings.Count; r1Idx++)
                {
                    if (numberOfrings == 1)
                    {
                        yield return [Rings[r1Idx]];
                    }
                    else
                    {
                        for(int r2Idx = r1Idx+1; r2Idx < Rings.Count; r2Idx++)
                        {
                            yield return [Rings[r1Idx], Rings[r2Idx]];
                        }
                    }
                }
            }
        }

        yield break;
    }

    IEnumerable<Equipment> GetEquipment()
    {
        foreach (var weapon in Weapons)
        {
            for (int aIdx = 0; aIdx <= Armors.Count; aIdx++)
            {
                foreach(var rings in GetRings())
                {
                    yield return new Equipment()
                    {
                        Weapon = weapon,
                        Armor = aIdx == 0 ? null : Armors[aIdx-1],
                        Rings = [..rings]
                    };
                }
            }
        }
        yield break;
    }

    [Benchmark]
    public int RunPart1()
    {
        var numbers = Regex.Matches(Content, @"\d+");
        int minValue = int.MaxValue;
        
        foreach (var equipment in GetEquipment().OrderBy(_ => _.PriceValue))
        {
            //Console.WriteLine($"Current cost: {equipment.PriceValue}");
            Player boss = new()
            {
                Id = 0,
                HitPoints = int.Parse(numbers[0].Value),
                Damage = int.Parse(numbers[1].Value),
                Armor = int.Parse(numbers[2].Value),
            };

            Player user = new()
            {
                Id = 1,
                HitPoints = 100,
                Armor = equipment.ArmorValue,
                Damage = equipment.DamageValue,
            };
            var winner = GetWinner(user, boss);

            if (winner.Id > 0 && equipment.PriceValue < minValue)
            {
                //Console.WriteLine($"Winner with cost: {equipment.PriceValue}");
                minValue = equipment.PriceValue;
                break;
            }
        }
        return minValue;
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
        var numbers = Regex.Matches(Content, @"\d+");
        int maxValue = 0;

        foreach (var equipment in GetEquipment().OrderByDescending(_ => _.PriceValue))
        {
            //Console.WriteLine($"Current cost: {equipment.PriceValue}");
            Player boss = new()
            {
                Id = 0,
                HitPoints = int.Parse(numbers[0].Value),
                Damage = int.Parse(numbers[1].Value),
                Armor = int.Parse(numbers[2].Value),
            };

            Player user = new()
            {
                Id = 1,
                HitPoints = 100,
                Armor = equipment.ArmorValue,
                Damage = equipment.DamageValue,
            };
            var winner = GetWinner(user, boss);

            if (winner.Id == 0)
            {
                //Console.WriteLine($"Loser with cost: {equipment.PriceValue}");
                maxValue = equipment.PriceValue;
                break;
            }
        }
        return maxValue;
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
