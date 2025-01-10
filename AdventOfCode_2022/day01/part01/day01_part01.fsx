open System.IO
open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
open AdventOfCode_Utilities
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day01/test_input_01.txt"
let path = "day01/day01_input.txt"

let inputLines = GetLinesFromFile(path) |> Seq.toList
let elvesCalories = getLinesGroupBySeparatorWithEmptySpaceFixReplace inputLines "" |> List.map(fun l -> l |> (List.map int) |> List.sum)
elvesCalories |> List.max