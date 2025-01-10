module day21_part02

open AdventOfCode_2015.Modules
open AdventOfCode_Utilities

type Kind =
    | Weapon
    | Armor
    | Ring

type Stats = {
    Name: string
    Cost: int
    Damage: int
    Armor: int
}

type ArmorPart = {
    Statistics: Stats
    What: Kind
}

type ArmorSet = {
    AttackWeapon: ArmorPart
    Equip: ArmorPart
    Ring1: ArmorPart
    Ring2: ArmorPart
}

type Enemy = {
    HitPoints: int
    Damage: int
    Armor: int
}

let parseContent(lines: string array) =
    let hitpoints = (int)(lines[0].Split(" ")[2])
    let damage = (int)(lines[1].Split(" ")[1])
    let armor = (int)(lines[2].Split(" ")[1])
    { HitPoints = hitpoints; Damage = damage; Armor = armor}

let weapons = [
    { What = Weapon; Statistics = { Name = "Dagger"; Cost = 8; Damage = 4; Armor = 0 } };
    { What = Weapon; Statistics = { Name = "Shortsword"; Cost = 10; Damage = 5; Armor = 0 } };
    { What = Weapon; Statistics = { Name = "Warhammer"; Cost = 25; Damage = 6; Armor = 0 } };
    { What = Weapon; Statistics = { Name = "Longsword"; Cost = 40; Damage = 7; Armor = 0 } };
    { What = Weapon; Statistics = { Name = "Greataxe"; Cost = 74; Damage = 8; Armor = 0 } }
]

let armors = [
    { What = Armor; Statistics = { Name = "None"; Cost = 0; Damage = 0; Armor = 0 } };

    { What = Armor; Statistics = { Name = "Leather"; Cost = 13; Damage = 0; Armor = 1 } };
    { What = Armor; Statistics = { Name = "Chainmail"; Cost = 31; Damage = 0; Armor = 2 } };
    { What = Armor; Statistics = { Name = "Splintmail"; Cost = 53; Damage = 0; Armor = 3 } };
    { What = Armor; Statistics = { Name = "Bandedmail"; Cost = 75; Damage = 0; Armor = 4 } };
    { What = Armor; Statistics = { Name = "Platemail"; Cost = 102; Damage = 0; Armor = 5 } }
]

let rings = [
    { What = Ring; Statistics = { Name = "NoneLeft"; Cost = 0; Damage = 0; Armor = 0 } };
    { What = Ring; Statistics = { Name = "NoneRight"; Cost = 0; Damage = 0; Armor = 0 } };

    { What = Ring; Statistics = { Name = "Damage +1"; Cost = 25; Damage = 1; Armor = 0 } };
    { What = Ring; Statistics = { Name = "Damage +2"; Cost = 50; Damage = 2; Armor = 0 } };
    { What = Ring; Statistics = { Name = "Damage +3"; Cost = 100; Damage = 3; Armor = 0 } };
    { What = Ring; Statistics = { Name = "Defense +1"; Cost = 20; Damage = 0; Armor = 1 } };
    { What = Ring; Statistics = { Name = "Defense +2"; Cost = 40; Damage = 0; Armor = 2 } };
    { What = Ring; Statistics = { Name = "Defense +3"; Cost = 80; Damage = 0; Armor = 3 } }
]

let allArmorySets =
    let setofRings = 
        Utilities.combination 2 rings
        |> List.map(fun rings ->
            (rings.Item(0), rings.Item(1))
        )

    let partialArmor = 
        Utilities.combinationsOfLists [weapons; armors]
        |> List.map(fun armorparts ->
            match armorparts with
            | [weapon; armor] when weapon.What.IsWeapon && armor.What.IsArmor -> (weapon, armor)
            | [armor; weapon] when armor.What.IsArmor && weapon.What.IsWeapon -> (weapon, armor)
            | _ -> failwith "error"
        )

    let possible = Utilities.combinationsOfLists [partialArmor; setofRings]
    
    possible
    |> List.map(fun parts ->
        let (weapon, armor) = (fst (parts.Item(0)), snd (parts.Item(0)))
        let (ringleft, ringright) = (fst (parts.Item(1)), snd (parts.Item(1)))

        (weapon, armor, ringleft, ringright)
    )

let fight(enemy: Enemy) (equipments: (ArmorPart*ArmorPart*ArmorPart*ArmorPart) list) =
    let tryEquip (en: Enemy) ((weapon, armor, ringleft, ringright): ArmorPart*ArmorPart*ArmorPart*ArmorPart) =
        let damage = weapon.Statistics.Damage + armor.Statistics.Damage + ringleft.Statistics.Damage + ringright.Statistics.Damage
        let armorqt = weapon.Statistics.Armor + armor.Statistics.Armor + ringleft.Statistics.Armor + ringright.Statistics.Armor
        let price = weapon.Statistics.Cost + armor.Statistics.Cost + ringleft.Statistics.Cost + ringright.Statistics.Cost
        let mutable hitpoints = 100
        let mutable currentFight = en
        let mutable docontinue = currentFight.HitPoints >= 0
        while docontinue do
            let newHitpoints = currentFight.HitPoints - (damage - enemy.Armor)
            currentFight <- {currentFight with HitPoints = newHitpoints }
            if currentFight.HitPoints >= 0 then
                hitpoints <- hitpoints - (currentFight.Damage - armorqt)
            if hitpoints <= 0 || currentFight.HitPoints <= 0 then
                docontinue <- false
        (currentFight.HitPoints > 0, price)
        
    equipments
    |> List.map(fun e -> 
        tryEquip enemy e
    )
    |> List.filter(fun (r, p) -> r)
    |> List.map snd
    |> List.sortDescending
    |> List.head

let execute =
    let input = "./day21/day21_input.txt"
    let content = LocalHelper.GetLinesFromFile input
    let enemy = parseContent content
    let all = allArmorySets
    fight enemy all