open System
open System.IO
open System.Text.RegularExpressions

let path = "day06_input.txt"
//let path = "test_input.txt"

let nextLanternfish = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun l -> l.Split(',') |> Array.map int |> Array.toList) |> Seq.exactlyOne

let lanternfishrate = 7
let numberOfDays = 80

let rec computeDay (fishes: int list, currentday: int, finishday: int) =
    match currentday = finishday with
    | true -> fishes.Length
    | false ->
        let newNumberOfFishes = fishes |> List.filter(fun f -> f = 0) |> List.length
        let newfishes = fishes |> List.map (fun f -> if (f - 1) = -1 then 6 else (f - 1))
        let babyfishes = [for i in 0..(newNumberOfFishes - 1) -> 8]
        computeDay (newfishes @ babyfishes, currentday + 1, finishday)

computeDay(nextLanternfish, 0, numberOfDays)