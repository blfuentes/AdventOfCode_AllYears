open AdventOfCode_2016.Modules

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day03/test_input_01.txt"
let path = "day03/day03_input.txt"

let isValidTriangle (sides: int[]) =
    sides.[0] + sides.[1] > sides.[2] &&
    sides.[1] + sides.[2] > sides.[0] &&
    sides.[0] + sides.[2] > sides.[1]

let inputLines = LocalHelper.GetLinesFromFile(path)

let triangleDef = 
    inputLines 
    |> Array.map(fun t -> [|int(t.Substring(0, 5)); int(t.Substring(5, 5)); int(t.Substring(10, 5))|]) 

let idx0 = triangleDef |> Array.map(fun t -> t.[0])
let idx1 = triangleDef |> Array.map(fun t -> t.[1])
let idx2 = triangleDef |> Array.map(fun t -> t.[2])

let groups0 = idx0 |> Array.chunkBySize(3) |> Array.filter(fun t -> isValidTriangle t) |> Array.length
let groups1 = idx1 |> Array.chunkBySize(3) |> Array.filter(fun t -> isValidTriangle t) |> Array.length
let groups2 = idx2 |> Array.chunkBySize(3) |> Array.filter(fun t -> isValidTriangle t) |> Array.length

groups0 + groups1 + groups2