open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day05_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
let values = File.ReadAllText(filepath).Split(',')
                |> Array.map int

// prepare the tranche input

let performOperation(input:int, idx: int, opDef: string array) =
    let op = int(opDef.[4]) + int(opDef.[3]) * 10
    let param1Mode = int opDef.[2]
    let param2Mode = int opDef.[1]
    let param3Mode = int opDef.[0]

    match op with
    | 1 -> // ADD
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)
        Array.set values values.[idx + 3] (fst parameters + snd parameters)
        (4, true)
    | 2 -> // MULTIPLY
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)
        Array.set values values.[idx + 3] (fst parameters * snd parameters)
        (4, true)
    | 3 -> // WRITE INPUT
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[idx + 1], 0)
            | (_, _) -> (0, 0)
        Array.set values (fst parameters) input
        (2, true)
    | 4 -> // OUTPUT
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], 0)
            | (_, _) -> (0, 0)
        printfn "%d" (fst parameters)
        //input <- values.[fst parameters]
        (2, true)
    | 5 -> // JUMP IF TRUE
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)

        match fst parameters with
        | 0 -> (3, true)
        | _ -> ((snd parameters) - idx, true)
    | 6 -> // JUMP IF FALSE
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)

        match fst parameters with
        | 0 -> ((snd parameters) - idx, true)
        | _ -> (3, true)  
    | 7 -> // LESS THAN
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)

        if (fst parameters < snd parameters) then Array.set values values.[idx + 3] 1
        else Array.set values values.[idx + 3] 0
        (4, true)
    | 8 -> // EQUALS
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)

        if (fst parameters = snd parameters) then Array.set values values.[idx + 3] 1
        else Array.set values values.[idx + 3] 0
        (4, true)
    | 99 -> (0, false)
    | _ -> (0, true)

let execute(input:int) =
    let mutable continueLooping = true
    let mutable increment = 4
    let mutable idx = 0
    
    while continueLooping && idx < values.Length do
        let opDefinition = values.[idx].ToString().PadLeft(5, '0') |> Seq.toArray |> Array.map string 
        let resultOp = performOperation(input, idx, opDefinition)
        increment <- fst resultOp
        continueLooping <- snd resultOp
        idx <- idx + increment 

        //printfn "first position in loop %d = %d" idx values.[0]
    //printfn "first position %d" values.[0]
    //values.[0]
execute 5



