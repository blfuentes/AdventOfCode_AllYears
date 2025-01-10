open System.IO
open System.Collections.Generic

#load @"../../Modules/IntcodeComputerModule.fs"
open AoC_2019.Modules

type BlockType = EMPTY | WALL | BLOCK | PADDLE | BALL | NONE

let toBlock(v: bigint) =
    match int(v) with
    | 0 -> EMPTY
    | 1 -> WALL
    | 2 -> BLOCK
    | 3 -> PADDLE
    | 4 -> BALL
    | _ -> NONE

let getBlocksSeq(blocksoutput: List<bigint>) =
    let result = blocksoutput 
                    |> Seq.map int 
                    |> Seq.toList 
                    |> List.chunkBySize(3) 
                    |> List.filter (fun x -> x.Length = 3) 
                    |> List.groupBy (fun b -> toBlock(bigint(b.[2])))
                    |> List.map (fun (block, coordinates) -> (block, coordinates, coordinates.Length))
    result

let rec executeNext(values: Dictionary<bigint, bigint>, relativeBase: bigint, input:bigint, idx:bigint, numberOfInputs: bigint, alloutputs: List<bigint>) =
    let (output, (idx, notfinished), relativeBase)  =  executeBigDataWithMemory(values, relativeBase,idx, input, numberOfInputs)
    alloutputs.Add(output)
    match notfinished with
    | false -> (output, (idx, notfinished), relativeBase)
    | true -> executeNext(values, relativeBase, input, idx, 1I, alloutputs)


//let getBlockScore(blocksoutput: List<bigint>) =
//    let result = blocksoutput 
//                    |> Seq.toList 
//                    |> List.chunkBySize(3) 
//                    |> List.filter (fun x -> x.Length = 3) 
//                    |> List.map List.toArray
//                    |> List.tryFind (fun x -> x.[0] = -1I && x.[1] = 0I )
//    match result with
//    | None -> 0I
//    | Some x -> x.[2]

let round(values: Dictionary<bigint, bigint>, relativeBase: bigint, input:bigint, idx:bigint, numberOfInputs: bigint) = 
    let alloutputs = new List<bigint>()
    let result = executeNext(values, 0I, 0I, 0I, 1I, alloutputs)
    let blocktypes = getBlocksSeq(alloutputs)
    let scoreElement = blocktypes |> List.tryFind(fun (b, l, s) -> b = BlockType.NONE) //getBlockScore(alloutputs)
    let score =
        match scoreElement with
        | None -> [|0; 0; 0|]
        | Some (b, l, s) -> l.[0] |> List.toArray
    let blocks = blocktypes |> List.tryFind (fun x -> 
        let (block, _, size) = x
        block = BlockType.BLOCK)
    let numberOfBlocks =
        match blocks with 
        | None -> 0
        | Some (b, l, s) -> s
    let (paddle, paddleCoordinates, numberOfPaddles) = blocktypes |> List.find (fun x -> 
        let (block, _, size) = x
        block = BlockType.PADDLE)
    let coords = paddleCoordinates.[0] |> List.toArray
    let nextInput = 
        match coords.[0] with
        | direction when direction = 22 -> 0I
        | direction when direction < 22 -> -1I
        | direction when direction > 22 -> 1I
        | _ -> 0I

    (score.[2], nextInput, numberOfBlocks <> 0)

let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day13_input.txt"
    let alloutputs = new List<bigint>()
    let values = IntcodeComputerModule.getInputBigData filepath

    let (score, nextInput, toContinue) = round(values, 0I, 0I, 0I, 1I)
    values.[0I] <- 2I
    let valuesArray = [|nextInput|]
    let mutable continueLooping = toContinue
    let mutable finalScore = score
    while continueLooping do
        let (score, nextInput, numberOfBlocks) = round(values, 0I, valuesArray.[0], 0I, 1I)
        continueLooping <- numberOfBlocks
        finalScore <- score
        Array.set valuesArray 0 nextInput

    finalScore
    
