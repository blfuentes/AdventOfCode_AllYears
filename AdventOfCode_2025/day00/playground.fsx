open System
open System.IO

let input = File.ReadAllText(__SOURCE_DIRECTORY__ + "/day00_input.txt")
let input2= File.ReadAllLines(__SOURCE_DIRECTORY__ + "/day00_input.txt") |> Array.toList


let double x = x * 2
let square x = x * x
let increment x = x + 1

let doublesquareincrement = double >> square >> increment

let result = 3 |> double |> square |> increment
let result2 = doublesquareincrement 3


open System.Diagnostics

let double x = x * 2
let square x = x * x
let increment x = x + 1

let doublesquareincrement = double >> square >> increment

let measureTime f =
    let stopwatch = Stopwatch.StartNew()
    let result = f
    stopwatch.Stop()
    (result, stopwatch.ElapsedTicks)

let result1, time1 = measureTime (fun x -> x |> double |> square |> increment) 3
let result2, time2 = measureTime (doublesquareincrement 3)

printfn "Pipe Result: %d, Time: %d ticks" result1 time1
printfn "Compose Result: %d, Time: %d ticks" result2 time2
