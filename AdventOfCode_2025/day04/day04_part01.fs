module day04_part01

open AdventOfCode_2025.Modules

let mutable maxRow = 0
let mutable maxCol = 0

let parseInput (input: string array) =
    maxRow <- input.Length - 1
    maxCol <- input[0].Length - 1
    let grid =
        Array2D.init (input.Length) (input[0].Length) (fun row col -> input[row][col])
    grid

let getNeighbors (row: int) (col: int) =
    [ 
        (row - 1, col); 
        (row + 1, col); 
        (row, col - 1); 
        (row, col + 1);
        (row-1, col - 1);
        (row-1, col + 1);
        (row+1, col - 1);
        (row+1, col + 1);
    ]
    |> List.filter (fun (r, c) -> r >= 0 && r <= maxRow && c >= 0 && c <= maxCol)

let countAccessible(grid: char[,]) =
    seq {
        for row in 0 .. Array2D.length1 grid - 1 do
            for col in 0 .. Array2D.length2 grid - 1 do
                if grid[row, col] = '@' then
                    let accessibleNeighbors =
                        getNeighbors row col
                        |> List.filter (fun (r, c) -> grid[r, c] = '@')
                        |> List.length
                    if accessibleNeighbors < 4 then
                        yield 1
    }
    |> Seq.sum

let execute() =
    //let path = "day04/test_input_04.txt"
    let path = "day04/day04_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let grid = parseInput content
    countAccessible grid