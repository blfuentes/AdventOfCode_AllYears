module day03_part02

open AdventOfCode_2020.Modules

let path = "day03/day03_input.txt"
let values = LocalHelper.GetLinesFromFile(path) |> Array.map (fun line -> line.ToCharArray())

let width = values.[0].Length
let height = values.Length

let trees =
    seq {
        for idx in [|0..height - 1|] do
            for jdx in [|0 .. width - 1|] do
                match values.[idx].[jdx] with
                    | '#' -> yield [|jdx; idx|]
                    | _  -> ()
    } |> List.ofSeq

let transportTrees (numPos: int) (input: list<int[]>) : list<int[]> =
    input |> List.map (fun t -> [|t.[0] + numPos * width; t.[1]|])

let slopesToCheck = [[|1; 1|]; [|3; 1|]; [|5; 1|]; [|7; 1|]; [|1; 2|]]

let execute =
    slopesToCheck |> List.map (fun s -> getCollisionsBasic trees 0 0 s.[0] s.[1] width height ) |> List.fold (*) 1