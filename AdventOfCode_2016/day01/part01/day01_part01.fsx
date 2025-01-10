#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_2016.Modules

// let path = "day01/text_input_03.txt"
let path = "day01/day01_input.txt"
let inputLines = LocalHelper.GetContentFromFile(path)

let getOp (value: string) =
    (value.Trim().Substring(0, 1), int(value.Trim().Substring(1)))
// getOp "L3"
let getNewPosition (init: string * int[]) (op: string * int) =
    match fst init with
    | "N" -> 
        match fst op with 
        | "R" -> ("R", [|(snd init).[0]; (snd init).[1] + snd op|])
        | "L" -> ("L", [|(snd init).[0]; (snd init).[1] - snd op|])
        | _ -> init
    | "S" -> 
        match fst op with 
        | "R" -> ("L", [|(snd init).[0]; (snd init).[1] - snd op|])
        | "L" -> ("R", [|(snd init).[0]; (snd init).[1] + snd op|])
        | _ -> init
    | "L" ->
        match fst op with 
        | "R" -> ("N", [|(snd init).[0] - snd op; (snd init).[1]|])
        | "L" -> ("S", [|(snd init).[0] + snd op; (snd init).[1]|])
        | _ -> init
    | "R" ->
        match fst op with 
        | "R" -> ("S", [|(snd init).[0] + snd op; (snd init).[1]|])
        | "L" -> ("N", [|(snd init).[0] - snd op; (snd init).[1]|])
        | _ -> init
    |_ -> init

let rec getPosition (ops: (string * int) list) (currentPos: string * int[]) =
    match ops with
    | [] -> currentPos
    | head :: tail -> getPosition tail (getNewPosition currentPos head)

let operations = inputLines.Split(',') |> Array.map(fun op -> getOp op) |> List.ofArray
let initPos = ("N", [|0; 0|])
let finalPos = getPosition operations initPos
Math.Abs((snd finalPos).[0]) + Math.Abs((snd finalPos).[1])
// getNewPosition initPos operations.[0]