module day21_part02

open System
open AdventOfCode_2023.Modules
open FSharpx.Collections

type Coord = {
    Row: int
    Col: int
}

let numOfDirs = 4
let rowDirs = [| -1; 0; 1; 0|]
let colDirs = [| 0; 1; 0; -1 |]

let border = Int32.MaxValue / 4

let parseInput (input: string list) =
    let rows = input.Length
    let cols = input.[0].Length
    let grid = Array2D.create rows cols '.'
    for row in 0..rows-1 do
        for col in 0..cols-1 do
            grid.[row, col] <- input.[row].[col]
    grid
 
let calculate (grid: char[,]) (steps: int) =
    let maxRows = grid.GetLength(0)
    let maxCols = grid.GetLength(1)
    let distances = Array2D.create maxRows maxCols 0
    let mutable coords = Queue.empty<Coord>

    let addCoord (row: int) (col: int) (dis: int) =
        distances[row, col] <- dis
        coords <- (Queue.conj { Row = row; Col = col } coords)

    let isValidCoord (row: int) (col: int) =
        row >= 0 && row < maxRows && 
        col >= 0 && col < maxCols && 
        grid.[row, col] <> '#' && distances[row, col] = border

    for row in 0..maxRows - 1 do
        for col in 0..maxCols - 1 do
            distances[row, col] <- border
            if grid[row, col] = 'S' then
                addCoord row col 0

    while not (Queue.isEmpty coords) do
        let (coord, coords') = Queue.uncons coords
        coords <- coords'
        let current = distances.[coord.Row, coord.Col]
        for dirIdx in 0..numOfDirs - 1 do
            let newRow = coord.Row + rowDirs[dirIdx]
            let newCol = coord.Col + colDirs[dirIdx]
            if isValidCoord newRow newCol then
                addCoord newRow newCol (current + 1)

    // array2d to array
    let checker = steps % 2
    let flatted = distances |> Seq.cast<int> |> Seq.toArray
    flatted |> Array.filter(fun d -> d <= steps && d % 2 = checker) |> Array.length                                                                                           

let printGrid (grid: char[,]) =
    let maxRows = grid.GetLength(0)
    let maxCols = grid.GetLength(1)
    for row in 0..maxRows - 1 do
        for col in 0..maxCols - 1 do
            printf "%c" grid.[row, col]
        printfn ""

let multiplymap (grid: char[,]) (num: int) =
    let mGrid = Array2D.create (grid.GetLength(0) * num) (grid.GetLength(1) * num) '.'
    let rowLength = grid.GetLength(0)
    let colLength = grid.GetLength(1)
    let mid = num / 2
    for row in 0..grid.GetLength(0) - 1 do
        for col in 0..grid.GetLength(1) - 1 do
            for i in 0..num - 1 do
                for j in 0..num - 1 do
                    if grid.[row, col] = 'S' then
                        if i = mid && j = mid then
                            mGrid.[row + rowLength * i, col + colLength * j] <- 'S'
                        else 
                            mGrid.[row + rowLength * i, col + colLength * j] <- '.'
                    else
                        mGrid.[row + rowLength * i, col + colLength * j] <- grid.[row, col]
    mGrid

let extrapolate (target: bigint) (values: bigint[]) =
    let result = Array.zeroCreate 3
    result.[0] <- values.[0]
    result.[1] <- values.[1] - values.[0]
    result.[2] <- values.[2] - values.[1]

    result.[0] + (result.[1] * target) + (target * (target - 1I) / 2I) * (result.[2] - result.[1])

let execute =
    let path = "day21/day21_input.txt"
    let map = LocalHelper.GetLinesFromFile path |> Seq.toList |> parseInput
    let map1 = multiplymap map 1
    let res1 = calculate map1 65

    let map3 = multiplymap map 3
    let res3 = calculate map3 (65 + 131)

    let map5 = multiplymap map 5
    let res5 = calculate map5 (65 + (131 * 2))

    let tmp = [| res1 |> bigint; res3 |> bigint; res5 |> bigint |]
    let poly = extrapolate ((26501365I - 65I) / 131I) tmp
    poly