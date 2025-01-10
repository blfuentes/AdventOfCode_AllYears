module day08_part01

open CustomDataTypes
open AdventOfCode_2020.Modules

let path = "day08/day08_input.txt"

let inputLines = LocalHelper.GetLinesFromFile(path)

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

let execute =
    calculateAccumulator 0 [] 0 operations