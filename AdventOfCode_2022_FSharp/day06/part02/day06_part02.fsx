open AdventOfCode_2022_FSharp.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities_FSharp/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities_FSharp

// let path = "day06/test_input_01.txt"
// let path = "day06/test_input_02.txt"
// let path = "day06/test_input_03.txt"
// let path = "day06/test_input_04.txt"
// let path = "day06/test_input_05.txt"
let path = "day06/day06_input.txt"

let content = GetContentFromFile(path)

let rec findIdxFourUnique (pos: int) (prev: string) (input: string) = 
    match pos < 14 with
    | true -> findIdxFourUnique (pos + 1) (prev + input.Substring(pos, 1)) input
    | false ->
        let lastfour = prev.ToCharArray() |> Array.rev |> Array.take 14        
        if lastfour |> Array.countBy(fun c -> c) |> Array.forall(fun (e, c) -> c = 1) then pos else findIdxFourUnique (pos + 1) (prev + input.Substring(pos, 1)) input

let startOfPacket = findIdxFourUnique 0 "" content