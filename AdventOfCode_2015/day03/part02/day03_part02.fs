module day03_part02

open AdventOfCode_Utilities
open AdventOfCode_2015.Modules.LocalHelper

let path = "day03/day03_input.txt"

let inputLine = GetLinesFromFile(path) |> Seq.map(fun l -> l.ToCharArray() |> Array.toList) |>Seq.toList

let rec getVisitedHouses (pos:int[]) (visited: list<int[]>) (steps: list<char>) =
    match steps with
    | '^'::b -> getVisitedHouses [|pos.[0]; pos.[1] - 1|] ([|pos.[0]; pos.[1] - 1|]::visited) b
    | 'v'::b -> getVisitedHouses [|pos.[0]; pos.[1] + 1|] ([|pos.[0]; pos.[1] + 1|]::visited) b
    | '>'::b -> getVisitedHouses [|pos.[0] + 1; pos.[1]|] ([|pos.[0] + 1; pos.[1]|]::visited) b
    | '<'::b -> getVisitedHouses [|pos.[0] - 1; pos.[1]|] ([|pos.[0] - 1; pos.[1]|]::visited) b
    | [] -> (visited |> List.distinct)
    | _ -> []
                            

let execute =
    let dealers = splitEvenOddList(inputLine.Head)
    ((getVisitedHouses [|0; 0|] [[|0; 0|]] (fst dealers) @ getVisitedHouses [|0; 0|] [[|0; 0|]] (snd dealers)) |> List.distinct).Length