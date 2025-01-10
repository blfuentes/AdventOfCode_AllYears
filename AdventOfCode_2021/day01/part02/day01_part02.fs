module day01_part02

open System.IO
open AdventOfCode_Utilities

let path = "day01_input.txt"
let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map int |> Seq.toList

let execute =
    let groupedByThree = (Utilities.getSubListBySize 3 inputLines) |> List.map (List.sum)
    groupedByThree |> List.pairwise |> List.filter (fun (x,y) -> y > x) |> List.length