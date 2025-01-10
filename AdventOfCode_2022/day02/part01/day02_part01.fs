module day02_part01

open AdventOfCode_2022.Modules.LocalHelper

let path = "day02/day02_input.txt"

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

let execute =
    let inputLines = GetLinesFromFile(path) |> Array.toList
    let rounds = inputLines |> List.map(fun l -> l.Split(" "))
    rounds |> List.map(fun r -> calculateRoundScore 0 r) |> List.sum