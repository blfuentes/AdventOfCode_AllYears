module day01_part02

open AdventOfCode_2025.Modules
open System

type Movement =
    | Left of int
    | Right of int

let parseContent (lines: string array) =
    lines
    |> Array.map (fun line ->
        match line[0] with
        | 'L' -> Left(int(line[1..]))
        | 'R' -> Right(int(line[1..]))
        | _ -> failwith "Invalid input"
    )

let floorDiv a b = Math.Floor(float a / float b) |> int

let rec zeroCrossings start end' =
    if start <= end' then
        floorDiv end' 100 - floorDiv start 100
    else
        zeroCrossings (end' - 1) (start - 1)

let getPassword rotations =
    let step (sum, count) rotation =
        let movement = 
            match rotation with
            | Left steps -> -steps
            | Right steps -> steps
        let newSum = sum + movement
        let crossings = zeroCrossings sum newSum
        (newSum, count + crossings)
    
    rotations |> Array.fold step (50, 0) |> snd

// Brute force...
//let getPassword rotations =
//    let step (pos, count) rotation =
//        let movement = 
//            match rotation with
//            | Left steps -> -steps
//            | Right steps -> steps
//        let mutable newPos = pos
//        let mutable counting = count
//        for _ in 1 .. Math.Abs(movement) do
//            newPos <- newPos + if movement > 0 then 1 else -1
//            if newPos = 100 then
//                newPos <- 0
//            elif newPos < 0 then
//                newPos <- 99
//            if newPos = 0 then
//                counting <- counting + 1
//        (newPos, counting)
    
//    rotations |> Array.fold step (50, 0) |> snd

let execute() =
    //let path = "day01/test_input_01.txt"
    let path = "day01/day01_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let dialectures = parseContent content
    getPassword dialectures