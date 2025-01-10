module day09_part02

open System

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules

let path = "day09/day09_input.txt"

let inputLines = LocalHelper.GetLinesFromFile(path) |> Array.map (fun x -> Convert.ToUInt64(x)) |> List.ofArray

let preambleSize = 25

let numberIsValid (value: uint64) (listChecker: uint64 list) =
    let permu = combination 2 listChecker
    (permu |> List.exists(fun x -> x.Item(0) + x.Item(1) = value), value)

let rec findInvalidValue (elements: uint64 list) (preamble: int) : (bool * uint64)=
    match elements.Length = 0 with
    | true -> (false, uint64(0))
    | false ->
        let checkList = elements |> List.take(preamble + 1)
        let valid = numberIsValid (checkList |> List.rev |> List.head) (checkList |> List.take(preamble))
        match fst valid with 
        | false -> (true, snd valid)
        |  true -> findInvalidValue (elements |> List.skip(1)) preamble


let rec findWeakness (value: uint64) (combSize: int) (listChecker: uint64 list) : (uint64 * uint64) =
    let permu = possibleCombinations combSize listChecker
    let found = (permu |> List.filter(fun l -> l |> List.sum = value))
    match found.Length > 0 with
    | true -> (found.Head |> List.min, found.Head |> List.max)
    | false -> findWeakness value (combSize + 1) listChecker

let execute =
    let result = findInvalidValue inputLines preambleSize
    let result2 = findWeakness (snd result) 2 (inputLines)
    fst result2 + snd result2