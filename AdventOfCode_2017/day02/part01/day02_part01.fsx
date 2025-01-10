open AdventOfCode_2017.Modules

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day02/test_input_01.txt"
let path = "day02/day02_input.txt"

let processLine (line: string) =
    let values = 
        seq {
            for c in line.ToCharArray() do
                if ['0'..'9'] |> List.contains c then
                    yield c
                else
                    yield! " "
        } |> List.ofSeq
    System.String.Concat(values).Split(' ')

// processLine "798	1976	1866	1862	559	1797	1129	747	85	1108	104	2000	248	131	87	95"

let calculateCheckSumRow (line: string) =
    // printfn "Length of line: %i" line.Length
    let parts = processLine line |> Array.map  int |> Array.sortDescending  
    parts.[0] - parts.[parts.Length - 1]
    

let inputLines = LocalHelper.GetLinesFromFile(path)
inputLines |> Array.sumBy calculateCheckSumRow