module day09_part02

open System
open AdventOfCode_2022.Modules

let path = "day09/day09_input.txt"

let getDirection (mov: string) =
    match mov with 
    | "L" -> [|0; -1|]
    | "U" -> [|-1; 0|]
    | "R" -> [|0; 1|]
    | "D" -> [|1; 0|]
    | _ -> [|0; 0|]
                    

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

let rec generateNewRope (ropeLenth: int) (initHead: int[]) (rope: int[] list) (mov: int[]) (resultrope: int[] list) =
    if rope.Length = ropeLenth then 
        let newPosHead = [|rope.Head.[0] + mov.[0]; rope.Head.[1] + mov.[1]|]
        let newResultRope = resultrope @ [newPosHead]
        generateNewRope ropeLenth newPosHead rope.Tail [|0; 0|] newResultRope
    else
        match rope with
        | head :: tail ->
            let newInitHead = if isTouching initHead head then head else getMov initHead head 
            let newResultRope = resultrope @ [newInitHead]
            generateNewRope ropeLenth newInitHead tail [|0; 0|] newResultRope
        | [] -> resultrope

let rec performStep (rope: int[] list) (mov: int[] * int) (visited: int[] list) =
    match snd mov with
    | 0 -> (rope, visited)
    | _ -> 
        let newRope = generateNewRope rope.Length [|0; 0|] rope (fst mov) []//getTouchingPos newPosHead (fst mov) 
        let nextMov = (fst mov, (snd mov) - 1)
        let lastTail = (newRope |> List.rev).Head
        let newVisited = if visited |> List.contains lastTail then visited else visited @ [lastTail]
        performStep newRope nextMov newVisited

let rec run (rope: int[] list) (movs: (int[] * int) list) (visited: int[] list) =
    match movs with
    | head :: tail ->
        let nextRun = performStep rope head visited
        let newSnake = fst nextRun
        let newVisited = snd nextRun
        run newSnake tail newVisited
    | [] -> visited

let execute =
    let inputLines = LocalHelper.GetLinesFromFile(path)
    let movements = inputLines |> Array.map(fun l -> getDirection (l.Split(' ').[0]), (int)(l.Split(' ').[1])) |> Array.toList
    let initRope = [0..9] |> List.map(fun i -> [|0; 0|])
    run initRope movements [] |> List.length