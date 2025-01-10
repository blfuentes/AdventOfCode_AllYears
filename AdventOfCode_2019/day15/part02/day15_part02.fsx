open System.IO
open System.Collections.Generic
open System
open System.Linq

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

let maxInt = 2147483647I
let maxInt_1 = (maxInt - 1I)   
   
let setDistance(point: KeyValuePair<(bigint * bigint), bigint[]>, visited:Dictionary<(bigint * bigint), bigint[]>)=
    let mutable changed = false
    let ((_x, _y), (_data)) = (point.Key, point.Value)
    if (_data.[1] = 0I) then
        for dir in [1I..4I] do
            let tmpNeighbour = getPoint([|_x; _y|], getMovement(dir))
            let found, tmpDist = visited.TryGetValue ((tmpNeighbour.[0], tmpNeighbour.[1]))
            match found with
            | true ->
                match tmpDist.[0] + 1I < _data.[0] with
                | true -> 
                    visited.[(_x, _y)] <- [|tmpDist.[0] + 1I; _data.[1]|]
                    changed <- true
                | false ->
                    ()
            | false ->
                match maxInt < _data.[0] with
                | true -> 
                    visited.[(_x, _y)] <- [|tmpDist.[0] + 1I; _data.[1]|]
                    changed <- true
                | false ->
                    ()
    changed



let finBSFPath(values: Dictionary<bigint, bigint>, relativeBase: bigint, movInput:bigint, idx:bigint, numberOfInputs: bigint) = 
    let startPoint = [|0; 0|]
    let resultPath = new Queue<int[]>()
    let visitedPoints: List<bigint[]> = new List<bigint[]>()
    resultPath.Enqueue(startPoint)
    let currentPoint = [|0I; 0I; -1I; -1I; maxInt_1; 0I |]
    visitedPoints.Add([|currentPoint.[0]; currentPoint.[1]; currentPoint.[2]; currentPoint.[3]; currentPoint.[4]; currentPoint.[5]|])
    let paramInputs = [|relativeBase; idx; numberOfInputs|]
    let mutable continueLooping = true
    let chainPoints = new List<(bigint[]*bigint[])>()
    chainPoints.Add([|currentPoint.[0]; currentPoint.[1]; currentPoint.[2]|], [|0I; 0I; -1I; -1I|])
    let oxygenPoint = [|-5000I; -5000I|]
    let mutable maxDistance = 0I
    while continueLooping do
        let movInput = currentPoint.[2] + 1I
        if movInput <= 4I then
            let neighbourPoint = getPoint(currentPoint, getMovement(movInput))
            if not(containsElement(visitedPoints, neighbourPoint)) then
                let (output, (idx, notfinished), relativeBase)  =  executeBigDataWithMemory(values, paramInputs.[0], paramInputs.[1], movInput, paramInputs.[2])
                match getStatus(output) with
                | WALL -> 
                    printfn "Found Wall at %A, %A - %A" neighbourPoint.[0] neighbourPoint.[1] (getMovement(movInput))
                    visitedPoints.Add([|neighbourPoint.[0]; neighbourPoint.[1]; 0I; getBackMov(movInput); maxInt_1; 1I|])
                    Array.set currentPoint 2 movInput
                    ()
                | WAY -> 
                    Array.set paramInputs 0 relativeBase
                    Array.set paramInputs 1 idx
                    Array.set paramInputs 2 numberOfInputs
                    printfn "Found WAY at %A, %A - %A" neighbourPoint.[0] neighbourPoint.[1] (getMovement(movInput))
                    chainPoints.Add([|neighbourPoint.[0]; neighbourPoint.[1]; movInput|], [|currentPoint.[0]; currentPoint.[1]; currentPoint.[2]|])
                    visitedPoints.Add([|neighbourPoint.[0]; neighbourPoint.[1]; movInput; getBackMov(movInput); maxInt_1; 0I|])
                    Array.set currentPoint 0 neighbourPoint.[0]
                    Array.set currentPoint 1 neighbourPoint.[1]
                    Array.set currentPoint 2 0I
                    Array.set currentPoint 3 0I
                | OXYGEN -> 
                    printfn "Found Oxygen at %A, %A - %A" neighbourPoint.[0] neighbourPoint.[1] (getMovement(movInput))
                    chainPoints.Add([|neighbourPoint.[0]; neighbourPoint.[1]; movInput|], [|currentPoint.[0]; currentPoint.[1]; currentPoint.[2]|])
                    continueLooping <- notfinished
                    Array.set oxygenPoint 0 neighbourPoint.[0]
                    Array.set oxygenPoint 1 neighbourPoint.[1]
                    visitedPoints.Add([|neighbourPoint.[0]; neighbourPoint.[1]; movInput; getBackMov(movInput); 0I; 1I|])
                    Array.set currentPoint 0 neighbourPoint.[0]
                    Array.set currentPoint 1 neighbourPoint.[1]
                    Array.set currentPoint 2 0I
                    Array.set currentPoint 3 0I
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
                    printfn "BACKWARD at %A, %A - %A" neighbourPoint.[0] neighbourPoint.[1] (getMovement(prevMov))
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

                let mutable updateDistance = true
                let dictWithDistances = new Dictionary<(bigint * bigint), bigint[]>()
                visitedPoints |> Seq.toList |> List.iter(fun x -> dictWithDistances.Add((x.[0], x.[1]), [|x.[4]; x.[5]|]))
                let pointsCount = dictWithDistances.Values.Count - 1
                while updateDistance do
                    updateDistance <- false
                    for idx in [0..pointsCount] do
                        let point = dictWithDistances.ElementAt(idx)
                        let distanceUpdated = setDistance(point, dictWithDistances)
                        updateDistance <- updateDistance || distanceUpdated

                let maxD = dictWithDistances.Values |> Seq.filter(fun x -> x.[1] = 0I) |> Seq.maxBy(fun x -> x.[0])
                maxDistance <- maxD.[0]
                ()
    (oxygenPoint, chainPoints, maxDistance)

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

    let (oxygenPoint, chainPoints, maxDistance) = finBSFPath(values, relBase, movInput, idx, numInputs)
    maxDistance

