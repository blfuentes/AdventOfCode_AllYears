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

let countAccessible(grid: char[,]) =
    let transformaed =
        grid
        |> Array2D.mapi (fun row col value ->
            if value = '.' then 
                9 
            else
                let neighbors = getNeighbors row col
                let accessibleNeighbors =
                    neighbors
                    |> List.filter (fun (r, c) -> grid[r, c] = '@')
                    |> List.length
                accessibleNeighbors
        )
    [
        for row in 0 .. maxRow do
            for col in 0 .. maxCol do
                if transformaed[row, col] < 4 then
                    yield (row, col, transformaed[row, col])
    ]
    |> Seq.length

let printGrid(grid: char[,]) =
    for row in 0 .. maxRow do
        for col in 0 .. maxCol do
            let char = 
                if grid[row, col] = '.' then
                    "."
                else
                    let neighbors = 
                        getNeighbors row col
                        |> List.filter (fun (r, c) -> grid[r, c] = '@')
                    neighbors.Length.ToString()

            printf "%s" char
            //printf "%c" grid[row, col]
        printfn ""

let printGrid2(grid: int[,]) =
    for row in 0 .. maxRow do
        for col in 0 .. maxCol do
            printf "%d" grid[row, col]
        printfn ""

let countRemoves(grid: char[,]) =
    let transformed =
        grid
        |> Array2D.mapi (fun row col value ->
            if value = '.' then 
                -1 
            else
                let neighbors = getNeighbors row col
                let accessibleNeighbors =
                    neighbors
                    |> List.filter (fun (r, c) -> grid[r, c] = '@')
                    |> List.length
                accessibleNeighbors
        )
    let mutable removed = true
    let paperRolls = HashSet<int*int>()
    while removed do
        let mutable found = false
        for row in 0 .. maxRow do
            for col in 0 .. maxCol do
                if transformed[row, col] < 4 && 
                    transformed[row, col] >= 0 &&
                    not(paperRolls.Contains(row, col)) then
                    found <- paperRolls.Add(row, col)
                    transformed[row, col] <- 0
                    let neighbors = getNeighbors row col
                    for (r, c) in neighbors do
                        if transformed[r, c] > 0 then
                            transformed[r, c] <- transformed[r, c] - 1
        removed <- found    
    paperRolls.Count

let execute() =
    //let path = "day04/test_input_04.txt"
    let path = "day04/day04_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let grid = parseInput content
    countRemoves grid