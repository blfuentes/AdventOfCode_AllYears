open System
open System.Collections.Generic

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
open AdventOfCode_Utilities

// let path = "day25/test_input_01.txt"
let path = "day25/day25_input.txt"

let convertToDecimal (snuffvalue: string) =
    let svaluesIdx = snuffvalue.ToCharArray() |> Array.rev |> Array.mapi(fun idx v -> (idx, (string)v))
    let parts =
        seq {
            for (idx, v) in svaluesIdx do
                let p = 
                    match v with
                    | "1" -> 1. * Math.Pow(5, idx)
                    | "2" -> 2. * Math.Pow(5, idx)
                    | "-" -> -1. * Math.Pow(5, idx)
                    | "="-> -2. * Math.Pow(5, idx)
                    | _ -> 0.
                yield p
        }
    bigint(parts |> Seq.sum)

let rec convertToSnuff (current: string) (decvalue: bigint) =
    if decvalue = 0I then String.Concat(current.ToCharArray() |> Array.rev)
    else
        let check = (decvalue + 2I) % 5I - 2I
        let newdigit =
            if check = 0I then "0"
            elif check = 1I then "1"
            elif check = 2I then "2"
            elif check = -2I then "="
            elif check = -1I then "-"
            else ""
        convertToSnuff (current + newdigit) ((decvalue + 2I) / 5I)

let inputLines = GetLinesFromFile(path)
let totalsumdec = bigint.Parse((inputLines |> Array.map convertToDecimal |> Array.sum).ToString())
let totalsumsnuff = convertToSnuff "" totalsumdec
// printfn "Result: %s" totalsumsnuff