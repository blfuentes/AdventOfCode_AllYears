module day07_part02

open System.IO

let path = "day07_input.txt"
//let path = "test_input.txt"

let horizontalpositions = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) 
        |> Seq.map(fun l -> l.Split(',') |> Array.map int |> Array.toList) 
            |> Seq.exactlyOne |> List.sort

let calculateFuel position target =
    let distance = abs(position - target) |> float
    ((1. + distance) / 2.) * distance |> int

let execute =
    [horizontalpositions.Head.. horizontalpositions.Item(horizontalpositions.Length - 1)]
    |> List.map(fun p -> horizontalpositions |> List.sumBy(fun i -> calculateFuel i p)) |> List.min