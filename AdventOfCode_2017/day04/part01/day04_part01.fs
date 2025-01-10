module day04_part01


open AdventOfCode_2017.Modules.LocalHelper

let path = "day04/day04_input.txt"

let execute() =
    let input = GetLinesFromFile path
    input |> Array.map (fun l -> l.Split(' ') |> Array.countBy id) 
        |> Array.filter(fun g -> g |> Array.forall(fun (k, c) -> c = 1)) |> Array.length