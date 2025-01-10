module day18_part01

open AdventOfCode_2017.Modules
open AdventOfCode_Utilities

open System.Collections.Generic

type InstructionKind =
    | Snd of register: string
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

let parseContent(lines: string array) =
    lines
    |> Array.map (fun line ->
        let parts = line.Split(' ')
        let instruction = parts[0]
        match instruction with
        | "snd" -> Snd(parts[1])
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

let performOp(ins: InstructionKind) (registers: Dictionary<string, int64>) (currentfrecuency: int64) =
    match ins with
    | Snd(register) -> 
        (registers[register], 1L, false)
    | SetByReg(target, from) -> 
        registers[target] <- registers[from]
        (currentfrecuency, 1L, false)
    | SetByVal(target, value) -> 
        registers[target] <- value
        (currentfrecuency, 1L, false)
    | AddByReg(target, from) -> 
        registers[target] <- registers[target] + registers[from]
        (currentfrecuency, 1L, false)
    | AddByVal(target, value) -> 
        registers[target] <- registers[target] + value
        (currentfrecuency, 1L, false)
    | MulByReg(target, from) -> 
        registers[target] <- registers[target] * registers[from]
        (currentfrecuency, 1L, false)
    | MulByVal(target, value) -> 
        registers[target] <- registers[target] * value
        (currentfrecuency, 1L, false)
    | ModByReg(target, from) -> 
        registers[target] <- registers[target] % registers[from]
        (currentfrecuency, 1L, false)
    | ModByVal(target, value) -> 
        registers[target] <- registers[target] % value
        (currentfrecuency, 1L, false)
    | Rcv(register) -> 
        if registers[register] = 0L then
            (currentfrecuency, 1L, false)
        else
            (currentfrecuency, 1L, true)
    | JgzByReg(checker, steps) -> 
        if registers[checker] > 0L then
            (currentfrecuency, registers[steps], false)
        else 
            (currentfrecuency, 1L, false)
    | JgzByRegWithVal(checker, steps) -> 
        if registers[checker] > 0L then 
            (currentfrecuency, steps, false)
        else
            (currentfrecuency, 1L, false)
    | JgzByVal(checker, steps) -> 
        if checker > 0L then
            (currentfrecuency, steps, false)
        else
            (currentfrecuency, 1L, false)
    | JgzByValWithReg(checker, steps) ->
        if checker > 0L then
            (currentfrecuency, registers[steps], false)
        else
            (currentfrecuency, 1L, false)

let runInstructions(instructions: InstructionKind array) =
    let registers = new Dictionary<string, int64>()
    [|'a'..'z'|]
    |> Array.iter (fun c -> registers.Add(c.ToString(), 0))
    let mutable currentfrecuency = 0L
    let mutable currentindex = 0
    let mutable recovered = false
    while currentindex >= 0 && currentindex < instructions.Length && not recovered do
        let ins = instructions[currentindex]
        let (newfrecuency, steps, isrecovered) = performOp ins registers currentfrecuency
        currentfrecuency <- newfrecuency
        recovered <- isrecovered && newfrecuency <> 0L
        currentindex <- currentindex + ((int)steps)
    currentfrecuency

let execute() =
    let path = "day18/day18_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let instructions = parseContent content

    runInstructions instructions
