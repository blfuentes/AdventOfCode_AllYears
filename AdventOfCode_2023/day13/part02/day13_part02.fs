module day13_part02


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

let rec checkHMirrorPoint (group: char[,]) (currentRow: int) (lastmirror: int)=
    if currentRow = group.GetLength(0) || lastmirror > 0 then
        lastmirror
    else
        let mirrored' =
            seq {
                for i in 0..currentRow - 1 do
                    let upRow = currentRow - i - 1
                    let downRow = currentRow + i
                    if downRow >= group.GetLength(0) then
                        yield (upRow, 0)
                    else
                        let up = group.[upRow, *]
                        let down = group.[downRow, *]
                        if up = down then
                            yield (upRow, 0)
                        else
                            let numOfDiffs = Array.map2 (fun a b -> if a = b then 0 else 1) up down |> Array.sum
                            yield (-1, numOfDiffs)
                        
            } |> Seq.toList
        let diffRows = mirrored' |> List.filter(fun m -> (fst m) = -1)
        let sumOfdiffs = diffRows |> List.sumBy(fun m -> snd m)
        let lastmirror' = 
            if lastmirror = 0 && (diffRows.Length = 1 && sumOfdiffs = 1) then 
                currentRow 
            else 
                lastmirror
        checkHMirrorPoint group (currentRow + 1) lastmirror'

let rec checkVMirrorPoint (group: char[,]) (currentCol: int) (lastmirror: int) =
    if currentCol = group.GetLength(1) || lastmirror > 0 then
        lastmirror   
    else
        let mirrored' =
            seq {
                for i in 0..currentCol - 1 do
                    let leftCol = currentCol - i - 1
                    let rightCol = currentCol + i
                    if rightCol >= group.GetLength(1) then
                        yield (leftCol, 0)
                    else
                        let left = group.[*, leftCol]
                        let right = group.[*, rightCol]
                        if left = right then
                            yield (leftCol, 0)
                        else
                            let numOfDiffs = Array.map2 (fun a b -> if a = b then 0 else 1) left right |> Array.sum
                            yield (-1, numOfDiffs)
                        
            } |> Seq.toList
        let diffCols = mirrored' |> List.filter(fun m -> (fst m) = -1)
        let sumOfdiffs = diffCols |> List.sumBy(fun m -> snd m)
        let lastmirror' = 
            if lastmirror = 0 && (diffCols.Length = 1 && sumOfdiffs = 1) then 
                currentCol 
            else 
                lastmirror
        checkVMirrorPoint group (currentCol + 1) lastmirror'

                

let calculateMirror (mirrors: int * int) =
    let (vMirror, hMirror) = mirrors
    vMirror + hMirror * 100

let execute =
    let path = "day13/day13_input.txt"
    let lines = LocalHelper.ReadLines path |> Seq.toList
    let groups = getGroupsOnSeparator lines ""
    let maps = groups |> List.map parseGroup
    maps 
    |> List.map(fun g -> (checkVMirrorPoint g 0 0, checkHMirrorPoint g 0 0)) 
    |> List.map calculateMirror 
    |> List.sum