module day08_part01

open AdventOfCode_2022.Modules.LocalHelper

//let path = "day08/test_input_01.txt"
let path = "day08/day08_input.txt"

let createForestVis (inputLines: char [] []) =
    let forest = Array2D.create inputLines.[0].Length inputLines.Length (0, "V")
    for row in [0..inputLines.Length - 1] do
        for col in [0..inputLines.[row].Length - 1] do
            forest.[row, col] <- ((int)((string)(inputLines.[row].[col])), "V")
    forest
let printForestV (forest: (int*string) [, ]) =
    for row in [0..forest.GetUpperBound(0)] do
        for col in [0..forest.GetUpperBound(1)] do
            printf "%i(%s)" (fst (forest[row, col])) (snd (forest[row, col]))
        printfn "%s" System.Environment.NewLine

let rec checkVisibility (direction: int[]) (pos: int[]) (value: int) (visible: bool) (currentForest: (int*string) [, ]) =
    let newrow = pos.[0] + direction.[0]
    let newcol = pos.[1] + direction.[1]
    if not visible then visible
    else
        match 
            (newrow <= currentForest.GetUpperBound(0) && newrow >= 0) &&
            (newcol <= currentForest.GetUpperBound(1) && newcol >= 0) with
        | true -> 
            let checkValue = fst currentForest[newrow, newcol]
            checkVisibility direction [|newrow; newcol|] value (value > checkValue) currentForest 
        | false -> visible


let isVisible (pos: int[]) (currentForest: (int*string) [, ]) =
    let (row, col) = (pos.[0], pos.[1])
    let value = fst currentForest[row, col]
    let visibleFromL = checkVisibility [|0; -1|] [|row; col|] value true currentForest
    let visibleFromU = checkVisibility [|-1; 0|] [|row; col|] value true currentForest
    let visibleFromR = checkVisibility [|0; 1|] [|row; col|] value true currentForest
    let visibleFromD = checkVisibility [|1; 0|] [|row; col|] value true currentForest
    let setVisible = visibleFromL || visibleFromU || visibleFromR || visibleFromD
    currentForest[row, col] <- (fst currentForest[row, col], if setVisible then "V" else "H")

let processForest (forest: (int*string) [, ]) =
    for row in [0..forest.GetUpperBound(0)] do
        for col in [0..forest.GetUpperBound(1)] do
            isVisible [|row; col|] forest
    forest

let countVisibleTrees (forest: (int*string) [, ]) =
    let trees =
        seq {
            for row in [0..forest.GetUpperBound(0)] do
                for col in [0..forest.GetUpperBound(1)] do
                    if (snd forest[row, col] = "V") then 
                        yield (forest[row, col])
        }
    trees |> Seq.length

let execute =
    let inputLines = GetLinesFromFile(path) |> Array.map(fun l -> l.ToCharArray())
    let myforestV = createForestVis inputLines
    let newforest = processForest myforestV
    countVisibleTrees newforest