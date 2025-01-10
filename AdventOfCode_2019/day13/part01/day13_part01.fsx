open System.IO
open System.Collections.Generic

#load @"../../Modules/IntcodeComputerModule.fs"
open AoC_2019.Modules

type BlockType = EMPTY | WALL | BLOCK | PADDLE | BALL | NONE

let output = "1,2,3,6,5,4,3,4,3"

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
    | false -> output
    | true -> executeNext(values, relativeBase, input, idx, 1I, alloutputs)



let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day13_input.txt"
    let alloutputs = new List<bigint>()
    let values = IntcodeComputerModule.getInputBigData filepath
    let result = executeNext(values, 0I, 0I, 0I, 1I, alloutputs)
    let (block, coordinates, size) = getBlocksSeq(alloutputs) |> List.find (fun x -> 
        let (block, _, size) = x
        block = BlockType.BLOCK)
    size
    