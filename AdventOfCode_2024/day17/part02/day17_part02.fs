module day17_part02
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
    Value: int64
}

let opType(v: int64) =
    match v with
    | 0L -> Adv
    | 1L -> Bxl
    | 2L -> Bst
    | 3L -> Jnz
    | 4L -> Bxc
    | 5L -> Out
    | 6L -> Bdv
    | 7L -> Cdv
    | _ -> failwith "error"

let revOpType(v: OpCode) =
    match v with
    | Adv -> 0L
    | Bxl -> 1L
    | Bst -> 2L
    | Jnz -> 3L
    | Bxc -> 4L
    | Out -> 5L
    | Bdv -> 6L
    | Cdv -> 7L

let parseContent(lines: string) =
    let partregisters = lines.Split("\r\n\r\n")[0]
    let partprogram = lines.Split("\r\n\r\n")[1]
    let registers = Dictionary<string,int64>()
    let r' =
        partregisters.Split("\r\n")
        |> Array.iter(fun r ->
            let (n, v) = ((r.Split(" ")[1]).Replace(":",""),
                (int64)(r.Split(" ")[2]))
            registers.Add(n, v)
        )
    let ops = 
        (partprogram.Split(" ")[1]).Split(",")
        |> Array.map int64 |> Array.map opType
    (registers, ops)

let comboOperand(op: OpCode) (registers: Dictionary<string,int64>) =
    match op with
    | Adv -> 0L
    | Bxl -> 1L
    | Bst -> 2L
    | Jnz -> 3L
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
    
let performOp(op: OpCode) (opOp: OpCode) (registers: Dictionary<string,int64>) (pIdx: int)=
    let mutable pointerIdx = pIdx + 2
    let mutable output = -1L
    if op.IsAdv then
        let numerator = (float)(registers["A"])
        let denominator = System.Math.Pow(2, (float)(comboOperand opOp registers))
        registers["A"] <- (int64)(numerator / denominator)
    elif op.IsBxl then
        registers["B"] <- registers["B"] ^^^ (literalOperand opOp)
    elif op.IsBst then
        let numerator = comboOperand opOp registers
        registers["B"] <- numerator % 8L
    elif op.IsJnz then
        if registers["A"] <> 0 then
            pointerIdx <- literalOperand opOp
    elif op.IsBxc then
        registers["B"] <- registers["B"] ^^^ registers["C"]
    elif op.IsOut then  
        output <- (comboOperand opOp registers) % 8L
    elif op.IsBdv then
        let numerator = (float)(registers["A"])
        let denominator = System.Math.Pow(2, (float)(comboOperand opOp registers))
        registers["B"] <- (int64)(numerator / denominator)
    elif op.IsCdv then
        let numerator = (float)(registers["A"])
        let denominator = System.Math.Pow(2, (float)(comboOperand opOp registers))
        registers["C"] <- (int64)(numerator / denominator)

    (pointerIdx, output)

let runProgram(ops: OpCode array)(registers: Dictionary<string,int64>) =
    let mutable pIdx = 0
    let mutable outputValues = []
    while pIdx < ops.Length do
        let (newPidx, output) = performOp ops[pIdx] ops[pIdx+1] registers pIdx
        pIdx <- newPidx
        if output <> -1 then
            outputValues <- outputValues @ [output]
    outputValues

let findNewRegister(ops: OpCode array) (registers: Dictionary<string,int64>) =
    let reversedOps = ops |> Array.rev
    let reversedOpsIds = reversedOps |> Array.map revOpType
    let checkStack = new Stack<int64*int>()
    checkStack.Push((0L,0))
    let mutable currentsolution = System.Int64.MaxValue
    while checkStack.Count > 0 do
        let (pIdx, mindex) = checkStack.Pop()
        for bitIdx in 0L..8L do
            let newRegisters = Dictionary(registers)
            newRegisters["A"] <- bitIdx + pIdx
            let output = runProgram ops newRegisters
            if output[0] = reversedOpsIds[mindex] then
                if mindex+1 >= ops.Length then
                    if bitIdx + pIdx < currentsolution then
                        currentsolution <- bitIdx + pIdx
                else
                    checkStack.Push((8L*(pIdx+bitIdx), mindex+1))
    currentsolution

// alternative solution with decomposition of the problem
let findA (output: int64 list) =
    let rec generateA (output: int64 list) =
        let mutable registerB = 0L
        seq {
            if output.Length = 0 then
                yield 0L
            else
                for currentOp in generateA output.Tail do // iterate in reverse order
                    for bitPos in 0L..7L do
                        let registerA = currentOp * 8L + bitPos
                        registerB <- registerA % 8L
                        registerB <- registerB ^^^ 3L
                        let registerC = registerA >>> int registerB
                        registerB <- registerB ^^^ registerC
                        registerB <- registerB ^^^ 5L
                        if output[0] = registerB % 8L then
                            yield registerA
        }
    
    generateA output |> Seq.min

let execute() =
    let path = "day17/day17_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let (registers, ops) = parseContent content
    let registerA = findNewRegister ops registers
    registerA
