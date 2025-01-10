module day04_part02

open AdventOfCode_Utilities
open AdventOfCode_2022.Modules.LocalHelper

let path = "day04/day04_input.txt"

let isOverlapped (lists: list<array<int>>) =
    let commonElements = Utilities.commonElements lists
    commonElements.Count > 0

let execute =
    let inputLines = GetLinesFromFile(path) |> Seq.toList
    let elvesPairs = inputLines |> List.map(fun l -> [[|(int)(l.Split(',').[0].Split('-').[0])..(int)(l.Split(',').[0].Split('-').[1])|]; [|(int)(l.Split(',').[1].Split('-').[0])..(int)(l.Split(',').[1].Split('-').[1])|]])
    elvesPairs |> List.sumBy(fun e -> if (isOverlapped e) then 1 else 0 )