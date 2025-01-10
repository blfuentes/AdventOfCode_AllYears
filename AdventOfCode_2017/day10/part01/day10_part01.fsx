#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic

open AdventOfCode_2017.Modules
open AdventOfCode_Utilities

let rec processLengths (state: int array) (lengths: int list) (currentskip: int) (currentindex: int) =
    match lengths with
    | [] -> state
    | length :: rest ->
        let totake = length + currentskip
        let (takeleft, takeright) = 
            if totake > (state.Length - currentindex) then 
                (totake - (totake - (state.Length - currentindex)), totake - (state.Length - currentindex))
            else
                (0, totake)
        let (lefttaken, righttaken) = (state |> Array.take(takeleft), state[currentindex..(currentindex + takeright - 1)])
        let mid = state[..currentindex] |> Array.skip(takeleft)
        let newstate = Array.concat([lefttaken; mid; righttaken])
        processLengths newstate rest (currentskip + 1) ((currentindex + length)% length)

let path = "day10/test_input_01.txt"
//let path = "day10/day10_input.txt"
let content = (LocalHelper.GetContentFromFile path).Split(", ") |> Array.map int |> List.ofArray
let size = 5
let circular = (Array.create size 0) |> Array.mapi(fun idx value -> idx)
processLengths circular content 0 0
