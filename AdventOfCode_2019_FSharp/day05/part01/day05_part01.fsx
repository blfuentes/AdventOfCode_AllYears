open System.IO

let filepath = __SOURCE_DIRECTORY__ + @"../../day05_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
let values = File.ReadAllText(filepath).Split(',')
                |> Array.map int

// prepare the tranche input 
//Array.set values 1 12
//Array.set values 2 2
let mutable input = 1

let performOperation(idx: int, opDef: string array) =
    let op = int(opDef.[4]) + int(opDef.[3]) * 10
    let param1Mode = int opDef.[2]
    let param2Mode = int opDef.[1]
    let param3Mode = int opDef.[0]

    match op with
    | 1 -> 
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)
        Array.set values values.[idx + 3] (fst parameters + snd parameters)
        (4, true)
    | 2 -> 
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], values.[values.[idx + 2]])
            | (0, 1) -> (values.[values.[idx + 1]], values.[idx + 2])
            | (1, 0) -> (values.[idx + 1], values.[values.[idx + 2]])
            | (1, 1) -> (values.[idx + 1], values.[idx + 2])
            | (_, _) -> (0, 0)
        Array.set values values.[idx + 3] (fst parameters * snd parameters)
        (4, true)
    | 3 -> 
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[idx + 1], 0)
            | (_, _) -> (0, 0)
        Array.set values (fst parameters) input
        (2, true)
    | 4 -> 
        let parameters = 
            match (param1Mode, param2Mode) with
            | (0, 0) -> (values.[values.[idx + 1]], 0)
            | (_, _) -> (0, 0)
        printfn "%d" (fst parameters)
        //input <- values.[fst parameters]
        (2, true)
    | 99 -> (0, false)
    | _ -> (0, true)

let testsAreOk(output: list<int>): bool =
    output |> Seq.rev |> Seq.skip(1) |> Seq.forall (fun x -> x = 0)
    

let execute =
    let mutable continueLooping = true
    let mutable increment = 4
    let mutable idx = 0
    
    while continueLooping && idx < values.Length do
        let opDefinition = values.[idx].ToString().PadLeft(5, '0') |> Seq.toArray |> Array.map string 
        let resultOp = performOperation(idx, opDefinition)
        increment <- fst resultOp
        continueLooping <- snd resultOp
        idx <- idx + increment 

        //printfn "first position in loop %d = %d" idx values.[0]
    //printfn "first position %d" values.[0]
    //values.[0]
execute


