module day23_part01

open System.Collections.Generic
open AdventOfCode_2015.Modules

type OpKind =
    | Half
    | Tpl
    | Inc
    | Jmp
    | Jie
    | Jio

type Operation = {
    Kind : OpKind
    Register: string option
    Value: int option
}

let parseContent(lines: string array) =
    let registers = Dictionary<string, int>()
    registers.Add("a", 0)
    registers.Add("b", 0)

    let parts = 
        lines |> Array.map _.Replace(",", "").Split(" ")
    let operations =
        seq {
            for part in parts do
                match part[0] with
                | "hlf" ->
                    yield { Kind = Half; Register = Some(part[1]); Value = None }
                | "tpl" ->
                    yield { Kind = Tpl; Register = Some(part[1]); Value = None }
                | "inc" ->
                    yield { Kind = Inc; Register = Some(part[1]); Value = None }
                | "jmp" ->
                    yield { Kind = Jmp; Register = None; Value = Some((int)part[1]) }
                | "jie" ->
                    yield { Kind = Jie; Register = Some(part[1]); Value = Some((int)part[2]) }
                | "jio" ->
                    yield { Kind = Jio; Register = Some(part[1]); Value = Some((int)part[2]) }
                | _ -> failwith "error"
        } |> List.ofSeq

    (registers, operations)

let performOp (op: Operation) (registers: Dictionary<string, int>) =
    let mutable opIdx = 1
    if op.Kind.IsHalf then
        registers[op.Register.Value] <- registers[op.Register.Value] / 2
    elif op.Kind.IsTpl then
        registers[op.Register.Value] <- registers[op.Register.Value] * 3
    elif op.Kind.IsInc then
        registers[op.Register.Value] <- registers[op.Register.Value] + 1
    elif op.Kind.IsJmp then
        opIdx <- op.Value.Value
    elif op.Kind.IsJie then
        if registers[op.Register.Value] % 2 = 0 then
            opIdx <- op.Value.Value
    elif op.Kind.IsJio then
        if registers[op.Register.Value] = 1 then
            opIdx <- op.Value.Value

    opIdx

let runProgram (operations: Operation list) (registers: Dictionary<string, int>)=
    let mutable currentOpIdx = 0
    while currentOpIdx < operations.Length do
        let newIdx = performOp operations[currentOpIdx] registers
        currentOpIdx <- currentOpIdx + newIdx

    registers["b"]

let execute =
    let input = "./day23/day23_input.txt"
    let content = LocalHelper.GetLinesFromFile input
    let (registers, operations) = parseContent content
    runProgram operations registers