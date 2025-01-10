module day18_part02

open AdventOfCode_2017.Modules
open AdventOfCode_Utilities

open System.Collections.Generic

type InstructionKind =
    | Snd of register: string
    | SndByVal of value: int64
    | SetByReg of target: string * from: string
    | SetByVal of target:string * value: int64
    | AddByReg of target: string * from: string
    | AddByVal of target:string * value: int64
    | MulByReg of target: string * from: string
    | MulByVal of target:string * value: int64
    | ModByReg of target: string * from: string
    | ModByVal of target:string * value: int64
    | Rcv of register: string
    | JgzByReg of checker: string * steps: string
    | JgzByRegWithVal of checker: string * steps: int64
    | JgzByVal of checker:int64 * steps: int64
    | JgzByValWithReg of checker:int64 * steps: string

type TheProgram = {
    Registers: Dictionary<string, int64>
    Pointer: int64
    Frequencies: Queue<int64>
}

let parseContent(lines: string array) =
    lines
    |> Array.map (fun line ->
        let parts = line.Split(' ')
        let instruction = parts[0]
        match instruction with
        | "snd" -> 
            if parts[1] |> Utilities.isNumeric then
                SndByVal(int64 parts[1])
            else
                Snd(parts[1])
        | "set" -> 
            if parts[2] |> Utilities.isNumeric then
                SetByVal(parts[1], int64 parts[2])
            else
                SetByReg(parts[1], parts[2])
        | "add" -> 
            if parts[2] |> Utilities.isNumeric then
                AddByVal(parts[1], int64 parts[2])
            else
                AddByReg(parts[1], parts[2])
        | "mul" -> 
            if parts[2] |> Utilities.isNumeric then
                MulByVal(parts[1], int64 parts[2])
            else
                MulByReg(parts[1], parts[2])
        | "mod" -> 
            if parts[2] |> Utilities.isNumeric then
                ModByVal(parts[1], int64 parts[2])
            else
                ModByReg(parts[1], parts[2])
        | "rcv" -> Rcv(parts[1])
        | "jgz" -> 
            match (parts[1] |> Utilities.isNumeric, parts[2] |> Utilities.isNumeric) with
            | (true, true) -> JgzByVal(int64 parts[1], int64 parts[2])
            | (true, false) -> JgzByValWithReg(int64 parts[1], parts[2])
            | (false, true) -> JgzByRegWithVal(parts[1], int64 parts[2])
            | (false, false) -> JgzByReg(parts[1], parts[2])
        | _ -> failwith "Invalid instruction"
    )

let performOp (program: TheProgram) (instructions: InstructionKind array) = //  (ins: InstructionKind) (registers: Dictionary<string, int64>) (currentfrecuency: int64) =
    let ins = instructions[(int)program.Pointer]
    match ins with
    | Snd(register) -> 
        ({ program with Pointer = program.Pointer + 1L }, Some(program.Registers[register]))

    | SndByVal(value) -> 
        ({ program with Pointer = program.Pointer + 1L }, Some(value))

    | SetByReg(target, from) -> 
        program.Registers[target] <- program.Registers[from]
        ({ program with Pointer = program.Pointer + 1L }, None)

    | SetByVal(target, value) -> 
        program.Registers[target] <- value
        ({ program with Pointer = program.Pointer + 1L }, None)

    | AddByReg(target, from) -> 
        program.Registers[target] <- program.Registers[target] + program.Registers[from]
        ({ program with Pointer = program.Pointer + 1L }, None)

    | AddByVal(target, value) -> 
        program.Registers[target] <- program.Registers[target] + value
        ({ program with Pointer = program.Pointer + 1L }, None)

    | MulByReg(target, from) -> 
        program.Registers[target] <- program.Registers[target] * program.Registers[from]
        ({ program with Pointer = program.Pointer + 1L }, None)

    | MulByVal(target, value) -> 
        program.Registers[target] <- program.Registers[target] * value
        ({ program with Pointer = program.Pointer + 1L }, None)

    | ModByReg(target, from) -> 
        program.Registers[target] <- program.Registers[target] % program.Registers[from]
        ({ program with Pointer = program.Pointer + 1L }, None)

    | ModByVal(target, value) -> 
        program.Registers[target] <- program.Registers[target] % value
        ({ program with Pointer = program.Pointer + 1L }, None)

    | Rcv(register) ->
        if program.Frequencies.Count > 0 then
            program.Registers[register] <- program.Frequencies.Dequeue()
            ({ program with Pointer = program.Pointer + 1L }, None)
        else
            ({ program with Pointer = program.Pointer }, None)

    | JgzByReg(checker, steps) -> 
        if program.Registers[checker] > 0L then
            ({ program with Pointer = program.Pointer + program.Registers[steps] }, None)
        else 
            ({ program with Pointer = program.Pointer + 1L }, None)

    | JgzByRegWithVal(checker, steps) -> 
        if program.Registers[checker] > 0L then 
            ({ program with Pointer = program.Pointer + steps }, None)
        else
            ({ program with Pointer = program.Pointer + 1L }, None)

    | JgzByVal(checker, steps) -> 
        if checker > 0L then
            ({ program with Pointer = program.Pointer + steps }, None)
        else
            ({ program with Pointer = program.Pointer + 1L }, None)

    | JgzByValWithReg(checker, steps) ->
        if checker > 0L then
            ({ program with Pointer = program.Pointer + program.Registers[steps] }, None)
        else
            ({ program with Pointer = program.Pointer + 1L }, None)

let runInstructions(instructions: InstructionKind array) =
    let registers0 = new Dictionary<string, int64>()
    [|'a'..'z'|]
    |> Array.iter (fun c -> registers0.Add(c.ToString(), 0))
    let registers1 = new Dictionary<string, int64>()
    [|'a'..'z'|]
    |> Array.iter (fun c -> registers1.Add(c.ToString(), 0))
    registers1["p"] <- 1L

    let program0 = { Registers = registers0; Pointer = 0; Frequencies = Queue<int64>() }
    let program1 = { Registers = registers1; Pointer = 0; Frequencies = Queue<int64>() }

    let mutable currentindex0 = 0L
    let mutable previndex0 = -1L
    let mutable currentindex1 = 0L
    let mutable previndex1 = -1L

    let mutable sentBy1 = 0

    while currentindex0 <> previndex0 || currentindex1 <> previndex1 do
        let (p0, freq) = performOp ({program0 with Pointer = currentindex0 }) instructions
        previndex0 <- currentindex0
        currentindex0 <- p0.Pointer
        if freq.IsSome then
            program1.Frequencies.Enqueue(freq.Value)
        
        let (p1, freq) = performOp ({program1 with Pointer = currentindex1}) instructions
        previndex1 <- currentindex1
        currentindex1 <- p1.Pointer
        if freq.IsSome then
            sentBy1 <- sentBy1 + 1
            program0.Frequencies.Enqueue(freq.Value)
    
    sentBy1

let execute() =
    let path = "day18/day18_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let instructions = parseContent content

    runInstructions instructions