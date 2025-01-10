#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../../AdventOfCode_2023/Modules/LocalHelper.fs"

open System
open System.Collections.Generic

open AdventOfCode_2023.Modules
open AdventOfCode_Utilities


let parseGroup (lines:string list) =
    let rows = lines.Length
    let cols = lines.[0].Length
    let group = Array2D.create rows cols '0'
    for i in 0..rows - 1 do
        for j in 0..cols - 1 do
            group.[i,j] <- lines.[i].[j]
    group


let printGroup (group:char[,]) =
    for i in 0..group.GetLength(0)-1 do
        for j in 0..group.GetLength(1)-1 do
            printf "%c" group.[i,j]
        printfn ""

let rec checkHMirrorPoint (group: char[,]) (currentRow: int) (lastmirror: int)=
    if currentRow = group.GetLength(0) then
        //printfn "Last horizontal mirror: %i" lastmirror
        //printGroup group
        lastmirror
    else
        let mirrored' =
            seq {
                for i in 0..currentRow - 1 do
                    let upRow = currentRow - i - 1
                    let downRow = currentRow + i
                    if downRow >= group.GetLength(0) then
                        yield upRow
                    else
                        let up = group.[upRow, *]
                        let down = group.[downRow, *]
                        if up = down then
                            yield upRow
                        else
                            yield -1
                        
            } |> Seq.toList
        let lastmirror' = if (mirrored' |> List.forall((<>) -1)) then currentRow else lastmirror
        checkHMirrorPoint group (currentRow + 1) lastmirror'

let rec checkVMirrorPoint (group: char[,]) (currentCol: int) (lastmirror: int) =
    if currentCol = group.GetLength(1) then
        //printfn "Last vertical mirror: %i" lastmirror
        lastmirror   
    else
        let mirrored' =
            seq {
                for i in 0..currentCol - 1 do
                    let leftCol = currentCol - i - 1
                    let rightCol = currentCol + i
                    if rightCol >= group.GetLength(1) then
                        yield leftCol
                    else
                        let left = group.[*, leftCol]
                        let right = group.[*, rightCol]
                        if left = right then
                            yield leftCol
                        else
                            yield -1
                        
            } |> Seq.toList
        let lastmirror' = if (mirrored' |> List.forall((<>) -1)) then currentCol else lastmirror
        checkVMirrorPoint group (currentCol + 1) lastmirror'

                

let calculateMirror (mirrors: int * int) =
    let (vMirror, hMirror) = mirrors
    vMirror + hMirror * 100

let execute =
    //let path = "day13/test_input_01.txt"
    //let path = "day13/test_input_02.txt"
    let path = "day13/day13_input.txt"
    let lines = LocalHelper.ReadLines path |> Seq.toList
    let groups = getGroupsOnSeparator lines ""
    let maps = groups |> List.map parseGroup
    //maps |> List.iter printGroup
    maps |> List.map(fun g -> (checkVMirrorPoint g 0 0, checkHMirrorPoint g 0 0)) |> List.map calculateMirror |> List.sum