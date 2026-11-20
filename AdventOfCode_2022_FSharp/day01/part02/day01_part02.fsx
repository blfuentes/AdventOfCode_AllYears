open System.IO
open AdventOfCode_2022_FSharp.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities_FSharp/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities_FSharp

let path = "day01/test_input_01.txt"
// let path = "day01/day01_input.txt"

let inputLines = GetLinesFromFile(path) |> Seq.toList
let elvesCalories = getLinesGroupBySeparatorWithEmptySpaceFixReplace inputLines "" |> List.map(fun l -> l |> (List.map int) |> List.sum)
elvesCalories |> List.sortDescending |> List.take 3 |> List.sum