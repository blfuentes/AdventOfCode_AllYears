open System.IO

let path = "day01_input.txt"
//let path = "test_input.txt"
let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map int |> Seq.toList

inputLines |> List.pairwise |> List.filter (fun (x,y) -> y > x) |> List.length