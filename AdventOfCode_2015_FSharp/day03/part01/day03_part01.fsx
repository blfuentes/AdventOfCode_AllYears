open System.IO

// let path = "test_input_01.txt"
// let path = "test_input_02.txt"
let path = "day03_input.txt"

let inputLine = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun l -> l.ToCharArray() |> Array.toList) |>Seq.toList

let rec countVisitedHouses (pos:int[]) (visited: list<int[]>) (steps: list<char>) =
    match steps with
    | '^'::b -> countVisitedHouses [|pos.[0]; pos.[1] - 1|] ([|pos.[0]; pos.[1] - 1|]::visited) b
    | 'v'::b -> countVisitedHouses [|pos.[0]; pos.[1] + 1|] ([|pos.[0]; pos.[1] + 1|]::visited) b
    | '>'::b -> countVisitedHouses [|pos.[0] + 1; pos.[1]|] ([|pos.[0] + 1; pos.[1]|]::visited) b
    | '<'::b -> countVisitedHouses [|pos.[0] - 1; pos.[1]|] ([|pos.[0] - 1; pos.[1]|]::visited) b
    | [] -> (visited |> List.distinct).Length
    | _ -> 0

countVisitedHouses [|0; 0|] [[|0; 0|]] inputLine.Head