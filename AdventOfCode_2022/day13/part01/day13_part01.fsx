open System.Text.Json.Nodes
open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day13/test_input_01.txt"
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

let inputLines = ReadLines(path)
let packets = inputLines |> Seq.filter (fun l -> String.IsNullOrEmpty l = false) 
                |> Seq.map (JsonNode.Parse >> convert) |> Array.ofSeq
let result = [0.. 2 .. (packets.Length - 1)]
                |> Seq.map (fun n ->
                match compare packets[n] packets[n+1] with
                | -1 -> (n/2) + 1
                | _ -> 0)
                |> Seq.sum