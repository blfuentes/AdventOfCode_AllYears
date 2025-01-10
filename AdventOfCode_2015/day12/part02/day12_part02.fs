module day12_part02

open FSharp.Data

open AdventOfCode_2015.Modules

let hasRed (record : (string * JsonValue)[]) =
    Array.exists (
        fun (key, value) -> 
            match value with
            | JsonValue.String k when k = "red" -> true
            | _ -> false 
        ) record

let rec sum filter (jsonToken : JsonValue) =
    match jsonToken with
    | JsonValue.Number number -> int number
    | JsonValue.Record record when not (filter record) -> 
        record 
        |> Array.sumBy (fun (key, value) -> sum filter value)
    | JsonValue.Array collection -> 
        collection 
        |> Array.sumBy (fun elem -> sum filter elem)
    | _ -> 0

let execute =
    let path = "day12/day12_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let inputobject = JsonValue.Parse content
    sum hasRed inputobject