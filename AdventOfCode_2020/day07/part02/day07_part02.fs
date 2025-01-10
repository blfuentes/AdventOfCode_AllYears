module day07_part02

open AdventOfCode_2020.Modules


let path = "day07/day07_input.txt"

let inputLines = LocalHelper.GetLinesFromFile(path)
let elements = parseBagsInput inputLines |> List.ofSeq
let shinyBag = elements |> List.filter(fun b -> b.Name = "shiny gold") |> List.head

let execute =
    countBags 0 shinyBag.Content elements