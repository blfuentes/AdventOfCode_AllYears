open System.IO
open Microsoft.FSharp.Collections
open System.Text.RegularExpressions

let filepath = __SOURCE_DIRECTORY__ + @"../../day03_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_01.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_03.txt"

let wires = File.ReadAllLines(filepath)
let wireA = wires.[0]
let wireB = wires.[1]

// U: (0, 1) D: (0, -1) L: (-1, 0) R: (1, 0)
let xDirection = [|0; 0; -1; 1|];
let yDirection = [|1; -1; 0; 0|];

//let pointsA
let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

let getPoints(wire:string) = 
    let initPosition:int array = Array.zeroCreate 2
    let mutable result:Map<int array, int> = Map.empty
    wire.Split(',')
        |> Array.iter (fun _operation ->
            match _operation with
            | Regex @"([aA-zZ])[-. ]?(\d+)" [op; movs] -> 
                // printfn "operation %s for %s movements" op movs
                let mutable idxOp = 0
                match op with
                | "U" -> idxOp <- 0
                | "D" -> idxOp <- 1
                | "L" -> idxOp <- 2
                | "R" -> idxOp <- 3
                | _ -> ()

                for idx in [|1..int movs|] do
                    Array.set initPosition 0 (initPosition.[0] + xDirection.[idxOp])
                    Array.set initPosition 1 (initPosition.[1] + yDirection.[idxOp])
                    let tmpElem = result.TryFind initPosition
                    match tmpElem with
                    | Some x -> () //printfn "element found"
                    | None -> result <- result.Add([|initPosition.[0]; initPosition.[1]|], (abs(initPosition.[0]) + abs(initPosition.[1]))) 
            | _ -> ()
        )

    result
let pointsA = getPoints wireA
let pointsB = getPoints wireB
 
let intersect a b = Map (seq {
    for KeyValue(k, va) in a do
        match Map.tryFind k b with
        | Some vb -> yield k, va
        | None    -> () })

//let commonPoints = intersect pointsA pointsB |> Seq.minBy (fun _point -> _point.Value)
let lowestPoint = intersect pointsA pointsB |> Seq.minBy (fun _point -> _point.Value) 

printfn "shortest distance is %d" lowestPoint.Value