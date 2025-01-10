open System.IO
open System.Collections.Generic
open System
open CustomDataTypes
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
let file = "day08_input.txt"
let path = "day08/" + file
let inputLines = GetLinesFromFile(path)

let operations = 
    seq {
        for line in inputLines do
            let parts = line.Split(' ')
            let operation =
                match parts.[0] with
                | "acc" -> HandheldOpType.ACC
                | "jmp" -> HandheldOpType.JMP
                | "nop" -> HandheldOpType.NOP
                | _ -> HandheldOpType.MISSING
            yield {
                Op = operation;
                Offset = parts.[1] |> int
            }
    } |> Array.ofSeq

let rec calculateAccumulator (currentValue: int) (consumedOps: int list) (newOpIdx: int) (program: HandledOperation[]) =
    if consumedOps |> List.contains(newOpIdx) then
        currentValue
    else
        let newOp = program.[newOpIdx]
        match newOp.Op with
        | HandheldOpType.ACC -> calculateAccumulator (currentValue + newOp.Offset) (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
        | HandheldOpType.JMP -> calculateAccumulator currentValue (consumedOps @ [newOpIdx]) (newOpIdx + newOp.Offset) program
        | HandheldOpType.NOP -> calculateAccumulator currentValue (consumedOps @ [newOpIdx]) (newOpIdx + 1) program
        | _ -> currentValue

let result = calculateAccumulator 0 [] 0 operations