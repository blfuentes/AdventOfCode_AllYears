open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day09/test_input_01.txt"
let path = "day09/day09_input.txt"

let inputLines = GetLinesFromFile(path)

let getDirection (mov: string) =
    match mov with 
    | "L" -> [|0; -1|]
    | "U" -> [|-1; 0|]
    | "R" -> [|0; 1|]
    | "D" -> [|1; 0|]
    | _ -> [|0; 0|]

let movements = inputLines |> Array.map(fun l -> getDirection (l.Split(' ').[0]), (int)(l.Split(' ').[1])) |> Array.toList

let distance (pointA: int[]) (pointB: int[]) =
    Math.Sqrt((Math.Pow((float)(pointA.[0] - pointB.[0]), 2.0) + Math.Pow((float)(pointA.[1] - pointB.[1]), 2.0)))

let maxDistance = distance [|0; 0|] [|1; 1|]

let isTouching (pointA: int[]) (pointB: int[]) =
    let dis = distance pointA pointB 
    dis <= maxDistance

let getTouchingPos (target: int[]) (mov: int[])  =
    [|target.[0] - mov.[0]; target.[1] - mov.[1]|]

 //getTouchingPos [|1; 3|]  [|0; 1|]
 //getTouchingPos [|3; 1|]  [|1; 0|]
 //getTouchingPos [|1; 2|]  [|-1; 0|]
 //getTouchingPos [|2; 3|]  [|0; 1|]

let rec performStep (posHead: int[]) (posTail: int[]) (mov: int[] * int) (visited: int[] list) =
    match snd mov with
    | 0 -> ((posHead, posTail), visited)
    | _ -> 
        let newPosHead = [|posHead.[0] + (fst mov).[0]; posHead.[1] + (fst mov).[1]|]
        let newPosTail = if isTouching newPosHead posTail then posTail else getTouchingPos newPosHead (fst mov) 
        let nextMov = (fst mov, (snd mov) - 1)
        let newVisited = if visited |> List.contains newPosTail then visited else visited @ [newPosTail]
        performStep newPosHead newPosTail nextMov newVisited

let rec run (posHead: int[]) (posTail: int[]) (movs: (int[] * int) list) (visited: int[] list) =
    match movs with
    | head :: tail ->
        let nextRun = performStep posHead posTail head visited
        let newHead = fst(fst(nextRun))
        let newTail = snd(fst(nextRun))
        let newVisited = snd nextRun
        run newHead newTail tail newVisited
    | [] -> visited

let initPosHead = [|0; 0|]
let initPosTail = [|0; 0|]

run initPosHead initPosTail movements [] |> List.length