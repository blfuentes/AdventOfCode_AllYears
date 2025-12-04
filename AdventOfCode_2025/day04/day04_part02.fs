module day04_part02

open AdventOfCode_2025.Modules
open System.Collections.Generic

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

let countRemoves(grid: char[,]) =
    let toDelete = Queue<int*int>()
    let transformed =
        grid
        |> Array2D.mapi (fun row col value ->
            if value = '.' then 
                -1 
            else
                let accessibleNeighbors =
                    getNeighbors row col
                    |> List.filter (fun (r, c) -> grid[r, c] = '@')
                    |> List.length

                if accessibleNeighbors < 4 then
                    toDelete.Enqueue(row, col)

                accessibleNeighbors
        )

    let mutable removed = 0
    while toDelete.Count > 0 do
        let (row, col) = toDelete.Dequeue()
        if transformed[row, col] <> -1 then
            removed <- removed + 1
            transformed[row, col] <- -1
            getNeighbors row col
            |> List.iter (fun (r, c) ->
                if transformed[r, c] > 0 then
                    if transformed[r, c] = 4 then
                        toDelete.Enqueue(r, c)
                    let newCount = transformed[r, c] - 1
                    transformed[r, c] <- newCount
            )
    removed
    

let execute() =
    //let path = "day04/test_input_04.txt"
    let path = "day04/day04_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let grid = parseInput content
    countRemoves grid