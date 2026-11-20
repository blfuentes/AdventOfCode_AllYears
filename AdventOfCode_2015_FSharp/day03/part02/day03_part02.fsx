open System.IO

// let path = "test_input_01.txt"
// let path = "test_input_02.txt"
let path = "day03_input.txt"

let inputLine = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun l -> l.ToCharArray() |> Array.toList) |>Seq.toList

let rec getVisitedHouses (pos:int[]) (visited: list<int[]>) (steps: list<char>) =
    match steps with
    | '^'::b -> getVisitedHouses [|pos.[0]; pos.[1] - 1|] ([|pos.[0]; pos.[1] - 1|]::visited) b
    | 'v'::b -> getVisitedHouses [|pos.[0]; pos.[1] + 1|] ([|pos.[0]; pos.[1] + 1|]::visited) b
    | '>'::b -> getVisitedHouses [|pos.[0] + 1; pos.[1]|] ([|pos.[0] + 1; pos.[1]|]::visited) b
    | '<'::b -> getVisitedHouses [|pos.[0] - 1; pos.[1]|] ([|pos.[0] - 1; pos.[1]|]::visited) b
    | [] -> (visited |> List.distinct)
    | _ -> []

let everyNth n l = 
  l |> List.mapi (fun i el -> el, i)              // Add index to element
    |> List.filter (fun (el, i) -> i % n = n - 1) // Take every nth element
    |> List.map fst                               // Drop index from the result

let splitEvenOddList (l: 'a list) = 
  let listwithIdx =
    l |> List.mapi (fun i el -> el, i)                // Add index to element
  ((listwithIdx |> List.filter (fun (el, i) -> i % 2 = 0)) |> List.map fst, listwithIdx |> List.filter (fun (el, i) -> i % 2 <> 0) |> List.map fst)

let dealers = splitEvenOddList(inputLine.Head)
((getVisitedHouses [|0; 0|] [[|0; 0|]] (fst dealers) @ getVisitedHouses [|0; 0|] [[|0; 0|]] (snd dealers)) |> List.distinct).Length