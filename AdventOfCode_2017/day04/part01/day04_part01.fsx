open AdventOfCode_2017.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day04/test_input_01.txt"
let path = "day04/day04_input.txt"

let input = GetLinesFromFile path
let groups = input |> Array.map (fun l -> l.Split(' ') |> Array.countBy id)
            |> Array.filter(fun g -> g |> Array.forall(fun (k, c) -> c = 1)) |> Array.length