module day06_part02

open System.IO

let path = "day06_input.txt"
//let path = "test_input.txt"

let nextLanternfish = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun l -> l.Split(',') |> Array.map int |> Array.toList) |> Seq.exactlyOne
let numberOfDays = 256

let numberOfFishes (initialfishes:int list) days =
    let fishesByDay = [0..8] |> List.map(fun d -> initialfishes |> List.filter(fun f -> f = d) |> List.length |> bigint) |> List.toArray 
    for i in [0 .. days - 1] do
        fishesByDay.[(i + 7) % 9] <- fishesByDay.[(i + 7) % 9] + fishesByDay.[i % 9]
    Array.sum(fishesByDay)

let execute =
    numberOfFishes (nextLanternfish) numberOfDays