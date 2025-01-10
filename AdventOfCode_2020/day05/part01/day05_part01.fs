module day05_part01

open AdventOfCode_2020.Modules


let path = "day05/day05_input.txt"
let inputLines = LocalHelper.GetLinesFromFile(path)

let execute =
    inputLines |> List.ofArray |> List.map (fun s -> calculateBinarySeat s) |> List.max