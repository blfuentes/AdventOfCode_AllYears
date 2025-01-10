module day17_part01

open AdventOfCode_2024.Modules
open System.Collections.Generic

type OpCode =
    | Adv
    | Bxl
    | Bst
    | Jnz
    | Bxc
    | Out
    | Bdv
    | Cdv

type Register = {
    Name: string
    Value: int
}

let opType(v: int) =
    match v with
    | 0 -> Adv
    | 1 -> Bxl
    | 2 -> Bst
    | 3 -> Jnz
    | 4 -> Bxc
    | 5 -> Out
    | 6 -> Bdv
    | 7 -> Cdv
    | _ -> failwith "error"

let parseContent(lines: string) =
    let partregisters = lines.Split("\r\n\r\n")[0]
    let partprogram = lines.Split("\r\n\r\n")[1]
    let registers = Dictionary<string,int>()
    let r' =
        partregisters.Split("\r\n")
        |> Array.iter(fun r ->
            let (n, v) = ((r.Split(" ")[1]).Replace(":",""),
                (int)(r.Split(" ")[2]))
            registers.Add(n, v)
        )
    let ops = 
        (partprogram.Split(" ")[1]).Split(",")
        |> Array.map int |> Array.map opType
    (registers, ops)

let comboOperand(op: OpCode) (registers: Dictionary<string,int>) =
    match op with
    | Adv -> 0
    | Bxl -> 1
    | Bst -> 2
    | Jnz -> 3
    | Bxc -> registers["A"]
    | Out -> registers["B"]
    | Bdv -> registers["C"]
    | Cdv -> failwith "error"

let literalOperand(op: OpCode) =
    match op with
    | Adv -> 0
    | Bxl -> 1
    | Bst -> 2
    | Jnz -> 3
    | Bxc -> 4
    | Out -> 5
    | Bdv -> 6
    | Cdv -> 7
    
let performOp(op: OpCode) (opOp: OpCode) (registers: Dictionary<string,int>) (pIdx: int)=
    let mutable pointerIdx = pIdx + 2
    let mutable output = -1
    if op.IsAdv then
        let numerator = (float)(registers["A"])
        let denominator = System.Math.Pow(2, (comboOperand opOp registers))
        registers["A"] <- (int)(numerator / denominator)
    elif op.IsBxl then
        registers["B"] <- registers["B"] ^^^ (literalOperand opOp)
    elif op.IsBst then
        let numerator = comboOperand opOp registers
        registers["B"] <- numerator % 8
    elif op.IsJnz then
        if registers["A"] <> 0 then
            pointerIdx <- literalOperand opOp
    elif op.IsBxc then
        registers["B"] <- registers["B"] ^^^ registers["C"]
    elif op.IsOut then  
        output <- (comboOperand opOp registers) % 8
    elif op.IsBdv then
        let numerator = (float)(registers["A"])
        let denominator = System.Math.Pow(2, (comboOperand opOp registers))
        registers["B"] <- (int)(numerator / denominator)
    elif op.IsCdv then
        let numerator = (float)(registers["A"])
        let denominator = System.Math.Pow(2, (comboOperand opOp registers))
        registers["C"] <- (int)(numerator / denominator)

    (pointerIdx, output)

let runProgram(ops: OpCode array)(registers: Dictionary<string,int>) =
    let mutable pIdx = 0
    let mutable outputValues = []
    while pIdx < ops.Length do
        let (newPidx, output) = performOp ops[pIdx] ops[pIdx+1] registers pIdx
        pIdx <- newPidx
        if output <> -1 then
            outputValues <- outputValues @ [output]
    outputValues |> List.map string

let execute() =
    let path = "day17/day17_input.txt"

    let content = LocalHelper.GetContentFromFile path
    let (registers, ops) = parseContent content
    let result = runProgram ops registers
    String.concat "," result