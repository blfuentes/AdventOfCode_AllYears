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

let rec travelMap (startPosition: Coord) (map: string[,]) (visited: Coord Set) =
    let possiblePositions = getConnections startPosition map
    let nextPosition = Set.difference possiblePositions visited
    if nextPosition.IsEmpty then visited
    else
        let point = nextPosition.MinimumElement
        //printfn "Visiting %A at %i %i" map.[point.row, point.col] point.row point.col
        let visited' = Set.add nextPosition.MinimumElement visited
        travelMap nextPosition.MinimumElement map visited'

let execute =
    //let path = "day10/test_input_02.txt"
    let path = "day10/day10_input.txt"
    let input = LocalHelper.ReadLines path |> Seq.toList
    let (startPoint, map) = buildMap input
    let connections = getConnections startPoint map
    //printfn "%A" connections
    let visited = Set.add(startPoint) Set.empty
    let pipePath = travelMap startPoint map visited
    //printMap map
    pipePath.Count / 2