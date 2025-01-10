module day08_part02

open System

open AdventOfCode_Utilities
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

let checkOpIdx = operations |> Array.filter(fun o -> o.Op = HandheldOpType.JMP ||o.Op = HandheldOpType.NOP) |> Array.map(fun o -> Array.IndexOf(operations, o))

let execute =
    let rec loop (checkOpIdxList: int list) (program: HandledOperation[]) = 
        let op = program.[checkOpIdxList.Head]
        let newOp =
            match op.Op with
            | HandheldOpType.JMP -> { Op = HandheldOpType.NOP; Offset = op.Offset }
            | HandheldOpType.NOP -> { Op = HandheldOpType.JMP; Offset = op.Offset }
            | _ -> { Op = HandheldOpType.MISSING; Offset = op.Offset }
        let currentProgram = (updateElement checkOpIdxList.Head newOp (program |> List.ofArray)) |> Array.ofList
        let sub = calculateAccumulatorComplex 0 [] 0 currentProgram
        if not (fst sub) && checkOpIdxList.Length > 0 then 
            loop checkOpIdxList.Tail program
        else
            sub
    snd (loop (checkOpIdx |> List.ofArray) operations)