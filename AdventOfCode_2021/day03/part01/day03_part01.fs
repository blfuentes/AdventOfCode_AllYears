module day03_part01

open System
open System.IO

let path = "day03_input.txt"
//let path = "test_input.txt"
let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map (fun x -> x.ToCharArray() |> Array.map string) |> Seq.toList

let calculateGammaRate(input: list<string[]>) =
    let maxIdx = input.Head.Length - 1
    let tmp = seq {
        for i in 0 .. maxIdx do
            let numberOfOnes = input |> List.map(fun l -> l.[i]) |> List.filter (fun e -> e = "1") |> List.length
            match numberOfOnes >= (input.Length / 2) with
            | true -> yield "1"
            | false -> yield "0"
    }
    tmp |> Seq.toList

let execute =
    let gammarate = calculateGammaRate(inputLines)
    let gammarateValue = Convert.ToInt32(gammarate |> List.fold (+) "", 2)
    let epsilonrate = gammarate |> List.map(fun x -> if x = "1" then "0" else "1")
    let epsilonrateValue = Convert.ToInt32(epsilonrate |> List.fold (+) "", 2)

    gammarateValue * epsilonrateValue