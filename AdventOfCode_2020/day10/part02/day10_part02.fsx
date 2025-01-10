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
let file = "day10_input.txt"
let path = "day10/" + file
let inputLines = GetLinesFromFile(path) |> Array.map int |> Array.sort |> List.ofArray

let diff(a, b) =
    b - a

let maxValue = inputLines.[inputLines.Length - 1] + 3
let compList = [0] @ inputLines @ [maxValue] |> List.pairwise |> List.map (diff) |> List.map string |> String.concat ""
let groups = compList.Split('3')
let groupsOf2 = groups |> Array.filter(fun c -> c.Length = 2) |> Array.length |> float
let groupsOf3 = groups |> Array.filter(fun c -> c.Length = 3) |> Array.length |> float
let groupsOf4 = groups |> Array.filter(fun c -> c.Length = 4) |> Array.length |> float

(2.0**groupsOf2 * 4.0**groupsOf3 * 7.0**groupsOf4) |> int64