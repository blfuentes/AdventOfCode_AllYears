﻿module day22_part02

open AdventOfCode_2015.Modules
    
type Enemy = {
    HitPoints: int
    Damage: int
}

type Spell = { 
    Name : string
    Cost : int
    Damage : int
    Heal : int
    Armor : int
    Mana : int
    Duration : int 
}

let parseContent(lines: string array) =
    let hitpoints = (int)(lines[0].Split(" ")[2])
    let damage = (int)(lines[1].Split(" ")[1])
    { HitPoints = hitpoints; Damage = damage }

let allSpells = [ 
    { Name = "Magic Missile"; Cost = 53;  Damage = 4; Heal = 0; Armor = 0; Mana = 0;   Duration = 1}
    { Name = "Drain";         Cost = 73;  Damage = 2; Heal = 2; Armor = 0; Mana = 0;   Duration = 1}
    { Name = "Shield";        Cost = 113; Damage = 0; Heal = 0; Armor = 7; Mana = 0;   Duration = 6}
    { Name = "Poison";        Cost = 173; Damage = 3; Heal = 0; Armor = 0; Mana = 0;   Duration = 6}
    { Name = "Recharge";      Cost = 229; Damage = 0; Heal = 0; Armor = 0; Mana = 101; Duration = 5} 
]

let cheapestWin (hp, mana) (enemy: Enemy) =
    let rec play myTurn best spent (hp, mana, spells) (enemy: Enemy) : int =
        // if we have already spent more than the known cheapest win, bail
        if spent >= best then best else

        // part 2, check if I die before any effects play out
        if  myTurn && hp = 1 then best else

        // apply effects
        let mana  = (spells |> List.sumBy (fun s -> s.Mana)) + mana
        let damage = spells |> List.sumBy (fun s -> s.Damage)
        let armor  = spells |> List.sumBy (fun s -> s.Armor)

        // does the boss die from effects?
        let bossHp = enemy.HitPoints - damage
        if bossHp <= 0 then spent else

        // decrement duration of all effects, and groom expired ones
        let spells =
            spells
            |> List.map (fun s -> {s with Duration = s.Duration - 1})
            |> List.filter (fun s -> s.Duration > 0)

        if myTurn then
            // part 2, I lose 1 HP on my turn
            let hp = hp - 1

            // what spells can I afford and don't already have running?
            let affordableSpells = 
                allSpells 
                |> List.filter (fun s -> (s.Cost <= mana) && not (spells |> List.exists (fun s' -> s.Name = s'.Name)))

            match affordableSpells with
            | [] -> best
            | buyableSpells ->
                // play out the rest of the game with each possible purchase
                let mutable newBest = best
                for s in buyableSpells do
                    let extraDamage, heal, spells = 
                        if s.Duration = 1 then s.Damage, s.Heal, spells
                        else 0, 0, s :: spells
                    let spent = spent + s.Cost
                    let mana = mana - s.Cost
                    let hp = hp + heal
                
                    let bossHp = bossHp - extraDamage
                    if bossHp <= 0 then newBest <- min newBest spent
                    else newBest <- min newBest (play false newBest spent (hp, mana, spells) { enemy with HitPoints = bossHp })
                newBest
        // boss's turn
        else
            let damage = max (enemy.Damage - armor) 1
            let hp = hp - damage
            if hp <= 0 then best else
            play true best spent (hp, mana, spells) { enemy with HitPoints = bossHp }

    play true 999999 0 (hp, mana, []) enemy

let execute =
    let input = "./day22/day22_input.txt"
    let content = LocalHelper.GetLinesFromFile input
    let enemy = parseContent content
    (cheapestWin (50, 500) enemy)