open System.IO
open System.Collections.Generic

#load @"../../Modules/IntcodeComputerModule.fs"
open AoC_2019.Modules

type DirectionEnum = UP | DOWN | LEFT | RIGHT

let filepath = __SOURCE_DIRECTORY__ + @"../../day11_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_01.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_03.txt"

let values = IntcodeComputerModule.getInputBigData filepath
let (result0, result1) = (executeBigData(filepath, 0I), executeBigData(filepath, 1I))

let bigint (x:int) = bigint(x)  // looks recursive, but isn't

let createPanel(size: int) =
    let panel = new Dictionary<(int * int), int>()
    for idx in [0 .. size] do
        for jdx in [0 .. size] do
            panel.Add((jdx, idx), 0)      
    panel

let getNextPosition(direction: DirectionEnum, position:int[]) =
    match direction with
    | UP -> (direction, [|position.[0]; position.[1] - 1|])
    | DOWN -> (direction, [|position.[0]; position.[1] + 1|])
    | LEFT -> (direction, [|position.[0] - 1; position.[1]|])
    | RIGHT -> (direction, [|position.[0] + 1; position.[1]|])

let paintingMap = createPanel 500
let paintedPositionMap = new Dictionary<(int * int), int>()
let rec getNextPaintPosition(values: Dictionary<bigint, bigint>, relativeBase: bigint, currentDirection: DirectionEnum, position:int[], inputColor:bigint, idx:bigint, paintedCount: int) =
    // GET COLOR
    let (paintingOutput, (paintingInstructionIdx, paintingNotFinished), paintingRelativeBase)  =  executeBigDataWithMemory(values, relativeBase, idx, inputColor, 1I)
    
    // GET DIRECTION
    let (movementOutput, (movementInstructionIdx, movementNotFinished), movementRelativeBase) =  executeBigDataWithMemory(values, paintingRelativeBase, paintingInstructionIdx, inputColor, 1I)

    // SET COLOR
    paintingMap.[(position.[0], position.[1])] <- (int)paintingOutput
    let alreadyPainted, value = paintedPositionMap.TryGetValue ((position.[0], position.[1]))
    let nextColor = paintingMap.[(position.[0], position.[1])]
    match alreadyPainted with 
    | false -> paintedPositionMap.Add((position.[0], position.[1]), (int)paintingOutput)
    | _ -> ()
        

    // SET DIRECTION
    let (nextDirection, nextCoord) =
        match (currentDirection, (int)movementOutput) with
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

    match paintingNotFinished with
    | true -> getNextPaintPosition(values, movementRelativeBase, nextDirection, nextCoord, nextColor, movementInstructionIdx, paintedCount + 1 )
    | false -> paintedPositionMap.Keys.Count


let execute =
    let initialPoint=[|60;60|]
    let values = IntcodeComputerModule.getInputBigData filepath
    let paintedPositions = getNextPaintPosition(values, 0I, UP, initialPoint, 0I, 0I, 0)
    paintedPositions
