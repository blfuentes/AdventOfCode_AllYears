open System
open System.IO
open System.Text.RegularExpressions

let path = "day08_input.txt"
//let path = "test_input.txt"
//let path = "test_input_00.txt"

let inputPartsCollection = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) 
        |> Seq.map(fun l -> l.Trim().Split('|') |> Array.map(fun s -> s.Split(' ') |> Array.filter(fun p -> p <> ""))) 

let result = inputPartsCollection |> Seq.map(fun l -> l.[1] |> Array.filter(fun l -> l.Length = 2 || l.Length = 3 || l.Length = 4 || l.Length = 7) |> Array.length) |> Seq.sum