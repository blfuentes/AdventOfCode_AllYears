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

let needwin = [[|"A"; "Z"|]; [|"B"; "Z"|]; [|"C"; "Z"|]]
let needdraw = [[|"A"; "Y"|]; [|"B"; "Y"|]; [|"C"; "Y"|]]
let needlost = [[|"A"; "X"|]; [|"B"; "X"|]; [|"C"; "X"|]]

let getPoint(figure: string) =
    match figure with
    | res when res = "A" || res = "X" -> 1
    | res when res = "B" || res = "Y" -> 2
    | res when res = "C" || res = "Z" -> 3
    | _ -> 0

let getCounterPoint(figure: string) (result: int) =
    match figure with
    | res when res = "A" && result = -1 -> getPoint("C")    // Rock vs Scissor -> Lost
    | res when res = "A" && result = 0 -> getPoint("A")     // Rock vs Rock -> Draw
    | res when res = "A" && result = 1 -> getPoint("B")     // Rock vs Paper -> Win
    | res when res = "B" && result = -1 -> getPoint("A")    // Paper vs Rock -> Lost
    | res when res = "B" && result = 0 -> getPoint("B")     // Paper vs Paper -> Draw
    | res when res = "B" && result = 1 -> getPoint("C")     // Paper vs Scissor -> Win
    | res when res = "C" && result = -1 -> getPoint("B")    // Scissor vs Paper -> Lost
    | res when res = "C" && result = 0 -> getPoint("C")     // Scissor vs Scissor -> Draw
    | res when res = "C" && result = 1 -> getPoint("A")     // Scissor vs Rock -> Win
    | _ -> 0

let calculateRoundScore (round: int) (play: string[]) =
    let result = 
        match needwin |> List.contains(play) with
        | true -> (getCounterPoint (play.[0]) 1) + 6
        | false -> 
            match needdraw |> List.contains(play) with
            | true -> (getCounterPoint(play.[0]) 0) + 3
            | false -> 
                match needlost |> List.contains(play) with
                | true -> (getCounterPoint(play.[0]) -1) + 0
                | false -> 0
    result

rounds |> List.map(fun r -> calculateRoundScore 0 r) |> List.sum