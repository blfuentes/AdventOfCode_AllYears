module day00_part00

open AdventOfCode_2021.Modules.LocalHelper

let path = "day00/day00_input.txt"
let inputLines = GetLinesFromFile(path) |> List.ofArray |> List.map (int)

let execute =
    0