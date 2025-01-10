module day13_part02

open System

open AdventOfCode_2020.Modules

let path = "day13/day13_input.txt"

let inputLines = LocalHelper.GetLinesFromFile(path) 

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

let execute =
    calculateTime buses.Tail min min