#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System.Text.RegularExpressions
open AdventOfCode_Utilities
open AdventOfCode_2016.Modules

type Instruction =
    | Rect of columns: int * rows: int
    | RotateColumn of column: int * distance: int
    | RotateRow of row: int * distance: int

//let path = "day08/test_input_01.txt"
let path = "day08/day08_input.txt"

let parseLines (lines: string array) : Instruction list =
    let parseRect(line: string) : Instruction =
        let pattern = "(?<type>rect|rotate) (((?<column>\d+)x(?<row>\d+))|((?<axis>column|row) (x|y)=(?<index>\d+) by (?<steps>\d+)))"
        let regex = new Regex(pattern)
        let matches = regex.Match(line)
        printfn "%s" line
        match matches.Groups["type"].Value with
        | "rect" ->
            Rect((int)matches.Groups["column"].Value, (int)matches.Groups["row"].Value)
        | "rotate" ->
            if matches.Groups["axis"].Value = "column" then
                RotateColumn((int)matches.Groups["index"].Value, (int)matches.Groups["steps"].Value)
            else
                RotateRow((int)matches.Groups["index"].Value, (int)matches.Groups["steps"].Value)
        | _ -> failwith line

    let parseLine (line: string) : Instruction =
        parseRect line

    lines |> Array.map parseLine |> Array.toList

let printMap (map: string[,]) =
    for row in [0..map.GetUpperBound(0) ] do
        for column in [0..map.GetUpperBound(1) ] do
            printf "%s" map[row, column]
        printfn ""

let executeRect (width: int) (tall: int) (map: string[,]) =
    for col in [0..width - 1] do
        for row in [0..tall - 1] do
            map[row, col] <- "#"

let executeRotateColumn (column: int) (distance: int) (map: string[,]) =
    let maxRowIdx = map.GetUpperBound(0)
    printfn "max rows %i" (maxRowIdx + 1)
    let newColumns = Array.create (maxRowIdx + 1) "."
    for row = maxRowIdx downto 0 do
        let currentValue = map[row, column]
        let newIdx = (row + distance)%(maxRowIdx + 1)
        newColumns[newIdx] <- currentValue
        printfn "row %i col %i value %s" newIdx column currentValue
    newColumns
        |> Array.iteri(fun idx v -> map[idx, column] <- v)

let executeRotateRow (row: int) (distance: int) (map: string[,]) =
    let maxColumnIdx = map.GetUpperBound(1)
    printfn "max cols %i" (maxColumnIdx + 1)
    let newRows = Array.create (maxColumnIdx + 1) "."
    for column = maxColumnIdx downto 0 do
        let currentValue = map[row, column]
        let newIdx = (column + distance)%(maxColumnIdx + 1)
        newRows[newIdx] <- currentValue
        printfn "row %i col %i value %s" row column currentValue
    newRows
        |> Array.iteri(fun idx v -> map[row, idx] <- v)


let rec runInstructions (instructions: Instruction list) (map: string[,]) =
    match instructions with
    | instruction::rest ->
        match instruction with
        | Rect (width,tall) -> executeRect width tall map
        | RotateColumn(column, distance) ->  executeRotateColumn column distance map
        | RotateRow(row, distance) -> executeRotateRow row distance map
        printMap map
        runInstructions rest map
    | [] -> map

let lines = LocalHelper.GetLinesFromFile(path)
let rows = 6
let columns = 50
let map = Array2D.create rows columns "."
printMap map
let instructions = parseLines lines

//executeRect 3 2 map
//executeRotateColumn 1 1 map
//executeRotateRow 0 4 map
//executeRotateColumn 1 1 map

runInstructions instructions map
Utilities.flattenArray2D(map) |> Array.filter(fun e -> e = "#") |> Array.length
