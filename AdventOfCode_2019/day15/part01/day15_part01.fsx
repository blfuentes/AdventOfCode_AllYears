open System.IO
open System.Collections.Generic

#load @"../../Modules/IntcodeComputerModule.fs"
open AoC_2019.Modules

type StatusType = WALL | WAY | OXYGEN | NONE
type MovementType = NORTH | SOUTH | WEST | EAST | NONE

let getStatus(status: bigint) =
    match int(status) with
    | 0 -> StatusType.WALL
    | 1 -> StatusType.WAY
    | 2 -> StatusType.OXYGEN
    | _ -> StatusType.NONE

let getMovement(movement: bigint) =
    match int(movement) with
    | 1 -> MovementType.NORTH
    | 2 -> MovementType.SOUTH
    | 3 -> MovementType.WEST
    | 4 -> MovementType.EAST
    | _ -> MovementType.NONE

let containsElement(theSeq: seq<bigint[]>, element: bigint[]) =
    let found = theSeq |> Seq.tryFind (fun x -> x.[0] = element.[0] && x.[1] = element.[1])
    found <> None

let getPoint(currPoint: bigint[], mov: MovementType) =
    let newPoint = 
        match mov with
        | NORTH -> [|currPoint.[0]; currPoint.[1] + 1I|]
        | SOUTH -> [|currPoint.[0]; currPoint.[1] - 1I|]
        | EAST -> [|currPoint.[0] - 1I; currPoint.[1]|]
        | WEST -> [|currPoint.[0] + 1I; currPoint.[1]|]
        | _ -> currPoint
    newPoint

let getBackMov(movement: bigint) =
    match int(movement) with
    | 1 -> 2I
    | 2 -> 1I
    | 3 -> 4I
    | 4 -> 3I
    | 0 -> 0I
    | _ -> 0I

let getPrevPoint(elems: List<(bigint[]*bigint[])>, point: bigint[]) =
    let tmpPoint = elems.Find(fun (c, p) -> c.[0] = point.[0] && c.[1] = point.[1])
    tmpPoint

let finBSFPath(values: Dictionary<bigint, bigint>, relativeBase: bigint, movInput:bigint, idx:bigint, numberOfInputs: bigint) = 
    let startPoint = [|0; 0|]
    let resultPath = new Queue<int[]>()
    let visitedPoints: List<bigint[]> = new List<bigint[]>()
    resultPath.Enqueue(startPoint)
    let currentPoint = [|0I; 0I; 0I; 0I |]
    visitedPoints.Add([|currentPoint.[0]; currentPoint.[1]; currentPoint.[2]|])
    let paramInputs = [|relativeBase; idx; numberOfInputs|]
    let mutable continueLooping = true
    let chainPoints = new List<(bigint[]*bigint[])>()
    chainPoints.Add([|currentPoint.[0]; currentPoint.[1]; currentPoint.[2]|], [|0I; 0I; -1I; -1I|])
    let oxygenPoint = [|-5000I; -5000I|]
    while continueLooping do
        let movInput = currentPoint.[2] + 1I
        if movInput <= 4I then
            let neighbourPoint = getPoint(currentPoint, getMovement(movInput))
            if not(containsElement(visitedPoints, neighbourPoint)) then
                let (output, (idx, notfinished), relativeBase)  =  executeBigDataWithMemory(values, paramInputs.[0], paramInputs.[1], movInput, paramInputs.[2])
                match getStatus(output) with
                | WALL -> 
                    //printfn "Found Wall at %A, %A - %A" neighbourPoint.[0] neighbourPoint.[1] (getMovement(movInput))
                    visitedPoints.Add([|neighbourPoint.[0]; neighbourPoint.[1]; 0I; getBackMov(movInput)|])
                    Array.set currentPoint 2 movInput
                    ()
                | WAY -> 
                    Array.set paramInputs 0 relativeBase
                    Array.set paramInputs 1 idx
                    Array.set paramInputs 2 numberOfInputs
                    //printfn "Found WAY at %A, %A - %A" neighbourPoint.[0] neighbourPoint.[1] (getMovement(movInput))
                    chainPoints.Add([|neighbourPoint.[0]; neighbourPoint.[1]; movInput|], [|currentPoint.[0]; currentPoint.[1]; currentPoint.[2]|])
                    visitedPoints.Add([|neighbourPoint.[0]; neighbourPoint.[1]; movInput; getBackMov(movInput)|])
                    Array.set currentPoint 0 neighbourPoint.[0]
                    Array.set currentPoint 1 neighbourPoint.[1]
                    Array.set currentPoint 2 0I
                    Array.set currentPoint 3 0I
                | OXYGEN -> 
                    //printfn "Found Oxygen at %A, %A - %A" neighbourPoint.[0] neighbourPoint.[1] (getMovement(movInput))
                    chainPoints.Add([|neighbourPoint.[0]; neighbourPoint.[1]; movInput|], [|currentPoint.[0]; currentPoint.[1]; currentPoint.[2]|])
                    continueLooping <- false
                    Array.set oxygenPoint 0 neighbourPoint.[0]
                    Array.set oxygenPoint 1 neighbourPoint.[1]
                    ()
                | _ ->
                    ()
            else
                Array.set currentPoint 2 movInput
        else
            let prevPoint = visitedPoints.Find(fun x -> x.[0] = currentPoint.[0] && x.[1] = currentPoint.[1])
            let prevMov = prevPoint.[3]
            if prevMov > 0I then 
                let neighbourPoint = getPoint(currentPoint, getMovement(prevMov))
                let (output, (idx, notfinished), relativeBase)  =  executeBigDataWithMemory(values, paramInputs.[0], paramInputs.[1], prevMov, paramInputs.[2])
                match getStatus(output) with
                | WAY 
                | OXYGEN ->
                    Array.set paramInputs 0 relativeBase
                    Array.set paramInputs 1 idx
                    Array.set paramInputs 2 numberOfInputs
                    //printfn "BACKWARD at %A, %A - %A" neighbourPoint.[0] neighbourPoint.[1] (getMovement(prevMov))
                    let prevPoint2 = visitedPoints.Find(fun x -> x.[0] = neighbourPoint.[0] && x.[1] = neighbourPoint.[1])
                    Array.set currentPoint 0 neighbourPoint.[0]
                    Array.set currentPoint 1 neighbourPoint.[1]
                    Array.set currentPoint 2 0I
                    Array.set currentPoint 3 prevPoint2.[3]
                    ()
                | _ ->
                    continueLooping <- false
                    ()
            else
                continueLooping <- false
                ()
    (oxygenPoint, chainPoints)

let rec countSteps(oxyPoint:bigint[], chainPoints: List<(bigint[]*bigint[])>) =
    let (curr, prevPoint) = chainPoints.Find(fun (c, p) -> c.[0] = oxyPoint.[0] && c.[1] = oxyPoint.[1])
    if prevPoint.[2] = -1I && prevPoint.[3] = -1I then
        1
    else
        1 + countSteps(prevPoint, chainPoints) 

let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day15_input.txt"
    let alloutputs = new List<bigint>()
    let values = IntcodeComputerModule.getInputBigData filepath
    
    let relBase = 0I
    let movInput = 0I
    let idx = 0I
    let numInputs = 1I

    let (oxygenPoint, chainPoints) = finBSFPath(values, relBase, movInput, idx, numInputs)
    countSteps(oxygenPoint, chainPoints) - 1
