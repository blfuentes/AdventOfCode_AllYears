module day23_part02

open AdventOfCode_Utilities
open AdventOfCode_2016.Modules

type operation =
    | Copy of value: int * registry: string
    | CopyReg of fromreg: string * toreg: string
    | Inc of registry: string
    | Dec of registry: string
    | Jump of value: int * steps: int
    | JumpReg of checker: string * steps: int
    | JumpIReg of checker: int * steps: string
    | JumpRegReg of checker: string * steps: string
    | Toggle of registry: string
    | None

let parseContent (lines: string array) =
    let buildop (line: string) =
        let parts = line.Split(" ")
        match parts[0] with
        | "cpy" ->
            match System.Int32.TryParse parts[1] with
            | true, v -> 
                Copy(v, parts[2])
            | false, _ ->
                CopyReg(parts[1], parts[2])
        | "inc" -> Inc(parts[1])
        | "dec" -> Dec(parts[1])
        | "jnz" ->
            match System.Int32.TryParse parts[1] with
            | true, v ->
                match System.Int32.TryParse parts[2] with
                | true, v' ->
                    Jump(v, v')
                | false, _ ->
                    JumpIReg(v, parts[2])
            | false, _ ->
                JumpReg(parts[1], (int)parts[2])
        | "tgl" -> Toggle(parts[1])
        | _ -> failwith "op error!"

    lines |> Array.map buildop   

let rec runOp (ops: operation array) (p': int) (values: Map<string, int>)=
    let performOp (op: operation) (values: Map<string, int>) pointer=
        match op with
        | Copy(value, registry) -> 
            (pointer + 1, Map.add registry value values)
        | CopyReg(fromreg, toreg) ->
            (pointer + 1, Map.add toreg values[fromreg] values)
        | Inc(registry) -> 
            (pointer + 1, Map.add registry (values[registry] + 1) values)
        | Dec(registry) -> 
            (pointer + 1, Map.add registry (values[registry] - 1) values)
        | Jump(checker, steps) -> 
            if checker <> 0 then
                (pointer + steps, values)
            else
                (pointer + 1, values)
        | JumpIReg(checker, steps) -> 
            if checker <> 0 then
                (pointer + values[steps], values)
            else
                (pointer + 1, values)
        | JumpReg(checker, steps) -> 
            if values[checker] <> 0 then
                (pointer + steps, values)
            else
                (pointer + 1, values)
        | JumpRegReg(checker, steps) ->
            if values[checker] <> 0 then
                (pointer + values[steps], values)
            else
                (pointer + 1, values)
        | Toggle(registry) ->
            let index = values[registry] + pointer
            if index < ops.Length then
                let opToSwitch = ops[index]
                let newop =
                    match opToSwitch with
                    | Copy(v, r) -> JumpIReg(v, r) // two arguments
                    | CopyReg(r1, r2) -> JumpRegReg(r1, r2) // two arguments
                    | Inc(r) -> Dec(r) // One argument
                    | Dec(r) -> Inc(r) // One Argument
                    | Jump(v, s) -> None // Two argument
                    | JumpReg(c, s) -> None // Two argument
                    | JumpIReg(c, s) -> Copy(c, s) // Two argument
                    | JumpRegReg(r1, r2) -> CopyReg(r1, r2) // Two argument
                    | Toggle(r) -> Inc(r) // One Argument
                    | None -> None
                ops[index] <- newop
            (pointer + 1, values)
        | None ->
            (pointer + 1, values)

    match ops.Length <= p'  with
    | true -> values
    | false ->
        let (newpointer, newvalues) = performOp ops[p'] values p'
        runOp ops newpointer newvalues

// hacked solution: If we leave the code running we can identify the loophole using c and d registries.
// identify the instructions in the input with unusual values, 
// cpy XXX c
// jnz YYY d
// these values are a constant for hacking the loophole: XXX * YYY
// then We just need to add the numberofeggs!  (factorial)
let runProgram((xxx, yyy): int*int) (numOfEggs: int) =
    xxx * yyy + factorialWithMemoization numOfEggs

let execute =
    let path = "day23/day23_input.txt"

    // Brute force solution takes ~5min
    //let content = LocalHelper.GetLinesFromFile path

    //let operations = parseContent content
    //let registries = Map[("a", 12); ("b", 0); ("c", 0); ("d", 0)]

    //let result = runOp operations 0 registries
    //result["a"]

    runProgram (71,75) 12