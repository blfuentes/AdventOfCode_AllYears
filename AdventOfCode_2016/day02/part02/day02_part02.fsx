open AdventOfCode_2016.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day02/test_input_01.txt"
let path = "day02/day02_input.txt"

let keyboard =[|
    [|'$'; '$'; '1'; '$'; '$'|]; 
    [|'$'; '2'; '3'; '4'; '$'|]; 
    [|'5'; '6'; '7'; '8'; '9'|]; 
    [|'$'; 'A'; 'B'; 'C'; '$'|]; 
    [|'$'; '$'; 'D'; '$'; '$'|]
    |]

let rec findButton (movs: char list) (kb: char[][]) (current: int[]) (result: char list)=
    match movs with
    | [] -> (current, kb.[current.[0]].[current.[1]])
    | head :: tail ->
        match head with
        | 'U' -> 
            let newPos = [|current.[0] - 1; current.[1]|]
            if current.[0] > 0 && kb.[newPos.[0]].[newPos.[1]] <> '$' then current.[0] <- current.[0] - 1
            findButton tail kb current result
        | 'D' ->
            let newPos = [|current.[0] + 1; current.[1]|]
            if current.[0] < kb.Length - 1 && kb.[newPos.[0]].[newPos.[1]] <> '$' then current.[0] <- current.[0] + 1
            findButton tail kb current result
        | 'L' ->
            let newPos = [|current.[0]; current.[1] - 1|]
            if current.[1] > 0 && kb.[newPos.[0]].[newPos.[1]] <> '$'then current.[1] <- current.[1] - 1
            findButton tail kb current result
        | 'R' -> 
            let newPos = [|current.[0]; current.[1] + 1|]
            if current.[1] < kb.[0].Length - 1 && kb.[newPos.[0]].[newPos.[1]] <> '$' then current.[1] <- current.[1] + 1
            findButton tail kb current result
        | _ -> (current, kb.[current.[0]].[current.[1]])


let rec resolveDigit (input: string list) (kb: char[][]) (init: int[]) (result: char list) =
    match input with
    | [] -> result
    | head :: tail ->
        let newButton = findButton (head.ToCharArray() |> List.ofArray) kb init result
        [snd newButton] @ resolveDigit tail kb (fst newButton) []

let inputLines = GetLinesFromFile(path) |> List.ofArray
System.String.Concat(resolveDigit inputLines keyboard [|2; 0|] [] |> List.map(fun i -> i.ToString()))