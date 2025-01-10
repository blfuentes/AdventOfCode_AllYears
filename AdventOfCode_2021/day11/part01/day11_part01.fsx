open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic

let path = "day11_input.txt"
//let path = "test_input.txt"
//let path = "test_input_00.txt"

let octopusCollection = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.map(fun l -> l.ToCharArray() |> Array.map string |> Array.map int) |> Seq.toArray

let printBoard (board:int[][]) =
    for row in [0 .. board.Length - 1] do
        for col in [0 .. board.[0].Length - 1] do
            printf "%i" board.[row].[col]
        printfn ""

//printBoard octopusCollection

let numberOfSteps = 100

let getSurroundedOctopuses(octopuses: int[][], row: int, col: int) =
    let moves = seq {
        for v in [-1 .. 1] do
            for h in [-1 .. 1] do
                if v<> 0 || h <> 0 then yield [|v; h|]
    }
    let minV = 0
    let maxV = octopuses.Length
    let minH = 0
    let maxH = octopuses.[0].Length

    let neighbours = seq {
        for m in moves do
            let cRow = row + m.[0]
            let cCol = col + m.[1]
            if (cRow >= minV && cRow < maxV && cCol >= minH && cCol < maxH) then yield (cRow, cCol)
    }
    neighbours |> Seq.toList
                

let increase(octopuses: int[][]) =
    for row in [0 .. octopuses.Length - 1] do
        for col in [0 .. octopuses.[0].Length - 1] do
            octopuses.[row].[col] <- octopuses.[row].[col] + 1          

let reset(octopuses: int[][]) =
    for row in [0 .. octopuses.Length - 1] do
        for col in [0 .. octopuses.[0].Length - 1] do
            if octopuses.[row].[col] > 9 then                
                octopuses.[row].[col] <- 0

let rec emitflash(octopuses: int[][], numberOfFlashes: int, oldFlashes: int) =
    match numberOfFlashes = oldFlashes with
    | true -> numberOfFlashes
    | false ->
        let tmpFlashes:int[] = [|numberOfFlashes|]
        for row in [0 .. octopuses.Length - 1] do
            for col in [0 .. octopuses.[0].Length - 1] do
                if octopuses.[row].[col] = 10 then 
                    tmpFlashes.[0] <- tmpFlashes.[0] + 1
                    octopuses.[row].[col] <- octopuses.[row].[col] + 1
                    let closeOctopuses = getSurroundedOctopuses(octopuses, row, col)
                    closeOctopuses |> List.filter(fun o -> octopuses.[fst o].[snd o] < 10) |> List.iter(fun o -> octopuses.[fst o].[snd o] <- octopuses.[fst o].[snd o] + 1)
        emitflash(octopuses, tmpFlashes.[0], numberOfFlashes)

let rec round (octopuses: int[][], currentStep: int, finalStep: int, numberOfFlashes: int) =
    printfn "Starting step %i" currentStep
    let nextStep = currentStep + 1
    printBoard octopuses
    increase(octopuses)
    let newflashes = emitflash(octopuses, 0, -1)
    reset(octopuses)
    printfn "Finished step %i" currentStep
    if nextStep <= finalStep then round(octopuses, nextStep, finalStep, numberOfFlashes + newflashes) else numberOfFlashes + newflashes

let simulate (octopuses: int[][], finalStep: int) =
    round(octopuses, 1, finalStep, 0)

simulate(octopusCollection, numberOfSteps)