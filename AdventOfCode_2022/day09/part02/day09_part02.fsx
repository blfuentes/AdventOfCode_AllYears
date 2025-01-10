open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

let path = "day09/test_input_01.txt"
//let path = "day09/test_input_02.txt"
//let path = "day09/day09_input.txt"

let limit = [|-4; 0; 0; 5|]
//let limit = [|-15; 5; -11; 14|]

let inputLines = GetLinesFromFile(path)

let getDirectionLetter (value : int[]) =
    match value with 
    | [|0; -1|] -> "L" 
    | [|-1; 0|] -> "U"
    | [|0; 1|] -> "R" 
    | [|1; 0|] -> "D"
    | _ -> "" 

let getDirection (mov: string) =
    match mov with 
    | "L" -> [|0; -1|]
    | "U" -> [|-1; 0|]
    | "R" -> [|0; 1|]
    | "D" -> [|1; 0|]
    | _ -> [|0; 0|]

let movements = inputLines |> Array.map(fun l -> getDirection (l.Split(' ').[0]), (int)(l.Split(' ').[1])) |> Array.toList

let printRopeMap (limits: int[]) (rope: int[] list) =
    for row in [limits.[0]..limits.[1]] do
        for col in [limits.[2]..limits.[3]] do
            let symbol =
                match [|row; col|] = rope.Head with
                | true -> "H"
                | false ->
                    match rope |> List.tryFindIndex(fun e -> e = [|row; col|]) with
                    | Some(found) -> found.ToString()
                    | None -> "."
            printf "%s" symbol
        printf"%s" System.Environment.NewLine
                    

let distance (pointA: int[]) (pointB: int[]) =
    Math.Sqrt((Math.Pow((float)(pointA.[0] - pointB.[0]), 2.0) + Math.Pow((float)(pointA.[1] - pointB.[1]), 2.0)))

let maxDistance = distance [|0; 0|] [|1; 1|]

let isTouching (pointA: int[]) (pointB: int[]) =
    let dis = distance pointA pointB 
    dis <= maxDistance

let getMov (target: int[]) (init: int[]) =
    let possiblePositions = 
        seq {
            for row in [-1..1] do
                for col in [-1..1] do
                    let checkPos = [|init.[0] + row; init.[1] + col|]
                    yield (checkPos, distance checkPos target)
        } |> Seq.sortBy(fun p -> snd p) |> Seq.toList
    fst possiblePositions.Head
    

let rec generateNewRope (initHead: int[]) (rope: int[] list) (mov: int[]) (resultrope: int[] list) =
    if rope.Length = 10 then 
        let newPosHead = [|rope.Head.[0] + mov.[0]; rope.Head.[1] + mov.[1]|]
        let newResultRope = resultrope @ [newPosHead]
        generateNewRope newPosHead rope.Tail [|0; 0|] newResultRope
    else
        match rope with
        | head :: tail ->
            let newInitHead = if isTouching initHead head then head else getMov initHead head 
            let newResultRope = resultrope @ [newInitHead]
            generateNewRope newInitHead tail [|0; 0|] newResultRope
        | [] -> resultrope

let rec performStep (rope: int[] list) (mov: int[] * int) (visited: int[] list) =
    match snd mov with
    | 0 -> (rope, visited)
    | _ -> 
        let newRope = generateNewRope [|0; 0|] rope (fst mov) []//getTouchingPos newPosHead (fst mov) 
        let nextMov = (fst mov, (snd mov) - 1)
        let lastTail = (newRope |> List.rev).Head
        let newVisited = if visited |> List.contains lastTail then visited else visited @ [lastTail]
        printfn "Printing %s %i" (getDirectionLetter (fst mov)) (snd mov)
        printRopeMap limit newRope
        performStep newRope nextMov newVisited

let rec run (rope: int[] list) (movs: (int[] * int) list) (visited: int[] list) =
    match movs with
    | head :: tail ->
        printfn "=========== Printing %s %i ===========" (getDirectionLetter (fst head)) (snd head)
        let nextRun = performStep rope head visited
        let newSnake = fst nextRun
        let newVisited = snd nextRun
        run newSnake tail newVisited
    | [] -> visited

let initPosHead = [|0; 0|]
let initRope = [0..9] |> List.map(fun i -> [|0; 0|])

let result = run initRope movements [] |> List.length
printfn "Result: %i" result