open System.IO
open System.Collections.Generic
open System
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
let file = "day09_input.txt"
let path = "day09/" + file
let inputLines = GetLinesFromFile(path) |> Array.map (fun x -> Convert.ToUInt64(x)) |> List.ofArray

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

let testList  = [uint64(0)..uint64(20)]
let possibleCombinations (combSize: int) (mainList: uint64 list) =
    seq {
        for init in mainList do
            let index = mainList |> List.findIndex(fun e -> e = init)
            if mainList.Length - combSize > index then
                yield mainList |> List.skip(index) |> List.take(combSize)
    } |> List.ofSeq

possibleCombinations 3 testList

let rec findWeakness (value: uint64) (combSize: int) (listChecker: uint64 list) : (uint64 * uint64) =
    let permu = possibleCombinations combSize listChecker
    let found = (permu |> List.filter(fun l -> l |> List.sum = value))
    match found.Length > 0 with
    | true -> (found.Head |> List.min, found.Head |> List.max)
    | false -> findWeakness value (combSize + 1) listChecker

let result = findInvalidValue inputLines preambleSize
let result2 = findWeakness (snd result) 2 (inputLines)
fst result2 + snd result2