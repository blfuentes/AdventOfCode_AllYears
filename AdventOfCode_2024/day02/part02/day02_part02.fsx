#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_Utilities

//let path = "day02/test_input_01.txt"
let path = "day02/day02_input.txt"

let getExclusions (arr: 'a array) : 'a array list =
    [for i in 0 .. arr.Length - 1 -> Array.append (arr.[0 .. i-1]) (arr.[i+1 ..])]

let arr = [|1; 2; 3; 4|]
let exclusions = getExclusions arr

exclusions |> List.iter (printfn "%A")
