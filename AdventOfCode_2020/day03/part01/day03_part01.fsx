open System.IO
open System.Collections.Generic
open AdventOfCode_2020.Modules.LocalHelper

#load @"../../Model/CustomDataTypes.fs"
#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

let file = "test_input.txt"
//let file = "day03_input.txt"
let path = "day03/" + file

let values = GetLinesFromFile(path) |> Array.ofSeq |> Array.map (fun line -> line.ToCharArray())

let width = values.[0].Length
let height = values.Length

let trees =
    seq {
        for idx in [|0..height - 1|] do
            for jdx in [|0 .. width - 1|] do
                match values.[idx].[jdx] with
                    | '#' -> yield [|jdx; idx|]
                    | _  -> ()
    } |> List.ofSeq

let transportTrees (numPos: int) (input: list<int[]>) : list<int[]> =
    input |> List.map (fun t -> [|t.[0] + numPos * width; t.[1]|])
//let trees2 = transportTrees 1 trees

let rec countCollisions (currentForest: list<int[]>) initX initY maxwidth maxheight right down =
    match initY <= maxheight with
    | true -> 
        let point = [|initX + right; initY + down|]
        let newWidth =
            match point.[0] >= maxwidth with
            | true -> maxwidth + width
            | false -> maxwidth
        let newForest =
            match point.[0] >= maxwidth with
            | true -> transportTrees 1 currentForest
            | false -> currentForest

        match newForest |> List.exists (fun t -> t.[0] = point.[0] && t.[1] = point.[1]) with 
        | true -> 1 + (countCollisions newForest (initX + right) (initY + down) newWidth maxheight right down)
        | _ -> countCollisions newForest (initX + right) (initY + down) newWidth maxheight right down
    | false -> 0

countCollisions trees 0 0 width height 3 1


let getCollisions (currentForest: list<int[]>) initX initY right down maxwidth maxheight =
    let positions = [initY..down..maxheight]
    seq {
        for pos in initY..down..maxheight do
            let currentPos = positions |> List.findIndex (fun x -> x = pos)
            let point = [|((initX + right) * (currentPos + 1)) % maxwidth; pos + down|]
            match currentForest |> List.exists (fun t -> t.[0] = point.[0] && t.[1] = point.[1]) with 
            | true -> yield point
            | _ -> ()
    } |> Seq.length
getCollisions trees 0 0 3 1 width height


let mutable idx = 0
let mutable forests = 0
let mutable maxWidth = width 
let points =
    seq {
        for jdx in [|0..height - 1|] do
            let point = [|idx + 3; jdx + 1|]
            if point.[0] >= maxWidth then 
                forests <- forests + 1
                maxWidth <- maxWidth + width
            else
                forests <- forests
            let checkForest = transportTrees forests trees
            match checkForest |> List.exists (fun t -> t.[0] = point.[0] && t.[1] = point.[1]) with 
            | true -> yield point
            | _ -> ()
            idx <- idx + 3
    } |>List.ofSeq
points.Length