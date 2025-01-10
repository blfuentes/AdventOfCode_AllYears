module day21_part01

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
    
let execute =
    let path = "day21/day21_input.txt"
    let map = LocalHelper.GetLinesFromFile path |> Seq.toList |> parseInput
    let maxRows = map.GetLength(0)
    let maxCols = map.GetLength(1)
    let distances = Array2D.create maxRows maxCols 0
    let mutable coords = Queue.empty<Coord>

    let addCoord (row: int) (col: int) (dis: int) =
        distances[row, col] <- dis
        coords <- (Queue.conj { Row = row; Col = col } coords)

    let isValidCoord (row: int) (col: int) =
        row >= 0 && row < maxRows && 
        col >= 0 && col < maxCols && 
        map.[row, col] <> '#' && distances[row, col] = border

    for row in 0..maxRows - 1 do
        for col in 0..maxCols - 1 do
            distances[row, col] <- border
            if map[row, col] = 'S' then
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
    
    let steps = 64
    let flatted = distances |> Seq.cast<int> |> Seq.toArray
    flatted |> Array.filter(fun d -> d <= steps && d % 2 = 0) |> Array.length
    