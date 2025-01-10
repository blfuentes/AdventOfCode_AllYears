module day01_part01

open System

open AdventOfCode_2016.Modules.LocalHelper

let path = "day01/day01_input.txt"

let getOp (value: string) =
    (value.Trim().Substring(0, 1), int(value.Trim().Substring(1)))

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

let execute =
    let inputLines = GetContentFromFile(path)
    let operations = inputLines.Split(',') |> Array.map(fun op -> getOp op) |> List.ofArray
    let initPos = ("N", [|0; 0|])
    let finalPos = getPosition operations initPos
    Math.Abs((snd finalPos).[0]) + Math.Abs((snd finalPos).[1])