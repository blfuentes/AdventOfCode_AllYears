module day03_part01

open AdventOfCode_Utilities
open AdventOfCode_2022.Modules.LocalHelper

let path = "day03/day03_input.txt"

let getValueOfElement (input: char) =
    let minorletters = ['a'..'z'] |> List.mapi(fun idx c -> (c, idx + 1))
    let capitalleters = ['A'..'Z'] |> List.mapi(fun idx c -> (c, idx + 27))
    snd ((minorletters @ capitalleters) |> List.find(fun e -> (fst e) = input))

let execute =
    let inputLines = GetLinesFromFile(path) |> Array.toList
    let rucksacks = inputLines |> List.map splitString
    rucksacks |> 
        List.map Utilities.commonElements |> 
        List.map(fun c -> c |> Seq.toList) |> 
        List.sumBy(fun l -> l |> List.sumBy(fun s -> getValueOfElement s))