open AdventOfCode_2023.Modules

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

let getExpandPoints (input: string list) =
    let allVerticalIdx = 
        seq {
            for col in [0..input.[0].Length - 1] do
                let allDots = input |> List.forall(fun l -> l.[col] = '.')
                if allDots then yield (int64)col
        } |> Seq.toList
    let allHorizontalIdx = 
        seq {
            for row in [0..input.Length - 1] do
                let allDots = input.[row].ToCharArray() |> Array.forall ((=) '.')
                if allDots then yield (int64)row
        } |> Seq.toList
    (allHorizontalIdx, allVerticalIdx)

let buildMap (lines: string list) =
    let array = Array2D.create (lines.Length) (lines.[0].Length) '.'
    let galaxies = new Dictionary<int array, int>()
    for rowIdx in 0..lines.Length - 1 do
        for colIdx in 0..lines.[0].Length - 1 do
            let value = lines.[rowIdx].[colIdx]
            array.[rowIdx, colIdx] <- value
            if value = '#' then
                //printfn "adding galaxy at %i, %i" rowIdx colIdx
                galaxies.Add([|rowIdx; colIdx|], galaxies.Count + 1)
    galaxies, array

let printMap (map: char[,]) =
    for rowIdx in 0..map.GetLength(0) - 1 do
        for colIdx in 0..map.GetLength(1) - 1 do
            printf "%c" map.[rowIdx, colIdx]
        printfn ""

let distance (a: int64 array) (b: int64 array) =
    let d = Math.Abs(a.[0] - b.[0]) + Math.Abs(a.[1] - b.[1])
    //printfn "Distance between %A and %A is %i" a b d
    d

let calculateDistance (init: int array) (goal: int array) (points: int64 list * int64 list) (range: int64) =
    let start = init |> Array.map int64
    let target = goal |> Array.map int64

    let hsExpanded = ((fst points) |> List.filter (fun p -> start.[0] > p)).Length |> int64
    let htExpanded = ((fst points) |> List.filter (fun p -> target.[0] > p)).Length |> int64

    let vsExpanded = ((snd points) |> List.filter (fun p -> start.[1] > p)).Length |> int64
    let vtExpanded = ((snd points) |> List.filter (fun p -> target.[1] > p)).Length |> int64

    let newStart = [|start.[0] + (range * hsExpanded - hsExpanded); start.[1] + (range * vsExpanded - vsExpanded)|]
    let newTarget = [|target.[0] + (range * htExpanded - htExpanded); target.[1] + (range * vtExpanded - vtExpanded)|]

    distance newStart newTarget

let execute =
    //let path = "day11/test_input_01.txt"
    let path = "day11/day11_input.txt"
    let lines = LocalHelper.ReadLines path |> List.ofSeq
    let pointsOfDouble = getExpandPoints lines
    let (galaxies, map) = buildMap lines
    //printMap map

    let expandRange = 1000000
    let combinations = Utilities.comb 2 (galaxies.Keys |> Seq.toList)
    let distances = combinations |> Seq.map (fun x -> calculateDistance x.[0] x.[1] pointsOfDouble expandRange) |> Seq.sum
    distances |> bigint
    //printMap map