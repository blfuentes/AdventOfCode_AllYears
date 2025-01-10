open AdventOfCode_2020.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules.Helpers
open AdventOfCode_Utilities

//let file = "test_input.txt"
let file = "day10_input.txt"
let path = "day10/" + file
let inputLines = GetLinesFromFile(path) |> Array.map int |> Array.sort |> List.ofArray

let diff(a, b) =
    b - a

let maxValue = inputLines.[inputLines.Length - 1] + 3
let result = [0] @ inputLines @ [maxValue] |> List.pairwise |> List.map (diff) |> List.groupBy (fun x -> x)

printf "%i %A" (fst (result.Item(0))) (snd (result.Item(0))).Length
printf "%i %A" (fst (result.Item(1))) (snd (result.Item(1))).Length

(snd (result.Item(0))).Length * (snd(result.Item(1))).Length