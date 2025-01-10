module day03_part01

open AdventOfCode_2015.Modules.LocalHelper

let path = "day03/day03_input.txt"

let inputLine = GetLinesFromFile(path) |> Seq.map(fun l -> l.ToCharArray() |> Array.toList) |>Seq.toList

let rec countVisitedHouses (pos:int[]) (visited: list<int[]>) (steps: list<char>) =
    match steps with
    | '^'::b -> countVisitedHouses [|pos.[0]; pos.[1] - 1|] ([|pos.[0]; pos.[1] - 1|]::visited) b
    | 'v'::b -> countVisitedHouses [|pos.[0]; pos.[1] + 1|] ([|pos.[0]; pos.[1] + 1|]::visited) b
    | '>'::b -> countVisitedHouses [|pos.[0] + 1; pos.[1]|] ([|pos.[0] + 1; pos.[1]|]::visited) b
    | '<'::b -> countVisitedHouses [|pos.[0] - 1; pos.[1]|] ([|pos.[0] - 1; pos.[1]|]::visited) b
    | [] -> (visited |> List.distinct).Length
    | _ -> 0

let execute =
    countVisitedHouses [|0; 0|] [[|0; 0|]] inputLine.Head