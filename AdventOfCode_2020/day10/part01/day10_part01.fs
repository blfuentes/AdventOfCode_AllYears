module day10_part01


open AdventOfCode_2020.Modules

let path = "day10/day10_input.txt"

let inputLines = LocalHelper.GetLinesFromFile(path) |> Array.map int |> Array.sort |> List.ofArray

let diff(a, b) =
    b - a

let execute =
    let maxValue = inputLines.[inputLines.Length - 1] + 3
    let result = [0] @ inputLines @ [maxValue] |> List.pairwise |> List.map (diff) |> List.groupBy (fun x -> x)
    (snd (result.Item(0))).Length * (snd(result.Item(1))).Length