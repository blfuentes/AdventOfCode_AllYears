module day05_part02

open AdventOfCode_Utilities
open AdventOfCode_2015.Modules.LocalHelper

let path = "day05/day05_input.txt"

let inputLines = GetLinesFromFile(path) |>Seq.toList

let pairIsRepeatedNoOverlapping (input: string) =
    let combinations = input.ToCharArray() |> Array.toList |> List.pairwise |> List.map(fun p -> string (fst p) + string (snd p))
    let filtered = combinations |> List.mapi(fun idx c -> (c, idx)) |> List.groupBy(fun g -> fst g) |> List.filter(fun g -> (snd g).Length > 1)
    let filteredReduced = filtered |> List.map(fun l -> (fst l, (snd l |> List.map snd))) |> List.map snd
    (filteredReduced |> List.filter(fun e -> (comb 2 e) |> List.exists(fun n -> not (areConsecutive n)))).Length > 0

let mirroredLetter (input: string) =
    let listOfThrees = [0..input.Length - 3] |> List.map(fun i -> input.Substring(i).ToCharArray() |> Array.take(3))
    (listOfThrees |> List.filter (fun l -> l.[0] = l.[2])).Length > 0

let isNiceStringNewRules (input: string) =
    pairIsRepeatedNoOverlapping input && mirroredLetter input

let execute =
    (inputLines |> List.filter(fun l -> isNiceStringNewRules l)).Length