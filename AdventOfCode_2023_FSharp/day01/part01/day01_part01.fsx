#load @"../../../AdventOfCode_Utilities_FSharp/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2023_FSharp.Modules
open AdventOfCode_Utilities_FSharp

//let path = "day01/test_input_01.txt"
let path = "day01/day01_input.txt"

let lines = LocalHelper.GetLinesFromFile path

let findNumbers input =
    let matches = Regex.Matches(input, @"\d")
    matches |> Seq.cast<Match> |> Seq.map(fun m -> m.Value) |> Seq.toList

let numbers = lines |> Array.map findNumbers


let getNumbersFromLine(line: string) =
    let numbers = findNumbers line
    if numbers.Length > 0 then (int)(numbers.Head + numbers.Item(numbers.Length - 1))
    else 0

//let values = lines |> Array.map getNumbersFromLine
//values
let totalSum = Array.sumBy getNumbersFromLine lines
totalSum
    

