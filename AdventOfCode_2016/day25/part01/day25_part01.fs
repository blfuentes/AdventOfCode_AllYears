module day25_part01

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
    | Out of value: int
    | OutReg of registry: string
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
        | "out" ->
            match System.Int32.TryParse parts[1] with
            | true, v ->
                Out(v)
            | false, _ ->
                OutReg(parts[1])
        | _ -> failwith "op error!"

    lines |> Array.map buildop   

let rec runOp (ops: operation array) (p': int) (values: Map<string, int>) (output: int list) (counter: int)=
    let performOp (op: operation) (values: Map<string, int>) (currentoutput: int list) pointer =
        match op with
        | Copy(value, registry) -> 
            (pointer + 1, Map.add registry value values, currentoutput)
        | CopyReg(fromreg, toreg) ->
            (pointer + 1, Map.add toreg values[fromreg] values, currentoutput)
        | Inc(registry) -> 
            (pointer + 1, Map.add registry (values[registry] + 1) values, currentoutput)
        | Dec(registry) -> 
            (pointer + 1, Map.add registry (values[registry] - 1) values, currentoutput)
        | Jump(checker, steps) -> 
            if checker <> 0 then
                (pointer + steps, values, currentoutput)
            else
                (pointer + 1, values, currentoutput)
        | JumpIReg(checker, steps) -> 
            if checker <> 0 then
                (pointer + values[steps], values, currentoutput)
            else
                (pointer + 1, values, currentoutput)
        | JumpReg(checker, steps) -> 
            if values[checker] <> 0 then
                (pointer + steps, values, currentoutput)
            else
                (pointer + 1, values, currentoutput)
        | JumpRegReg(checker, steps) ->
            if values[checker] <> 0 then
                (pointer + values[steps], values, currentoutput)
            else
                (pointer + 1, values, currentoutput)
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
                    | Toggle(r) -> Inc(r) // One argument
                    | Out(v) -> None // One argument
                    | OutReg(r) -> Inc(r) // One argument
                    | None -> None
                ops[index] <- newop
            (pointer + 1, values, currentoutput)
        | Out(v) ->
            (pointer + 1, values, currentoutput @ [v])
        | OutReg(registry) ->
            (pointer + 1, values, currentoutput @ [values[registry]])
        | None ->
            (pointer + 1, values, currentoutput)

    if output.Length > 50 then
        output
        |> List.mapi(fun i v -> 
            if i % 2 = 0 then
                v = 0
            else
                v = 1
        )
        |> List.forall(fun v -> v)
    else
        match ops.Length <= p'  with
        | true -> 
            false
        | false ->
            let (newpointer, newvalues, newoutput) = performOp ops[p'] values output p'
            runOp ops newpointer newvalues newoutput (counter + 1)

let findMinRegA(ops: operation array)(registries: Map<string, int>) =
    let mutable initRegA = 0
    let mutable currentRun = false
    while not currentRun do
        let registry = Map.add "a" initRegA registries
        currentRun <- runOp ops 0 registry [] 0
        initRegA <- initRegA + 1
    initRegA - 1
        

let execute =
    let path = "day25/day25_input.txt"
    let content = LocalHelper.GetLinesFromFile path

    let operations = parseContent content
    let registries = Map[("a", 0); ("b", 0); ("c", 0); ("d", 0)]
    findMinRegA operations registries
