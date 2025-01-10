module day01_part02

open AdventOfCode_Utilities
open AdventOfCode_2022.Modules.LocalHelper

let path = "day01/day01_input.txt"

let execute =
    let inputLines = GetLinesFromFile(path) |> Seq.toList
    let elvesCalories = getLinesGroupBySeparatorWithEmptySpaceFixReplace inputLines "" |> List.map(fun l -> l |> (List.map int) |> List.sum)
    elvesCalories |> List.sortDescending |> List.take 3 |> List.sum