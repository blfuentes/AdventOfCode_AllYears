module day24_part02

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

let findWrongWires (program: TheProgram) =
    let maxId = 45

    let generateId id = id.ToString().PadLeft(2, '0')

    let findOp predicate = program.Operations |> List.tryFind predicate

    let incorrect =
        [0..maxId - 1]
        |> Seq.collect (fun id ->
            let currentId = generateId id

            let xorWire = findOp (fun op ->
                (
                    (op.Input1.Name = $"x{currentId}" && op.Input2.Name = $"y{currentId}") ||
                    (op.Input1.Name = $"y{currentId}" && op.Input2.Name = $"x{currentId}")
                ) &&
                op.KindOp.IsXOR
            )

            let andWire = findOp (fun op ->
                (
                    (op.Input1.Name = $"x{currentId}" && op.Input2.Name = $"y{currentId}") ||
                    (op.Input1.Name = $"y{currentId}" && op.Input2.Name = $"x{currentId}")
                ) &&
                op.KindOp.IsAND
            )

            let zWire = findOp (fun op -> op.Output.Name = $"z{currentId}")

            seq {
                match zWire with
                | Some z when not z.KindOp.IsXOR -> yield z.Output.Name
                | _ -> ()

                match xorWire, andWire, zWire with
                | Some xor, Some andOp, Some _ ->
                    let orWire = findOp (fun op ->
                        op.Input1.Name = andOp.Output.Name ||
                        op.Input2.Name = andOp.Output.Name
                    )

                    if Option.isSome orWire && not orWire.Value.KindOp.IsOR && id > 0 then
                        yield andOp.Output.Name

                    let connectedWire = findOp (fun op ->
                        op.Input1.Name = xor.Output.Name ||
                        op.Input2.Name = xor.Output.Name
                    )

                    if Option.isSome connectedWire && connectedWire.Value.KindOp.IsOR then
                        yield xor.Output.Name
                | _ -> ()
            }
        )
        |> Seq.toList

    let wrong =
        program.Operations
        |> List.filter (fun op ->
            not (op.Input1.Name.StartsWith("x") || op.Input1.Name.StartsWith("y")) &&
            not (op.Input2.Name.StartsWith("x") || op.Input2.Name.StartsWith("y")) &&
            not (op.Output.Name.StartsWith("z")) &&
            op.KindOp.IsXOR)
        |> List.map (fun op -> op.Output.Name)

    incorrect @ wrong
    |> Seq.distinct
    |> Seq.sort


let execute() =
    let path = "day24/day24_input.txt"

    let content = LocalHelper.GetContentFromFile path
    let theprogram = parseContent content
    let result = findWrongWires theprogram
    String.concat "," result