module day12_part02

open AdventOfCode_2016.Modules

type operation =
    | Copy of value: int * registry: string
    | CopyReg of fromreg: string * toreg: string
    | Inc of registry: string
    | Dec of registry: string
    | Jump of value: int * steps: int
    | JumpReg of checker: string * steps: int

let parseContent (lines: string array) =
    let buildop (line: string) =
        let parts = line.Split(" ")
        match parts[0] with
        | "cpy" ->
            match System.Int32.TryParse parts[1] with
            | true, v -> 
                Copy(v, parts[2])
            | false, v ->
                CopyReg(parts[1], parts[2])
        | "inc" -> Inc(parts[1])
        | "dec" -> Dec(parts[1])
        | "jnz" ->
            match System.Int32.TryParse parts[1] with
            | true, v ->
                Jump(v, (int)parts[2])
            | false, v ->
                JumpReg(parts[1], (int)parts[2])
        | _ -> failwith "op error!"

    lines |> Array.map buildop

let performOp (op: operation) (values: Map<string, int>) (pointer: int)=
    match op with
    | Copy(value, registry) -> 
        (pointer + 1, Map.add registry value values)
    | CopyReg(fromreg, toreg) ->
        (pointer + 1, Map.add toreg values[fromreg] values)
    | Inc(registry) -> (pointer + 1, Map.add registry (values[registry] + 1) values)
    | Dec(registry) -> (pointer + 1, Map.add registry (values[registry] - 1) values)
    | Jump(checker, steps) -> 
        if checker <> 0 then
            (pointer + steps, values)
        else
            (pointer + 1, values)
    | JumpReg(checker, steps) -> 
        if values[checker] <> 0 then
            (pointer + steps, values)
        else
            (pointer + 1, values)

let rec runOp (ops: operation array) (pointer: int) (values: Map<string, int>)=
    match ops.Length <= pointer with
    | true -> values
    | false ->
        let (newpointer, newvalues) = performOp ops[pointer] values pointer
        runOp ops newpointer newvalues
        

let execute =
    let path = "day12/day12_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let operations = parseContent content
    let registries = Map[("a", 0); ("b", 0); ("c", 1); ("d", 0)]
    let result = runOp operations 0 registries
    result["a"]