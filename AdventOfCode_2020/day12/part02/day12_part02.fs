module day12_part02


open AdventOfCode_Utilities
open CustomDataTypes
open AdventOfCode_2020.Modules

let path = "day12/day12_input.txt"

let inputLines = LocalHelper.GetLinesFromFile(path) 

let movements = 
    seq {
        for line in inputLines do
            let parts =
                match line with
                | Regex @"(?<direction>\w)(?<offset>\d+)" [d; o] -> 
                    let mov =
                        match d with
                        | "N" -> MovementType.NORTH
                        | "S" -> MovementType.SOUTH
                        | "E" -> MovementType.EAST
                        | "W" -> MovementType.WEST
                        | "L" -> MovementType.LEFT
                        | "R" -> MovementType.RIGHT
                        | "F" -> MovementType.FORWARD
                        | _ -> failwith "Invalid Movement type"
                    Some { Mov= mov; Offset = o|> int}
                | _ -> failwith "Invalid input line"
            yield parts.Value
    } |> List.ofSeq

let getNewPosition (shipStatus: MovementOperation[]) (waypointStatus: MovementOperation[]) (movement: MovementOperation) =
    let newCurrentStatus =
        match movement.Mov with
        | MovementType.NORTH -> (shipStatus, [| { Mov = MovementType.NORTH; Offset = waypointStatus.[0].Offset + movement.Offset }; waypointStatus.[1]; waypointStatus.[2]; waypointStatus.[3] |])
        | MovementType.SOUTH -> (shipStatus, [| waypointStatus.[0]; { Mov = MovementType.SOUTH; Offset = waypointStatus.[1].Offset + movement.Offset }; waypointStatus.[2]; waypointStatus.[3] |])
        | MovementType.EAST ->  (shipStatus, [| waypointStatus.[0]; waypointStatus.[1]; { Mov = MovementType.EAST; Offset = waypointStatus.[2].Offset + movement.Offset }; waypointStatus.[3] |])
        | MovementType.WEST ->  (shipStatus, [| waypointStatus.[0]; waypointStatus.[1]; waypointStatus.[2]; { Mov = MovementType.WEST; Offset = waypointStatus.[3].Offset + movement.Offset } |])
        | MovementType.LEFT -> 
            match movement.Offset with
            | 90 -> 
                let waypointNorth = { Mov= MovementType.NORTH; Offset= waypointStatus.[2].Offset }
                let waypointSouth = { Mov= MovementType.SOUTH; Offset= waypointStatus.[3].Offset }
                let waypointEast = { Mov= MovementType.EAST; Offset= waypointStatus.[1].Offset }
                let waypointWest = { Mov= MovementType.WEST; Offset= waypointStatus.[0].Offset }
                (shipStatus, [| waypointNorth; waypointSouth; waypointEast; waypointWest |])
            | 180 ->
                let waypointNorth = { Mov= MovementType.NORTH; Offset= waypointStatus.[1].Offset }
                let waypointSouth = { Mov= MovementType.SOUTH; Offset= waypointStatus.[0].Offset }
                let waypointEast = { Mov= MovementType.EAST; Offset= waypointStatus.[3].Offset }
                let waypointWest = { Mov= MovementType.WEST; Offset= waypointStatus.[2].Offset}
                (shipStatus, [| waypointNorth; waypointSouth; waypointEast; waypointWest |])
            | 270 ->
                let waypointNorth = { Mov= MovementType.NORTH; Offset= waypointStatus.[3].Offset }
                let waypointSouth = { Mov= MovementType.SOUTH; Offset= waypointStatus.[2].Offset }
                let waypointEast = { Mov= MovementType.EAST; Offset= waypointStatus.[0].Offset }
                let waypointWest = { Mov= MovementType.WEST; Offset= waypointStatus.[1].Offset }
                (shipStatus, [| waypointNorth; waypointSouth; waypointEast; waypointWest |])
            | _ -> failwith "Invalid turn left"
        | MovementType.RIGHT -> 
            match movement.Offset with
            | 90 -> 
                let waypointNorth = { Mov= MovementType.NORTH; Offset= waypointStatus.[3].Offset }
                let waypointSouth = { Mov= MovementType.SOUTH; Offset= waypointStatus.[2].Offset }
                let waypointEast = { Mov= MovementType.EAST; Offset= waypointStatus.[0].Offset }
                let waypointWest = { Mov= MovementType.WEST; Offset= waypointStatus.[1].Offset }
                (shipStatus, [| waypointNorth; waypointSouth; waypointEast; waypointWest |])
            | 180 ->
                let waypointNorth = { Mov= MovementType.NORTH; Offset= waypointStatus.[1].Offset }
                let waypointSouth = { Mov= MovementType.SOUTH; Offset= waypointStatus.[0].Offset }
                let waypointEast = { Mov= MovementType.EAST; Offset= waypointStatus.[3].Offset }
                let waypointWest = { Mov= MovementType.WEST; Offset= waypointStatus.[2].Offset }
                (shipStatus, [| waypointNorth; waypointSouth; waypointEast; waypointWest |])
            | 270 ->
                let waypointNorth = { Mov= MovementType.NORTH; Offset= waypointStatus.[2].Offset }
                let waypointSouth = { Mov= MovementType.SOUTH; Offset= waypointStatus.[3].Offset }
                let waypointEast = { Mov= MovementType.EAST; Offset= waypointStatus.[1].Offset }
                let waypointWest = { Mov= MovementType.WEST; Offset= waypointStatus.[0].Offset }
                (shipStatus, [| waypointNorth; waypointSouth; waypointEast; waypointWest |])
            | _ -> failwith "Invalid turn right"
        | MovementType.FORWARD -> 
            let shipNorth = { Mov= MovementType.NORTH; Offset= shipStatus.[0].Offset + waypointStatus.[0].Offset * movement.Offset }
            let shipSouth = { Mov= MovementType.SOUTH; Offset= shipStatus.[1].Offset + waypointStatus.[1].Offset * movement.Offset }
            let shipEast = { Mov= MovementType.EAST; Offset= shipStatus.[2].Offset + waypointStatus.[2].Offset * movement.Offset }
            let shipWeast = { Mov= MovementType.EAST; Offset= shipStatus.[3].Offset + waypointStatus.[3].Offset * movement.Offset }
            ([|shipNorth; shipSouth; shipEast; shipWeast|], waypointStatus)
    newCurrentStatus

let rec findFinalPoint (shipStatus: MovementOperation[]) (waypointStatus: MovementOperation[]) (operations: MovementOperation list) =
    let newPosition = getNewPosition shipStatus waypointStatus operations.Head
    match operations.Tail.IsEmpty with
    | true -> newPosition
    | false -> findFinalPoint (fst newPosition) (snd newPosition) operations.Tail

let startWaypoint = [| {Mov = MovementType.NORTH; Offset= 1}; {Mov = MovementType.SOUTH; Offset= 0}; {Mov = MovementType.EAST; Offset= 10}; {Mov = MovementType.WEST; Offset= 0} |]
let startShip = [| {Mov = MovementType.NORTH; Offset= 0}; {Mov = MovementType.SOUTH; Offset= 0}; {Mov = MovementType.EAST; Offset= 0}; {Mov = MovementType.WEST; Offset= 0} |]

let execute =
    let final = findFinalPoint startShip startWaypoint movements
    abs((fst final).[0].Offset - (fst final).[1].Offset) + abs((fst final).[2].Offset - (fst final).[3].Offset)