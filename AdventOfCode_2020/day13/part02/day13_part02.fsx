open System.Collections.Generic
open AdventOfCode_2020.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules.Helpers
open AdventOfCode_Utilities

//let file = "test_input.txt"
//let file = "test_input_00.txt"
//let file = "test_input_01.txt"
//let file = "test_input_02.txt"
//let file = "test_input_03.txt"
//let file = "test_input_04.txt"
let file = "day13_input.txt"
let path = "day13/" + file
let inputLines = GetLinesFromFile(path)

let buses = 
    let inputSplitted = inputLines.[1].Split(',')
    seq {
        for bus in inputSplitted do
            if bus <> "x" then
                yield [|inputSplitted |> Array.findIndex((=)bus) |> bigint; bus |> int |> bigint|]
    } |> List.ofSeq

let min = buses.Head |> Array.sum

let rec getTime (time: bigint) (offset: bigint) (bus: bigint) (period: bigint) =
    match ((time + offset) % bus = 0I) with
    | true -> time
    | false -> getTime (time + period) offset bus period

let rec calculateTime (busList: bigint[] list) (currentTime: bigint) (currentPeriod: bigint) =
    match busList.IsEmpty with
    | true -> currentTime
    | false ->
        let newTime = getTime currentTime busList.Head.[0] busList.Head.[1] currentPeriod
        calculateTime busList.Tail newTime (currentPeriod * busList.Head.[1])

let result = calculateTime buses.Tail min min

//foreach (var (schedule, offset) in buses.Skip(1))
//{
//  while ((time + offset) % schedule != 0) time += period;

//  period *= schedule;
//}
//let max = buses.Tail |> List.map(fun x -> x |> Array.sum) |> List.max

//let rec findElement (increment:bigint) (step:bigint) (busList: bigint[] list) =
//    match busList |> List.forall(fun b -> ((step + b.[0]) % b.[1]) = 0I) with
//    | true -> step
//    | false -> findElement increment (increment + step) busList

//findElement min 100000000000000I buses

//for step in [0..min..1000000] do
//    if buses |> List.forall(fun b -> ((step + b.[0]) % b.[1]) = 0) then
//        printf "Found at step %i" step

