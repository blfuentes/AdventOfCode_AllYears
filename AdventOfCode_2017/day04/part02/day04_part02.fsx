open AdventOfCode_2017.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day04/test_input_02.txt"
let path = "day04/day04_input.txt"

let input = GetLinesFromFile path

let isAnagram (s1:string) (s2:string) =
    let s1 = s1 |> Seq.sort |> Seq.toArray
    let s2 = s2 |> Seq.sort |> Seq.toArray
    s1 = s2

let rec checkAnagram (values: string list) =
    match values with
    | [] -> false
    | head::tail ->
        let foundAnagram = tail |> List.exists (fun v -> isAnagram head v)
        match foundAnagram with
        | true -> true
        | false -> checkAnagram tail

input |> Array.map (fun l -> l.Split(' ') |> Array.toList) |> Array.filter(fun g -> checkAnagram g |> not) |> Array.length