module day08_part02

open AdventOfCode_2022.Modules

//let path = "day08/test_input_01.txt"
let path = "day08/day08_input.txt"

let createForestVis (inputLines: char [] []) =
    let forest = Array2D.create inputLines.[0].Length inputLines.Length ((0, 0), "V")
    for row in [0..inputLines.Length - 1] do
        for col in [0..inputLines.[row].Length - 1] do
            let value = (int)((string)(inputLines.[row].[col]))
            forest.[row, col] <- ((value, 0), "V")
    forest

let printForestV (currentForest: ((int*int)*string) [, ]) =
    for row in [0..currentForest.GetUpperBound(0)] do
        for col in [0..currentForest.GetUpperBound(1)] do
            printf "%i[%i](%s)" 
                (fst(fst (currentForest[row, col]))) 
                (snd(fst (currentForest[row, col])))
                (snd (currentForest[row, col]))
        printfn "%s" System.Environment.NewLine

let rec checkVisibility (direction: int[]) (pos: int[]) 
    (value: int) (spottrees: int) (visible: bool) 
    (currentForest: ((int*int)*string) [, ]) =
    let newrow = pos.[0] + direction.[0]
    let newcol = pos.[1] + direction.[1]
    if not visible then (spottrees, visible)
    else
        match 
            (newrow <= currentForest.GetUpperBound(0) && newrow >= 0) &&
            (newcol <= currentForest.GetUpperBound(1) && newcol >= 0) with
        | true -> 
            let checkValue = fst (fst currentForest[newrow, newcol])
            checkVisibility direction [|newrow; newcol|] value (spottrees + 1) (value > checkValue) currentForest 
        | false -> (spottrees, visible)


let isVisible (pos: int[]) (currentForest: ((int*int)*string) [, ]) =
    let (row, col) = (pos.[0], pos.[1])
    let value = fst (fst currentForest[row, col])
    let visibleFromL = checkVisibility [|0; -1|] [|row; col|] value 0 true currentForest
    let visibleFromU = checkVisibility [|-1; 0|] [|row; col|] value 0 true currentForest
    let visibleFromR = checkVisibility [|0; 1|] [|row; col|] value 0 true currentForest
    let visibleFromD = checkVisibility [|1; 0|] [|row; col|] value 0 true currentForest
    let setVisible = snd visibleFromL || snd visibleFromU || snd visibleFromR || snd visibleFromD
    let scenicscore = fst visibleFromL * fst visibleFromU * fst visibleFromR * fst visibleFromD
    currentForest[row, col] <- ((value, scenicscore), if setVisible then "V" else "H")

let processForest (currentForest: ((int*int)*string) [, ]) =
    for row in [0..currentForest.GetUpperBound(0)] do
        for col in [0..currentForest.GetUpperBound(1)] do
            isVisible [|row; col|] currentForest
    currentForest


let findMaxScenicScore (currentForest: ((int*int)*string) [, ]) =
    let trees =
        seq {
            for row in [0..currentForest.GetUpperBound(0)] do
                for col in [0..currentForest.GetUpperBound(1)] do
                    if (snd currentForest[row, col] = "V") then 
                        yield (snd (fst currentForest[row, col]))
        }
    trees |> Seq.sortDescending |> Seq.toList |> List.head 

let execute =
    let inputLines = LocalHelper.GetLinesFromFile(path) |> Array.map(fun l -> l.ToCharArray())
    let myforestV = createForestVis inputLines
    let newforest = processForest myforestV
    findMaxScenicScore newforest