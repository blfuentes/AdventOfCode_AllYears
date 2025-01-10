module day13_part01


open AdventOfCode_2020.Modules

let path = "day13/day13_input.txt"

let inputLines = LocalHelper.GetLinesFromFile(path) 

let initTime = inputLines.[0] |> int
let buses = inputLines.[1].Split(',') |> Array.filter(fun b -> b <> "x") |> Array.map int
let times = buses |> Array.map(fun b -> ((initTime / b) * b) + b)
let minTime = times |> Array.min
let bus = buses.[times |> Array.findIndex(fun t -> t = minTime)]

let execute =
    bus * (minTime - initTime)