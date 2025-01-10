open AdventOfCode_2017.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day05/test_input_01.txt"
let path = "day05/day05_input.txt"

let input = GetLinesFromFile path |> Array.map int

let rec execute (input: int[]) (index: int) (steps: int) =
    let length = input.Length
    if index < 0 || index >= length then
        steps
    else
        let jump = input.[index]
        let newIndex = index + jump
        input.[index] <- input.[index] + (if jump >= 3 then -1 else 1)
        execute input newIndex (steps + 1)

let result = execute input 0 0
printfn "Number of jumps : %i" result