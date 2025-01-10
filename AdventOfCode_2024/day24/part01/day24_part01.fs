module day24_part01

open System.Collections.Generic
open AdventOfCode_2024.Modules

type Register = {
    Name: string
    Value: bool
}

type Op =
    | AND
    | OR
    | XOR

type Operation = {
    Input1: Register
    Input2: Register
    KindOp: Op
    Output: Register
}

type TheProgram = {
    Registers: Dictionary<string, bool>
    Operations: Operation list
}

let parseContent(lines: string) =
    let (registerpart, operationspart) = (lines.Split("\r\n\r\n")[0], lines.Split("\r\n\r\n")[1])
    let registers = Dictionary<string, bool>()
        
    registerpart.Split("\r\n")
    |> Array.iter(fun r -> 
        registers.Add(r.Split(": ")[0], r.Split(": ")[1] = "1")
    )
    let operations =
        operationspart.Split("\r\n")
        |> Array.map(fun o ->
            let parts = o.Split(" ")
            let kind = 
                match parts[1] with
                | "AND" -> AND
                | "XOR" -> XOR
                | "OR" -> OR
                | _ -> failwith "error"
            { Input1 = { Name = parts[0]; Value = false };
                Input2 = { Name = parts[2]; Value = false };
                KindOp = kind; 
                Output = { Name = parts[4]; Value = false} }
        )

    { Registers = registers; Operations = operations |> List.ofArray }

let runProgram(program: TheProgram) =
    let rec performOp(operations: Operation list) =
        match operations with
        | [] -> true
        | current :: rest ->
            if program.Registers.ContainsKey(current.Input1.Name) && program.Registers.ContainsKey(current.Input2.Name) then
                let (op1, op2) = (program.Registers[current.Input1.Name], program.Registers[current.Input2.Name])
                let result =
                    match current.KindOp with
                    | AND -> { Name = current.Output.Name; Value = op1 && op2 }
                    | XOR -> { Name = current.Output.Name; Value = op1 <> op2 }
                    | OR -> { Name = current.Output.Name; Value = op1 || op2 }
                if program.Registers.ContainsKey(current.Output.Name) then
                    program.Registers[current.Output.Name] <- result.Value
                else
                    program.Registers.Add(current.Output.Name, result.Value)

                performOp rest
            else
                performOp (rest @ [current])

    if performOp program.Operations then
        [for kvp in program.Registers do
            if kvp.Key.StartsWith("z") then 
                kvp
        ] |> List.sortByDescending _.Key |> List.map (fun kvp -> if kvp.Value then "1" else "0")
    else
        []


let execute() =
    let path = "day24/day24_input.txt"

    let content = LocalHelper.GetContentFromFile path
    let theprogram = parseContent content
    let result = runProgram theprogram
    System.Convert.ToInt64((String.concat "" result), 2)
