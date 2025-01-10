#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_2023.Modules

type Coord = {
    row: int
    col: int
}

let buildMap (input: string list) =
    let array = Array2D.create (input.Length) (input.[0].Length) "."
    let startPoint = Array.create 1 { row = 0; col = 0 }

    for rowIdx in 0..input.Length - 1 do
        for colIdx in 0..input.[0].Length - 1 do
            let value = input.[rowIdx].[colIdx].ToString()
            array[rowIdx, colIdx] <- value
            if value = "S" then 
                startPoint.[0] <- { row = rowIdx; col = colIdx }


    (startPoint.[0], array)

let getValidDirections (position: Coord) (map: string[,]) =
    let directions = [[|-1; 0|]; [|1; 0|]; [|0; -1|]; [|0; 1|]]
    let validDirections = directions |> List.filter (fun direction ->
        let row = position.row + direction.[0]
        let col = position.col + direction.[1]
        row >= 0 && row < map.GetLength(0) && col >= 0 && col < map.GetLength(1) && map.[row, col] <> "."
    )
    validDirections

let getStartDirections (position: Coord) (map: string[,]) =
    let availableDirections = getValidDirections position map
    let startDirections = seq {
        for d in availableDirections do
            let posToCheck = { row = position.row + d.[0]; col = position.col + d.[1] }
            let value = map.[posToCheck.row, posToCheck.col]
            match d, value with
            | [|-1; 0|], p when ["|"; "7"; "F"] |> List.contains p -> yield d
            | [|0; -1|], p when ["-"; "L"; "F"] |> List.contains p -> yield d
            | [|0; 1|], p when ["-"; "J"; "7"] |> List.contains p -> yield d
            | [|1; 0|], p when ["|"; "J"; "L"] |> List.contains p -> yield d
            | _ -> ()
    }
    startDirections |> List.ofSeq

let getConnections (position: Coord) (map: string[,])=
    let value = map.[position.row, position.col]
    let directions =
        match value with
        | "|" -> [[|-1; 0|]; [|1; 0|]]
        | "-" -> [[|0; -1|]; [|0; 1|]]
        | "L" -> [[|-1; 0|]; [|0; 1|]]
        | "J" -> [[|-1; 0|]; [|0; -1|]]
        | "7" -> [[|1; 0|]; [|0; -1|]]
        | "F" -> [[|1; 0|]; [|0; 1|]]
        | "S" -> getStartDirections position map
        | _ -> []

    let connections = seq {
        for d in directions do
            let posToCheck = { row = position.row + d.[0]; col = position.col + d.[1] }
            yield { row = posToCheck.row; col = posToCheck.col }
    }
    connections |> Set.ofSeq

let printMap (map: string[,]) =
    for row in 0..map.GetLength(0) - 1 do
        for col in 0..map.GetLength(1) - 1 do
            printf "%s" map.[row, col]
        printfn ""

let rec numOfCuts (from: Coord) (target: Coord) (visited: Coord Set) (map: string[,]) (cuts: int) =
    if from.col = target.col then 
        cuts
    else
        let next = { row = from.row; col = from.col + 1 }
        if Set.contains next visited then 
            let point = map.[next.row, next.col]
            if point = "F" || point = "7" || point = "|" then
                printfn "Found cut at %i %i %s cutting" next.row next.col point 
                numOfCuts next target visited map (cuts + 1)
            else
                numOfCuts next target visited map cuts
        else 
            numOfCuts next target visited map cuts

let rec travelMap (startPosition: Coord) (map: string[,]) (visited: Coord Set) =
    let possiblePositions = getConnections startPosition map
    let nextPosition = Set.difference possiblePositions visited
    if nextPosition.IsEmpty then visited
    else
        let point = nextPosition.MinimumElement
        let visited' = Set.add point visited
        travelMap point map visited'

let replaceStartPoint (map: string[,]) (startPoint: Coord) =
    //let neighboursStartPoint = getConnections startPoint map |> Set.toList |> List.map (fun c -> map.[c.row, c.col])
    let right = map.[startPoint.row, startPoint.col + 1]
    let left = map.[startPoint.row, startPoint.col - 1]
    let top = map.[startPoint.row - 1, startPoint.col]
    let bottom = map.[startPoint.row + 1, startPoint.col]
    
    match right, left, top, bottom  with 
    // horizontal
    | "-", "-", _, _ -> "-"
    | "-", "L", _, _ -> "-"
    | "J", "-", _, _ -> "-"
    | "J", "L", _, _ -> "-"

    // vertical
    | _, _, "|", "|" -> "|"
    | _, _, "|", "J" -> "|"
    | _, _, "7", "|" -> "|"
    | _, _, "7", "J" -> "|"

    // top left
    | _, "-", "|", _ -> "J"
    | _, "-", "7", _ -> "J"
    | _, "L", "|", _ -> "J"
    | _, "L", "7", _ -> "J"
    | _, "F", "|", _ -> "J"
    | _, "F", "7", _ -> "J"

    // top right
    | "-", _, "|", _ -> "L"
    | "-", _, "7", _ -> "L"
    | "J", _, "|", _ -> "L"
    | "J", _, "7", _ -> "L"
    | "7", _, "|", _ -> "L"
    | "7", _, "7", _ -> "L"

    // bottom left
    |  _, "-", _, "|" -> "7"
    |  _, "-", _, "J" -> "7"
    |  _, "L", _, "|" -> "7"
    |  _, "L", _, "J" -> "7"
    |  _, "F", _, "|" -> "7"
    |  _, "F", _, "J" -> "7"

    // bottom right
    | "-", _,  _, "|" -> "F"
    | "-", _,  _, "J" -> "F"
    | "7", _,  _, "|" -> "F"
    | "7", _,  _, "J" -> "F"
    | "J", _,  _, "|" -> "F"
    | "J", _,  _, "J" -> "F"

    | _ -> "S"

let execute =
    //let path = "day10/test_input_05.txt"
    let path = "day10/day10_input.txt"
    let input = LocalHelper.ReadLines path |> Seq.toList
    let (startPoint, map) = buildMap input
    let visited = Set.add(startPoint) Set.empty
    let pipePath = travelMap startPoint map visited

    let pipePath' = pipePath |> List.ofSeq

    let top = pipePath' |> List.minBy (fun p -> p.row)
    let bottom = pipePath' |> List.maxBy (fun p -> p.row)
    let left = pipePath' |> List.minBy (fun p -> p.col)
    let right = pipePath' |> List.maxBy (fun p -> p.col)
    
    printfn "Starting counting inner points"
    map.[startPoint.row, startPoint.col] <- replaceStartPoint map startPoint
    let points =
        seq {
            for rowIdx in (top.row + 1)..(bottom.row - 1) do
                for colIdx in (left.col + 1)..(right.col - 1) do
                    let pointToCheck = { row = rowIdx; col = colIdx }
                    if not (pipePath.Contains(pointToCheck)) then
                        let cuts = numOfCuts pointToCheck { row = rowIdx; col = right.col } pipePath map 0
                        if cuts % 2 = 1 then
                            printfn "Found cut at %i %i %s" rowIdx colIdx map.[rowIdx, colIdx]
                            yield { row = rowIdx; col = colIdx }
        } |> List.ofSeq
    points.Length