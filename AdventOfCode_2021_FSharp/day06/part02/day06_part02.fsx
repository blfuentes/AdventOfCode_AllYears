open System
open System.IO
open System.Text.RegularExpressions

let path = "day06_input.txt"
//let path = "test_input.txt"

let nextLanternfish = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun l -> l.Split(',') |> Array.map int |> Array.toList) |> Seq.exactlyOne

let rec computeDay (fishes: int list, currentday: int, finishday: int) =
    match currentday = finishday with
    | true -> fishes.Length
    | false ->
        let newNumberOfFishes = fishes |> List.filter(fun f -> f = 0) |> List.length
        let newfishes = fishes |> List.map (fun f -> if (f - 1) = -1 then 6 else (f - 1))
        let babyfishes = [for i in 0..(newNumberOfFishes - 1) -> 8]
        computeDay (newfishes @ babyfishes, currentday + 1, finishday)

let numberOfFishes (initialfishes:int list) days =
    let fishesByDay = [0..8] |> List.map(fun d -> initialfishes |> List.filter(fun f -> f = d) |> List.length |> bigint) |> List.toArray 
    for i in [0 .. days - 1] do
        fishesByDay.[(i + 7) % 9] <- fishesByDay.[(i + 7) % 9] + fishesByDay.[i % 9]
    Array.sum(fishesByDay)

numberOfFishes (nextLanternfish) 256