module day15_part01

open AdventOfCode_2015.Modules

type Ingredient = {
    Name: string
    Capacity : int
    Durability : int
    Flavor: int
    Texture: int
    Calories: int
}

let parseContent(lines: string array) =
    lines
    |> Array.map(fun l ->
        let (name, parts) = (l.Split(": ")[0], l.Split(": ")[1])
        let values = parts.Split(", ")
        {   Name = name;
            Capacity = (int)(values[0].Split(" ")[1]);
            Durability = (int)(values[1].Split(" ")[1]);
            Flavor = (int)(values[2].Split(" ")[1]);
            Texture = (int)(values[3].Split( " ")[1]);
            Calories = (int)(values[4].Split(" ")[1])
        }
    )

let teaspoonsCombs =
    [
        for f in 0..100 do
            for c in 0..(100 - f) do
                for b in 0..(100 - f - c) do
                    let s = 100 - f - c - b
                    yield (f, c, b, s)
    ]

let findBestCombination((combis, ingredients): ((int*int*int*int) list*Ingredient array)) =
    let mutable bestCurrent = 0

    let first = ingredients[0]
    let second = ingredients[1]
    let third = ingredients[2]
    let fourth = ingredients[3]

    for (f, c, b, s) in combis do
        let capacity = f * first.Capacity + c * second.Capacity + b * third.Capacity + s * fourth.Capacity
        let durability = f * first.Durability + c * second.Durability + b * third.Durability + s * fourth.Durability
        let flavor = f * first.Flavor + c * second.Flavor + b * third.Flavor + s * fourth.Flavor
        let texture = f * first.Texture + c * second.Texture + b * third.Texture + s * fourth.Texture
        
        let total = 
            (if capacity < 0 then 0 else capacity) * 
            (if durability < 0 then 0 else durability) * 
            (if flavor < 0 then 0 else flavor) * 
            (if texture < 0 then 0 else texture)

        if total > bestCurrent then
            bestCurrent <- total

    bestCurrent

let execute =
    let input = "./day15/day15_input.txt"
    let content = LocalHelper.GetLinesFromFile input

    let ingredients = parseContent content
    findBestCombination(teaspoonsCombs, ingredients)