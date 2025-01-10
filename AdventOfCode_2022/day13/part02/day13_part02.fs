module day13_part02

open System
open System.Text.Json.Nodes

open AdventOfCode_2022.Modules.LocalHelper

let path = "day13/day13_input.txt"

type Packet =
    | Integer of int
    | List of Packet array

let rec convert (p : JsonNode) : Packet =
    match p with
    | :? JsonValue as n -> Integer (n.GetValue<int>())
    | :? JsonArray as l -> l |> Seq.map convert |> Array.ofSeq |> List
    | _ -> failwith "Invalid Input"

let wrap = Array.singleton >> List
let createDividerPacket = Integer >> wrap >> wrap

let rec compare (l : Packet) (r : Packet) =
    match l, r with
    | Integer li, Integer ri when li < ri -> -1
    | Integer li, Integer ri when li > ri -> 1
    | Integer _, Integer _ -> 0
    | Integer _, List _ -> compare (wrap l) r
    | List _, Integer _ -> compare l (wrap r)
    | List ll, List rl ->
        (ll, rl)
        ||> Seq.fold2 (fun acc a b ->
            match acc with
            | 0 -> compare a b
            | x -> x
        ) 0
        |> function
        | 0 when ll.Length < rl.Length -> -1
        | 0 when ll.Length > rl.Length -> 1
        | x -> x

let execute =
    let inputLines = ReadLines(path)
    let packets = inputLines |> Seq.filter (fun l -> String.IsNullOrEmpty l = false) 
                    |> Seq.map (JsonNode.Parse >> convert) |> Array.ofSeq
    let dp2 = createDividerPacket 2
    let dp6 = createDividerPacket 6
    let packets' =
        packets
        |> Array.append [| dp2; dp6 |]
        |> Array.sortWith compare
    let dp2Index = packets' |> Array.findIndex((=) dp2) |> (+) 1
    let dp6Index = packets' |> Array.findIndex((=) dp6) |> (+) 1
    dp2Index * dp6Index