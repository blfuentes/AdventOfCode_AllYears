module day23_part01

open AdventOfCode_2017.Modules
open AdventOfCode_Utilities

open System.Collections.Generic

type InstructionKind =
    | SetByReg of target: string * from: string
    | SetByVal of target: string * value: int64

    | SubByReg of target: string * from: string
    | SubByVal of target: string * value: int64

    | MulByReg of target: string * from: string
    | MulByVal of target:string * value: int64

    | JnzByReg of checker: string * steps: string
    | JnzByRegWithVal of checker: string * steps: int64
    | JnzByVal of checker:int64 * steps: int64
    | JnzByValWithReg of checker:int64 * steps: string

type TheProgram = {
    Registers: Dictionary<string, int64>
    Pointer: int64
}

let parseContent(lines: string array) =
    lines
    |> Array.map (fun line ->
        let parts = line.Split(' ')
        let instruction = parts[0]
        match instruction with
        | "set" -> 
            if parts[2] |> Utilities.isNumeric then
                SetByVal(parts[1], int64 parts[2])
            else
                SetByReg(parts[1], parts[2])
        | "sub" -> 
            if parts[2] |> Utilities.isNumeric then
                SubByVal(parts[1], int64 parts[2])
            else
                SubByReg(parts[1], parts[2])
        | "mul" -> 
            if parts[2] |> Utilities.isNumeric then
                MulByVal(parts[1], int64 parts[2])
            else
                MulByReg(parts[1], parts[2])
        | "jnz" -> 
            match (parts[1] |> Utilities.isNumeric, parts[2] |> Utilities.isNumeric) with
            | (true, true) -> JnzByVal(int64 parts[1], int64 parts[2])
            | (true, false) -> JnzByValWithReg(int64 parts[1], parts[2])
            | (false, true) -> JnzByRegWithVal(parts[1], int64 parts[2])
            | (false, false) -> JnzByReg(parts[1], parts[2])
        | _ -> failwith "Invalid instruction"
    )

let performOp (program: TheProgram) (instructions: InstructionKind array) (currentmuls: int64)=
    let ins = instructions[(int)program.Pointer]
    match ins with
    | SetByReg(target, from) -> 
        program.Registers[target] <- program.Registers[from]
        ({ program with Pointer = program.Pointer + 1L }, currentmuls)

    | SetByVal(target, value) -> 
        program.Registers[target] <- value
        ({ program with Pointer = program.Pointer + 1L }, currentmuls)

    | SubByReg(target, from) -> 
        program.Registers[target] <- program.Registers[target] - program.Registers[from]
        ({ program with Pointer = program.Pointer + 1L }, currentmuls)

    | SubByVal(target, value) -> 
        program.Registers[target] <- program.Registers[target] - value
        ({ program with Pointer = program.Pointer + 1L }, currentmuls)

    | MulByReg(target, from) -> 
        program.Registers[target] <- program.Registers[target] * program.Registers[from]
        ({ program with Pointer = program.Pointer + 1L }, currentmuls + 1L)

    | MulByVal(target, value) -> 
        program.Registers[target] <- program.Registers[target] * value
        ({ program with Pointer = program.Pointer + 1L }, currentmuls + 1L)

    | JnzByReg(checker, steps) -> 
        if program.Registers[checker] <> 0L then
            ({ program with Pointer = program.Pointer + program.Registers[steps] }, currentmuls)
        else 
            ({ program with Pointer = program.Pointer + 1L }, currentmuls)

    | JnzByRegWithVal(checker, steps) -> 
        if program.Registers[checker] <> 0L then 
            ({ program with Pointer = program.Pointer + steps }, currentmuls)
        else
            ({ program with Pointer = program.Pointer + 1L }, currentmuls)

    | JnzByVal(checker, steps) -> 
        if checker <> 0L then
            ({ program with Pointer = program.Pointer + steps }, currentmuls)
        else
            ({ program with Pointer = program.Pointer + 1L }, currentmuls)

    | JnzByValWithReg(checker, steps) ->
        if checker <> 0L then
            ({ program with Pointer = program.Pointer + program.Registers[steps] }, currentmuls)
        else
            ({ program with Pointer = program.Pointer + 1L }, currentmuls)

let runInstructions(instructions: InstructionKind array) =
    let registers = new Dictionary<string, int64>()
    [|'a'..'h'|]
    |> Array.iter (fun c -> registers.Add(c.ToString(), 0))
    let program0 = { Registers = registers; Pointer = 0 }

    let mutable currentindex = 0L
    let mutable currentmuls = 0L
    while currentindex >= 0 && currentindex < instructions.Length do
        let (newprogram, newmuls) = performOp { program0 with Pointer = currentindex } instructions currentmuls
        currentindex <- newprogram.Pointer
        currentmuls <- newmuls
    currentmuls

let execute() =
    let path = "day23/day23_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let instructions = parseContent content
    runInstructions instructions