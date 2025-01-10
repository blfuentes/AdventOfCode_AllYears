open System.IO
open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day02/test_input_01.txt"
let path = "day02/day02_input.txt"
let inputLines = GetLinesFromFile(path) |> Array.toList
let rounds = inputLines |> List.map(fun l -> l.Split(" "))

let win = [[|"A"; "Y"|]; [|"B"; "Z"|]; [|"C"; "X"|]]
let draw = [[|"A"; "X"|]; [|"B"; "Y"|]; [|"C"; "Z"|]]
let lost = [[|"A"; "Z"|]; [|"B"; "X"|]; [|"C"; "Y"|]]

let getPoint(figure: string) =
    match figure with
    | res when res = "A" || res = "X" -> 1
    | res when res = "B" || res = "Y" -> 2
    | res when res = "C" || res = "Z" -> 3
    | _ -> 0

let calculateRoundScore (round: int) (play: string[]) =
    let result = 
        match win |> List.contains(play) with
        | true -> getPoint(play.[1]) + 6
        | false -> 
            match draw |> List.contains(play) with
            | true -> getPoint(play.[1]) + 3
            | false -> 
                match lost |> List.contains(play) with
                | true -> getPoint(play.[1]) + 0
                | false -> 0
    result

rounds |> List.map(fun r -> calculateRoundScore 0 r) |> List.sum