open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day06/test_input_01.txt"
// let path = "day06/test_input_02.txt"
// let path = "day06/test_input_03.txt"
// let path = "day06/test_input_04.txt"
// let path = "day06/test_input_05.txt"
let path = "day06/day06_input.txt"

let content = GetContentFromFile(path)

let rec findIdxOfUnique (size: int) (pos: int) (prev: string) (input: string) = 
    match pos < size with
    | true -> findIdxOfUnique size (pos + 1) (prev + input.Substring(pos, 1)) input
    | false ->
        let lastfour = prev.ToCharArray() |> Array.rev |> Array.take size        
        if lastfour |> Array.countBy(fun c -> c) |> Array.forall(fun (e, c) -> c = 1) then pos 
        else findIdxOfUnique size (pos + 1) (prev + input.Substring(pos, 1)) input

let startOfPacket = findIdxOfUnique 4 0 "" content