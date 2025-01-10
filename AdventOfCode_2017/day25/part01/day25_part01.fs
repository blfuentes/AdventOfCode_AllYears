module day25_part01

open System.Text.RegularExpressions

open AdventOfCode_2017.Modules
open System.Collections.Generic

let beginpattern = @"Begin in state ([A-Z])."
let diagnosticpattern = @"Perform a diagnostic checksum after (\d+) steps."
let inpattern = @"In state ([A-Z]):"
let ifpattern = @"  If the current value is (\d+):"
let writepattern = @"    - Write the value (\d+)."
let movepattern = @"    - Move one slot to the (left|right)."
let continuepattern = @"    - Continue with state ([A-Z])."

type Direction =
    | Left
    | Right

type StateAction = {
    WriteValue: int
    MoveDir: Direction
    NextState: string
}

type State = {
    Name: string
    IfZero: StateAction
    IfOne: StateAction
}

type TheProgram = {
    Begin: string
    Steps: int
    States: State list
}

let parseContent(input: string) =
    let blocks = input.Split("\r\n\r\n")
    let init = blocks[0].Split("\r\n")
    let beginState = Regex.Match(init[0], beginpattern).Groups[1].Value
    let numOfSteps = (int)Regex.Match(init[1], diagnosticpattern).Groups[1].Value

    let states = 
        [for bIdx in 1..blocks.Length-1 do
            let statename = Regex.Match(blocks[bIdx].Split("\r\n")[0],inpattern).Groups[1].Value

            let ifzero = Regex.Match(blocks[bIdx].Split("\r\n")[1],ifpattern).Groups[1].Value
            let writeIfZero = (int)Regex.Match(blocks[bIdx].Split("\r\n")[2],writepattern).Groups[1].Value
            let moveIfZero =
                match Regex.Match(blocks[bIdx].Split("\r\n")[3],movepattern).Groups[1].Value with
                | "left" -> Left
                | "right" -> Right
                | _ -> failwith "error"
            let continueIfZero = Regex.Match(blocks[bIdx].Split("\r\n")[4],continuepattern).Groups[1].Value

            let ifZero = { WriteValue = writeIfZero; MoveDir = moveIfZero; NextState = continueIfZero }

            let ifOne = Regex.Match(blocks[bIdx].Split("\r\n")[5],ifpattern).Groups[1].Value
            let writeIfOne = (int)Regex.Match(blocks[bIdx].Split("\r\n")[6],writepattern).Groups[1].Value
            let moveIfOne = 
                match Regex.Match(blocks[bIdx].Split("\r\n")[7],movepattern).Groups[1].Value with
                | "left" -> Left
                | "right" -> Right
                | _ -> failwith "error"
            let continueIfOne = Regex.Match(blocks[bIdx].Split("\r\n")[8],continuepattern).Groups[1].Value

            let ifOne = { WriteValue = writeIfOne; MoveDir = moveIfOne; NextState = continueIfOne }

            yield { Name = statename; IfZero = ifZero; IfOne = ifOne }
        ]

    { Begin = beginState; Steps = numOfSteps; States = states }

let runTuringMachine(program: TheProgram) =
    let positionsValues = Dictionary<int, int>()
    positionsValues.Add(0, 0)
    let rec executeState (pointer: int) (nextState: State) (executions: int) =
        if executions = program.Steps then
            positionsValues.Values |> Seq.sum
        else
            match positionsValues[pointer] with
            | 1 ->
                let nextValue = nextState.IfOne.WriteValue
                if positionsValues.ContainsKey(pointer) then
                    positionsValues[pointer] <- nextValue
                else
                    positionsValues[pointer] <- nextValue
                let nextPointer = 
                    match nextState.IfOne.MoveDir with
                    | Left -> pointer - 1
                    | Right -> pointer + 1
    
                if not (positionsValues.ContainsKey(nextPointer)) then
                    positionsValues.Add(nextPointer, 0)
                
                let next' = program.States |> List.find(fun s -> s.Name = nextState.IfOne.NextState)
                executeState nextPointer next' (executions + 1)
            | 0 ->
                let nextValue = nextState.IfZero.WriteValue
                if positionsValues.ContainsKey(pointer) then
                    positionsValues[pointer] <- nextValue
                else
                    positionsValues[pointer] <- nextValue

                let nextPointer = 
                    match nextState.IfZero.MoveDir with
                    | Left -> pointer - 1
                    | Right -> pointer + 1

                if not (positionsValues.ContainsKey(nextPointer)) then
                    positionsValues.Add(nextPointer, 0)

                let next' = program.States |> List.find(fun s -> s.Name = nextState.IfZero.NextState)
                executeState nextPointer next' (executions + 1)
            | _ -> failwith "error"

    let initstate = program.States |> List.find(fun s -> s.Name = program.Begin)
    executeState 0 initstate 0

let execute() =
    let path = "day25/day25_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let theprogram = parseContent content

    runTuringMachine theprogram