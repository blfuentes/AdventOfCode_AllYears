module day02_part01


open AdventOfCode_2016.Modules.LocalHelper

let path = "day02/day02_input.txt"

let keyboard =[|[|1; 2; 3|]; [|4; 5; 6|]; [|7; 8; 9|]|]

let rec findButton (movs: char list) (kb: int[][]) (current: int[]) (result: int list)=
    match movs with
    | [] -> (current, kb.[current.[0]].[current.[1]])
    | head :: tail ->
        match head with
        | 'U' -> 
            if current.[0] > 0 then current.[0] <- current.[0] - 1
            findButton tail kb current result
        | 'D' ->
            if current.[0] < kb.Length - 1 then current.[0] <- current.[0] + 1
            findButton tail kb current result
        | 'L' ->
            if current.[1] > 0 then current.[1] <- current.[1] - 1
            findButton tail kb current result
        | 'R' -> 
            if current.[1] < kb.[0].Length - 1 then current.[1] <- current.[1] + 1
            findButton tail kb current result
        | _ -> (current, kb.[current.[0]].[current.[1]])


let rec resolveDigit (input: string list) (kb: int[][]) (init: int[]) (result: int list) =
    match input with
    | [] -> result
    | head :: tail ->
        let newButton = findButton (head.ToCharArray() |> List.ofArray) kb init result
        [snd newButton] @ resolveDigit tail kb (fst newButton) []

let execute =
    let inputLines = GetLinesFromFile(path) |> List.ofArray
    System.String.Concat(resolveDigit inputLines keyboard [|1; 1|] [] |> List.map(fun i -> i.ToString()))