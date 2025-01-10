module day21_part02

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

let solvePart2 seed =
    let rec findLastUnique prev seen =
        let next = getNextTerminatingValue seed prev
        if Set.contains next seen then
            prev
        else
            findLastUnique next (Set.add next seen)
    findLastUnique 0 Set.empty

let execute =
    let path = "day21/day21_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    solvePart2 (getSeed content)