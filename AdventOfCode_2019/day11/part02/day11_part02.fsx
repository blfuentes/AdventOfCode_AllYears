open System.IO
open System.Collections.Generic

#load @"../../Modules/IntcodeComputerModule.fs"
#load @"../../Modules/HelperModule.fs"
open AoC_2019.Modules

let filepath = __SOURCE_DIRECTORY__ + @"../../day11_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_01.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_02.txt"
//let filepath = __SOURCE_DIRECTORY__ + @"../../test_input_03.txt"


let paintingMap = createPanel(200, 1)
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
    let initialPoint=[|0;0|]
    let values = IntcodeComputerModule.getInputBigData filepath
    let paintedPositions = getNextPaintPosition(values, 0I, UP, initialPoint, 1I, 0I, 0)
    let leftCorner = paintingMap.Keys 
    printPanel(paintingMap, 50, 10)
    //paintedPositions
