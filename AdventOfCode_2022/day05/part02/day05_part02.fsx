open System.IO
open System.Collections.Generic
open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day05/test_input_01.txt"
let path = "day05/day05_input.txt"
let inputLines = GetLinesFromFile(path) |> Seq.toList

let content = Utilities.getGroupsOnSeparator inputLines ""
let (initDrawing, movements) = (content.Head, content.Tail.Head)
let movDefinitions = movements |> List.map(fun m -> [|(int)(m.Split(' ').[1]); (int)(m.Split(' ').[3]); (int)(m.Split(' ').[5])|])

let parseLines (lines: string list) =
    let numberOfTowers = lines.Head.Length / 4 + 1
    let indexes = [0..(numberOfTowers - 1)] |> List.map(fun i -> 1 + (i * 4))
    let chunks = lines |> List.mapi(fun idx l -> (idx, (l.ToCharArray() |> Array.toList)))
    let towers = Array.init numberOfTowers (fun v -> "")
    for l in chunks do
        for idx in indexes do
            if ['A'..'Z'] |> List.contains((snd l).Item(idx)) then
                towers.[idx / 4] <- towers.[idx / 4] + ((string)((snd l).Item(idx)))
    towers 

let towers = parseLines initDrawing

let performMove (towers: string []) (mov: int[]) =
    let transfer = System.String.Concat((towers.[mov.[1] - 1].Substring(0, mov.[0])).ToCharArray())
    towers.[mov.[2] - 1] <- transfer + towers.[mov.[2] - 1]
    towers.[mov.[1] - 1] <- towers.[mov.[1] - 1].Substring(mov.[0])
    towers

let rec runMovements (towers: string[]) (movs: int[] list) =
    match movs.Length with
    | 0 -> towers
    | _ -> 
        let newTowers = performMove towers movs.Head
        runMovements newTowers movs.Tail

let result = System.String.Concat(runMovements towers movDefinitions |> Array.map(fun w -> w.[0]))