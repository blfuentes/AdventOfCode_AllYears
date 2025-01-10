module day11_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

let InsertOrUpdate(collection: Dictionary<int64, int64>) (key: int64) (value: int64) =
    collection[key] <- 
        if collection.ContainsKey(key) then
            collection[key] + value
        else
            value

let parseContent (lines: string) =
    let data = Dictionary<int64, int64>()
    lines.Split(" ")
    |> Array.iter (fun s ->
        let key = int64 s
        InsertOrUpdate data key 1L
    )
    data

let runMutations (stonesdata: Dictionary<int64, int64>) (step: int) =
    let mutable currentStoneData = stonesdata
    [1..step]
    |> List.iter(fun _ ->
        let newStoneData = Dictionary<int64, int64>()
        for kvp in currentStoneData do
            let stonekey = kvp.Key
            let stoneData = kvp.Value
            let length = stonekey.ToString().Length
        
            match stonekey with
            | sk when sk = 0L ->
                InsertOrUpdate newStoneData 1L stoneData
            | sk when length % 2 = 0 ->
                let mid = length / 2
                let left, right = (
                    System.Int64.Parse(sk.ToString()[0..mid-1])
                    , System.Int64.Parse(sk.ToString()[mid..]))
                InsertOrUpdate newStoneData left stoneData
                InsertOrUpdate newStoneData right stoneData
            | _ ->
                let newStoneKey = stonekey * 2024L
                InsertOrUpdate newStoneData newStoneKey stoneData
        currentStoneData <- newStoneData
    )
        
    currentStoneData

let execute() =
    let path = "day11/day11_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let stones = parseContent content
    let stoneQuantityData = runMutations stones 75

    stoneQuantityData.Values |> Seq.sum