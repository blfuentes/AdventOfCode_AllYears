open System.IO
open System
open System.Collections.Generic

let mDistance (origin: int[]) (dest: int[]) =
    Math.Abs(origin.[0] - dest.[0]) + Math.Abs(origin.[1] - dest.[1])

let getShortestDistance (point: int[]) (lCords: int array list) =
    let distances = lCords |> List.map(fun c -> (c.[0], mDistance point [|c.[1]; c.[2]|])) // check * distance
    distances |> List.sumBy(fun c -> snd c)

let createPanel size =
    let dict = new Dictionary<string, int>()
    for colIdx in 0..size-1 do
        for rowIdx in 0..size-1 do 
            dict.Add($"{rowIdx},{colIdx}", 0)
    dict

let calculateDistances (size: int) (coords: int array list) (panel: Dictionary<string, int>) =
    for colIdx in 0..size-1 do
        for rowIdx in 0..size-1 do
            let distances = getShortestDistance [|rowIdx; colIdx|] coords
            panel.Item($"{rowIdx},{colIdx}") <- distances

let count (size: int) (maxvalue: int) (panel: Dictionary<string, int>) = 
    let result = 
        seq {
            for colIdx in 0..size-1 do
                for rowIdx in 0..size-1 do
                    let value = panel.Item($"{rowIdx},{colIdx}")
                    if value < maxvalue then
                        yield value
        } |> Seq.length
    result

//let execute =
//let filepath = __SOURCE_DIRECTORY__ + @"./test_input.txt"
let filepath = __SOURCE_DIRECTORY__ + @"./day06_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"./day06_input2.txt"

let inputLines = File.ReadAllLines(filepath) |> List.ofArray
let coords = inputLines |> List.mapi(fun idx v -> [|idx; int(v.Split(", ")[0]); int(v.Split(", ")[1]); 0|])
let size = 400
let maxDistance = 10000
let panel = createPanel size
let counter = Array.create coords.Length 0
coords |> List.iter(fun  e -> panel.Item($"{e.[1]},{e.[2]}") <- e.[0])
calculateDistances size coords panel
count size maxDistance panel
