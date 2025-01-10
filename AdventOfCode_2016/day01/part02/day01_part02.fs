module day01_part02

open System

open AdventOfCode_2016.Modules.LocalHelper

let path = "day01/day01_input.txt"

let getOp (value: string) =
    (value.Trim().Substring(0, 1), int(value.Trim().Substring(1)))

let getNewPosition (init: string * int[]) (op: string * int) =
    match fst init with
    | "N" -> 
        match fst op with 
        | "R" -> 
            let visited = 
                seq {
                    for idx in 1..snd op do
                        yield [|(snd init).[0]; (snd init).[1] + idx|]
                } |> List.ofSeq |> List.rev
            // visited
            ("R", visited)
        | "L" -> 
            let visited = 
                seq {
                    for idx in 1..snd op do
                        yield [|(snd init).[0]; (snd init).[1] - idx|]
                } |> List.ofSeq |> List.rev
            // visited
            ("L", visited)
        | _ -> ("N", [snd init])
    | "S" -> 
        match fst op with 
        | "R" -> 
            let visited = 
                seq {
                    for idx in 1..snd op do
                        yield [|(snd init).[0]; (snd init).[1] - idx|]
                } |> List.ofSeq |> List.rev
            ("L", visited)
        | "L" -> 
            let visited = 
                seq {
                    for idx in 1..snd op do
                        yield [|(snd init).[0]; (snd init).[1] + idx|]
                } |> List.ofSeq |> List.rev
            ("R", visited)
        | _ -> ("S", [snd init])
    | "L" ->
        match fst op with 
        | "R" -> 
            let visited = 
                seq {
                    for idx in 1..snd op do
                        yield [|(snd init).[0] - idx; (snd init).[1]|]
                } |> List.ofSeq |> List.rev
            ("N", visited)
        | "L" -> 
            let visited = 
                seq {
                    for idx in 1..snd op do
                        yield [|(snd init).[0] + idx; (snd init).[1]|]
                } |> List.ofSeq |> List.rev
            ("S", visited)
        | _ -> ("L", [snd init])
    | "R" ->
        match fst op with 
        | "R" ->
            let visited = 
                seq {
                    for idx in 1..snd op do
                        yield [|(snd init).[0] + idx; (snd init).[1]|]
                } |> List.ofSeq |> List.rev
            ("S", visited)
        | "L" -> 
            let visited = 
                seq {
                    for idx in 1..snd op do
                        yield [|(snd init).[0] - idx; (snd init).[1]|]
                } |> List.ofSeq |> List.rev
            ("N", visited)
        | _ -> ("R", [snd init])
    |_ -> (fst init, [snd init])

let rec getPosition (ops: (string * int) list) (currentPos: string * int[])  (visited: int[] list)=
    match ops with
    | [] -> snd currentPos
    | head :: tail -> 
        let newPos = (getNewPosition currentPos head)
        let intsersec = visited |> List.tryFind(fun v -> (snd newPos) |> List.exists(fun p -> p = v))
        match intsersec with
        | Some(found) -> found
        | None -> getPosition tail (fst newPos, (snd newPos).Head) (visited @ (snd newPos))

let execute =
    let inputLines = GetContentFromFile(path)
    let operations = inputLines.Split(',') |> Array.map(fun op -> getOp op) |> List.ofArray
    let initPos = ("N", [|0; 0|])
    let finalPos = getPosition operations initPos [[|0; 0|]]
    Math.Abs(finalPos.[0]) + Math.Abs(finalPos.[1])