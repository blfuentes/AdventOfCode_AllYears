module day03_part01


open AdventOfCode_2016.Modules.LocalHelper

let path = "day03/day03_input.txt"

let isValidTriangle (sides: int[]) =
    sides.[0] + sides.[1] > sides.[2] &&
    sides.[1] + sides.[2] > sides.[0] &&
    sides.[0] + sides.[2] > sides.[1]

let execute =
    let inputLines = GetLinesFromFile(path)
    inputLines 
        |> Array.map(fun t -> [|int(t.Substring(0, 5)); int(t.Substring(5, 5)); int(t.Substring(10, 5))|]) 
        |> Array.filter(fun e -> isValidTriangle e) |> Array.length