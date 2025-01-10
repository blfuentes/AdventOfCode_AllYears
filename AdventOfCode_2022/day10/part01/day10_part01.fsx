open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day10/test_input_01.txt"
// let path = "day10/test_input_02.txt"
let path = "day10/day10_input.txt"

let inputLines = GetLinesFromFile(path)
let instructions = inputLines |> Array.map(fun l -> if l.Split(' ').[0] = "noop" then (1, 0) else (2, (int)(l.Split(' ').[1]))) |> Array.toList

let rec runOp (op: int * int) (currentValue: int) (listOfCycles: int list) =
    match fst op > 1 with
    | false -> 
        let lastValue = currentValue + snd op
        (lastValue, listOfCycles @ [lastValue])
    | true -> 
        let newListOfCycles = listOfCycles @ [currentValue]
        runOp ((fst op) - 1, snd op) currentValue newListOfCycles

//runOp (1, 0) 1 []
// runOp (2, 3) 1 1

let rec performInstructions (ins: (int * int) list) (currentValue: int) (listOfCycles: int list)=
    match ins with
    | head :: tail ->
        let (newCurrentValue, newListOfCycles) = runOp head currentValue listOfCycles
        performInstructions tail newCurrentValue newListOfCycles
    | [] -> listOfCycles

let initValue = 1
let initlistOfCycles = [1]
let result = performInstructions instructions initValue initlistOfCycles

let _20th = result.Item(19) * 20
let _60th = result.Item(59) * 60
let _100th = result.Item(99) * 100
let _140th = result.Item(139) * 140
let _180th = result.Item(179) * 180
let _220th = result.Item(219) * 220
let signalStrength = (_20th + _60th + _100th + _140th + _180th + _220th)