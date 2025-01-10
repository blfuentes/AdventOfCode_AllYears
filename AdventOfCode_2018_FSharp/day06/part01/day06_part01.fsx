open System.IO
open System
open System.Collections.Generic

let mDistance (origin: int[]) (dest: int[]) =
    Math.Abs(origin.[0] - dest.[0]) + Math.Abs(origin.[1] - dest.[1])

let getShortestDistance (point: int[]) (lCords: int array list) =
    let distances = lCords |> List.map(fun c -> (c.[0], mDistance point [|c.[1]; c.[2]|])) // check * distance
    let groupedDistance = distances |> List.groupBy snd // distance * (check * distance)
    (groupedDistance |> List.sortBy fst).Head

let createPanel size =
    let dict = new Dictionary<string, int>()
    for colIdx in 0..size-1 do
        for rowIdx in 0..size-1 do 
            dict.Add($"{rowIdx},{colIdx}", -1)
    dict
            

let printPanel size (panel: Dictionary<string, int>) =
    for colIdx in 0..size-1 do
        for rowIdx in 0..size-1 do 
            if panel.ContainsKey($"{rowIdx},{colIdx}") then
                let value = panel.Item($"{rowIdx},{colIdx}")
                printf $"{value, 2}" 
        printfn ""

let calculateDistances (size: int) (coords: int array list) (panel: Dictionary<string, int>) =
    for colIdx in 0..size-1 do
        for rowIdx in 0..size-1 do
            let distances = getShortestDistance [|rowIdx; colIdx|] coords
            if (fst distances) > 0 then 
                if (snd distances).Length = 1 then
                    panel.Item($"{rowIdx},{colIdx}") <- fst (snd distances).Head
                else
                    panel.Item($"{rowIdx},{colIdx}") <- -1
            else
                panel.Item($"{rowIdx},{colIdx}") <- fst (snd distances).Head

let getBoundaries (size: int) (panel: Dictionary<string, int>) =
    let leftBoundaries = 
        seq {
            for rowIdx in 0..size-1 do
                yield panel.Item($"{rowIdx},{0}")
        } |> Seq.distinct |> List.ofSeq
    let topBoundaries = 
        seq {
            for coldIdx in 0..size-1 do
                yield panel.Item($"{0},{coldIdx}")
        } |> Seq.distinct |> List.ofSeq
    let rightBoundaries = 
        seq {
            for rowIdx in 0..size-1 do
                yield panel.Item($"{rowIdx},{size-1}")
        } |> Seq.distinct |> List.ofSeq
    let bottomBoundaries = 
        seq {
            for coldIdx in 0..size-1 do
                yield panel.Item($"{size-1},{coldIdx}")
        } |> Seq.distinct |> List.ofSeq
    (leftBoundaries @ topBoundaries @ rightBoundaries @ bottomBoundaries) |> List.distinct


let count (coords: int array list) (size: int) (panel: Dictionary<string, int>) (counter: int array)
    (boundaries: int list) =
    for colIdx in 0..size-1 do
        for rowIdx in 0..size-1 do
            let value = panel.Item($"{rowIdx},{colIdx}")
            printfn "Checking %i, %i with value %i" rowIdx colIdx value
            if panel.Item($"{rowIdx},{colIdx}") > -1 && (not (boundaries |> List.contains value)) then
                counter.[panel.Item($"{rowIdx},{colIdx}")] <- counter.[panel.Item($"{rowIdx},{colIdx}")] + 1
                printfn "Element %i size %i" (panel.Item($"{rowIdx},{colIdx}")) counter.[panel.Item($"{rowIdx},{colIdx}")]
    (counter |> Array.sortDescending).[0]

//let execute =
//let filepath = __SOURCE_DIRECTORY__ + @"./test_input.txt"
let filepath = __SOURCE_DIRECTORY__ + @"./day06_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"./day06_input2.txt"

let inputLines = File.ReadAllLines(filepath) |> List.ofArray
let coords = inputLines |> List.mapi(fun idx v -> [|idx; int(v.Split(", ")[0]); int(v.Split(", ")[1]); 0|])
let size = 400
let panel = createPanel size
//printPanel size panel
let counter = Array.create coords.Length 0
coords |> List.iter(fun  e -> panel.Item($"{e.[1]},{e.[2]}") <- e.[0])
calculateDistances size coords panel
let boundaries = getBoundaries size panel
count coords size panel counter boundaries
//printPanel size panel