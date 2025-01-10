module day04_part01

open AdventOfCode_2024.Modules
open System

let parseContent (lines: string array) =
    let map = Array2D.create lines.Length lines[0].Length ""
    for rowIdx in [0..lines.Length - 1] do
        for colIdx in [0..lines[0].Length - 1] do         
            let row = lines[rowIdx].ToCharArray()
            map[rowIdx, colIdx] <- (string)row[colIdx]
    map

let getDiagonals (matrix: string[,]) =
    let rows = Array2D.length1 matrix
    let cols = Array2D.length2 matrix

    // diagonals from left to right
    let leftToRightDiagonals =
        [ for diagonalIdx in 0 .. rows + cols - 2 do
            [ for rowIdx in 0 .. diagonalIdx do
                let colIdx = diagonalIdx - rowIdx
                if rowIdx < rows && colIdx < cols then 
                    yield matrix.[rowIdx, colIdx]
            ]
        ]

    // diagonals from right to left
    let rightToLeftDiagonals =
        [ for diagonalIdx in 0 .. rows + cols - 2 do
            [ for rowIdx in 0 .. diagonalIdx do
                let colIdx = cols - 1 - (diagonalIdx - rowIdx)
                if rowIdx < rows && colIdx >= 0 then 
                    yield matrix[rowIdx, colIdx]
            ]
        ]

    leftToRightDiagonals @ rightToLeftDiagonals

let getRow (matrix: string[,]) row = 
    let cols = Array2D.length2 matrix 
    [| for col in 0 .. cols - 1 do yield matrix[row, col] |]

let getCol (matrix: string[,]) col = 
    let rows = Array2D.length1 matrix 
    [| for row in 0 .. rows - 1 do yield matrix[row, col] |]

let countTimesOverlapped (pattern: string) (input: string) =
    let rec countFromIndex index count =
        match input.IndexOf(pattern, index, StringComparison.OrdinalIgnoreCase) with
        | -1 -> count
        | i -> countFromIndex (i + 1) (count + 1)

    countFromIndex 0 0

let countXmas(map: string [,]) =
    let maxRows = Array2D.length1 map
    let maxCols = Array2D.length2 map

    List.init maxRows (fun id -> String.concat "" (getRow map id))
    @
    List.init maxCols (fun id -> String.concat "" (getCol map id))
    @ 
    (getDiagonals map |> List.map(fun d -> String.concat "" d))
    |> List.sumBy(fun row -> 
        (countTimesOverlapped "XMAS" row) + (countTimesOverlapped "SAMX" row)
    )

let execute() =
    let path = "day04/day04_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let map = parseContent content
    countXmas map