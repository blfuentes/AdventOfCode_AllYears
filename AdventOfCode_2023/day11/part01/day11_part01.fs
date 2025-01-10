module day11_part01

open System
open System.Collections.Generic

open AdventOfCode_2023.Modules
open AdventOfCode_Utilities

let prepareLines (lines: string list) =    
    let rec loop (lines: string list) (acc: string list) =
        match lines with
        | [] -> acc
        | head :: tail ->
            if head.ToCharArray() |> Array.forall ((=) '.') then
                loop tail (acc @ [head] @ [head])
            else
                loop tail (acc @ [head])
    let rec insertElementAt (acc:int) (idx: int list) (input: string)  (element: string) =
        match idx with
        | [] -> input
        | head :: tail ->
            let idx = head + acc
            let newInput = input.Insert(idx, element)
            insertElementAt (acc + 1) tail newInput element

    let doubleH = loop lines []
    let allVerticalIdx = 
        seq {
            for col in [0..doubleH.[0].Length - 1] do
                let allDots = doubleH |> List.forall(fun l -> l.[col] = '.')
                if allDots then yield col
        } |> Seq.toList
    let doubleV = doubleH |> List.map(fun l ->
        insertElementAt 0 allVerticalIdx l "."
    )
    doubleV


let buildMap (lines: string list) =
    let array = Array2D.create (lines.Length) (lines.[0].Length) '.'
    let galaxies = new Dictionary<int array, int>()
    for rowIdx in 0..lines.Length - 1 do
        for colIdx in 0..lines.[0].Length - 1 do
            let value = lines.[rowIdx].[colIdx]
            array.[rowIdx, colIdx] <- value
            if value = '#' then
                galaxies.Add([|rowIdx; colIdx|], galaxies.Count + 1)
    galaxies, array

let printMap (map: char[,]) =
    for rowIdx in 0..map.GetLength(0) - 1 do
        for colIdx in 0..map.GetLength(1) - 1 do
            printf "%c" map.[rowIdx, colIdx]
        printfn ""

let distance (a: int array) (b: int array) =
    let d = Math.Abs(a.[0] - b.[0]) + Math.Abs(a.[1] - b.[1])
    d

let execute =
    let path = "day11/day11_input.txt"
    let lines = LocalHelper.ReadLines path |> List.ofSeq
    let lines' = prepareLines lines
    let (galaxies, map) = buildMap lines'

    let combinations = Utilities.comb 2 (galaxies.Keys |> Seq.toList)
    let distances = combinations |> Seq.map (fun x -> distance x.[0] x.[1]) |> Seq.sum
    distances
    