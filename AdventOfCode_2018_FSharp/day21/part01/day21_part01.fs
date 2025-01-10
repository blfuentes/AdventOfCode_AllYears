module day21_part01

open AdventOfCode_Utilities
open AdventOfCode_2018.Modules

let getSeed =
    // after comparing many inputs, the only lines that was different was line 8 
    Seq.item 8 >> splitByFn " " (fun p -> int p.[1])

let getNextTerminatingValue seed prev =
    let applyByte value =
        (+) value
        >> (&&&) 0xFFFFFF
        >> (*) 65899
        >> (&&&) 0xFFFFFF
    
    let b = prev ||| 0x10000
    [|0; 8; 16|]
    |> Array.map (fun shift -> b >>> shift &&& 0xFF)
    |> Array.fold applyByte seed

let longestNonHalting seed = 
    getNextTerminatingValue seed 0

let execute =
    let path = "day21/day21_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    longestNonHalting (getSeed content)