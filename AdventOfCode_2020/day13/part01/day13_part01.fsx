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
let file = "day13_input.txt"
let path = "day13/" + file
let inputLines = GetLinesFromFile(path)

let initTime = inputLines.[0] |> int
let buses = inputLines.[1].Split(',') |> Array.filter(fun b -> b <> "x") |> Array.map int
let times = buses |> Array.map(fun b -> ((initTime / b) * b) + b)
let minTime = times |> Array.min
let bus = buses.[times |> Array.findIndex(fun t -> t = minTime)]
let result = bus * (minTime - initTime)