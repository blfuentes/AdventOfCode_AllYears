module day11_part02

open AoC_2019.Modules
open System.Collections.Generic

let paintingMap = createPanel(200, 1)
let paintedPositionMap = new Dictionary<(int * int), int>()

let rec getNextPaintPosition(values: Dictionary<bigint, bigint>, relativeBase: bigint, currentDirection: DirectionEnum, position:int[], inputColor:bigint, idx:bigint, paintedCount: int) =
    // GET COLOR
    let colorResult = IntCodeModule.getOutput values idx relativeBase [inputColor] true 0I

    // GET DIRECTION
    let directionResult = IntCodeModule.getOutput values colorResult.Idx colorResult.RelativeBase [0I] true 0I

    // SET COLOR
    paintingMap.[(position.[0], position.[1])] <- (int)colorResult.Output
    let alreadyPainted, value = paintedPositionMap.TryGetValue ((position.[0], position.[1]))
    match alreadyPainted with 
    | false -> paintedPositionMap.Add((position.[0], position.[1]), (int)colorResult.Output)
    | _ -> ()
    

    // SET DIRECTION
    let (nextDirection, nextCoord) =
        match (currentDirection, (int)directionResult.Output) with
        | (UP, 0) -> getNextPosition(LEFT, position)
        | (UP, 1) -> getNextPosition(RIGHT, position)
        | (DOWN, 0) -> getNextPosition(RIGHT, position)
        | (DOWN, 1) -> getNextPosition(LEFT, position)
        | (LEFT, 0) -> getNextPosition(DOWN, position)
        | (LEFT, 1) -> getNextPosition(UP, position)
        | (RIGHT, 0) -> getNextPosition(UP, position)
        | (RIGHT, 1) -> getNextPosition(DOWN, position)
        | (_, _) -> (currentDirection, position)


    let nextColor = bigint paintingMap.[(nextCoord.[0], nextCoord.[1])] 

    match colorResult.Continue with
    | true -> getNextPaintPosition(values, directionResult.RelativeBase, nextDirection, nextCoord, nextColor, directionResult.Idx, paintedCount + 1 )
    | false -> paintedPositionMap.Keys.Count


let execute =
    let filepath = __SOURCE_DIRECTORY__ + @"../../day11_input.txt"
    let initialPoint=[|0;0|]
    let values = IntcodeComputerModule.getInputBigData filepath
    let paintedPositions = getNextPaintPosition(values, 0I, UP, initialPoint, 1I, 0I, 0)
    printPanel(paintingMap, 50, 10)