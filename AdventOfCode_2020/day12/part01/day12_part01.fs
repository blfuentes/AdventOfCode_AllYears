module day12_part01


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

let getNewPosition (currentStatus: MovementOperation[]) (currentPosition: MovementOperation) (movement: MovementOperation) =
    let newCurrentStatus =
        match movement.Mov with
        | MovementType.NORTH -> (currentPosition, [| { Mov = MovementType.NORTH; Offset = currentStatus.[0].Offset + movement.Offset }; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
        | MovementType.SOUTH -> (currentPosition, [| currentStatus.[0]; { Mov = MovementType.SOUTH; Offset = currentStatus.[1].Offset + movement.Offset }; currentStatus.[2]; currentStatus.[3] |])
        | MovementType.EAST ->  (currentPosition, [| currentStatus.[0]; currentStatus.[1]; { Mov = MovementType.EAST; Offset = currentStatus.[2].Offset + movement.Offset }; currentStatus.[3] |])
        | MovementType.WEST ->  (currentPosition, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; { Mov = MovementType.WEST; Offset = currentStatus.[3].Offset + movement.Offset } |])
        | MovementType.LEFT -> 
            match (movement.Offset, currentPosition) with
            | (90, mov) when mov.Mov = MovementType.NORTH -> ({ Mov= MovementType.WEST; Offset= currentPosition.Offset } , [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (90, mov) when mov.Mov = MovementType.SOUTH -> ({ Mov= MovementType.EAST; Offset= currentPosition.Offset } , [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (90, mov) when mov.Mov = MovementType.EAST ->  ({ Mov= MovementType.NORTH; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (90, mov) when mov.Mov = MovementType.WEST ->  ({ Mov= MovementType.SOUTH; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (180, mov) when mov.Mov = MovementType.NORTH -> ({ Mov= MovementType.SOUTH; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (180, mov) when mov.Mov = MovementType.SOUTH -> ({ Mov= MovementType.NORTH; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (180, mov) when mov.Mov = MovementType.EAST ->  ({ Mov= MovementType.WEST; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (180, mov) when mov.Mov = MovementType.WEST ->  ({ Mov= MovementType.EAST; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (270, mov) when mov.Mov = MovementType.NORTH -> ({ Mov= MovementType.EAST; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (270, mov) when mov.Mov = MovementType.SOUTH -> ({ Mov= MovementType.WEST; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (270, mov) when mov.Mov = MovementType.EAST ->  ({ Mov= MovementType.SOUTH; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (270, mov) when mov.Mov = MovementType.WEST ->  ({ Mov= MovementType.NORTH; Offset= currentPosition.Offset }, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | _ -> failwith "Invalid turn left"
        | MovementType.RIGHT -> 
            match (movement.Offset, currentPosition) with
            | (90, mov) when mov.Mov = MovementType.NORTH -> ({Mov= MovementType.EAST; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (90, mov) when mov.Mov = MovementType.SOUTH -> ({Mov= MovementType.WEST; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (90, mov) when mov.Mov = MovementType.EAST ->  ({Mov= MovementType.SOUTH; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (90, mov) when mov.Mov = MovementType.WEST ->  ({Mov= MovementType.NORTH; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (180, mov) when mov.Mov = MovementType.NORTH -> ({Mov= MovementType.SOUTH; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (180, mov) when mov.Mov = MovementType.SOUTH -> ({Mov= MovementType.NORTH; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (180, mov) when mov.Mov = MovementType.EAST ->  ({Mov= MovementType.WEST; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (180, mov) when mov.Mov = MovementType.WEST ->  ({Mov= MovementType.EAST; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (270, mov) when mov.Mov = MovementType.NORTH -> ({Mov= MovementType.WEST; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (270, mov) when mov.Mov = MovementType.SOUTH -> ({Mov= MovementType.EAST; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (270, mov) when mov.Mov = MovementType.EAST ->  ({Mov= MovementType.NORTH; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | (270, mov) when mov.Mov = MovementType.WEST ->  ({Mov= MovementType.SOUTH; Offset= currentPosition.Offset}, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | _ -> failwith "Invalid turn right"
        | MovementType.FORWARD ->
            match currentPosition.Mov with
            | MovementType.NORTH -> (currentPosition, [| { Mov = MovementType.NORTH; Offset = currentStatus.[0].Offset + movement.Offset }; currentStatus.[1]; currentStatus.[2]; currentStatus.[3] |])
            | MovementType.SOUTH -> (currentPosition, [| currentStatus.[0]; { Mov = MovementType.SOUTH; Offset = currentStatus.[1].Offset + movement.Offset }; currentStatus.[2]; currentStatus.[3] |])
            | MovementType.EAST ->  (currentPosition, [| currentStatus.[0]; currentStatus.[1]; { Mov = MovementType.EAST; Offset = currentStatus.[2].Offset + movement.Offset }; currentStatus.[3] |])
            | MovementType.WEST ->  (currentPosition, [| currentStatus.[0]; currentStatus.[1]; currentStatus.[2]; { Mov = MovementType.WEST; Offset = currentStatus.[3].Offset + movement.Offset } |])
            | _ -> failwith "Invalid forward"
    newCurrentStatus

let rec findFinalPoint (start: MovementOperation) (currentStatus: MovementOperation[]) (operations: MovementOperation list) =
    let newPosition = getNewPosition currentStatus start operations.Head
    match operations.Tail.IsEmpty with
    | true -> newPosition
    | false -> findFinalPoint (fst newPosition) (snd newPosition) operations.Tail

let startPoint = { Mov = MovementType.EAST; Offset = 0 }
let startStatus = [| {Mov = MovementType.NORTH; Offset= 0}; {Mov = MovementType.SOUTH; Offset= 0}; {Mov = MovementType.EAST; Offset= 0}; {Mov = MovementType.WEST; Offset= 0} |]

let execute =
    let final = findFinalPoint startPoint startStatus movements
    abs((snd final).[0].Offset - (snd final).[1].Offset) + abs((snd final).[2].Offset - (snd final).[3].Offset)