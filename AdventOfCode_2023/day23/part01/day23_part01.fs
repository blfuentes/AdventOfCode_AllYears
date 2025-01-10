module day23_part01

open System.Collections.Generic

open AdventOfCode_2023.Modules

type Coord = {
    Row: int
    Col: int
    Dir: int
}

let numOfDirs = 4
let rowDirs = [| -1; 0; 1; 0 |]
let colDirs = [| 0; 1; 0; -1 |]
let directions = [| '^'; '>'; 'v'; '<' |]


let parseInput (lines: string list) =
    let map = Array2D.create (lines.Length) (lines.Head.Length) '.'
    for row in 0..lines.Length - 1 do
        for col in 0..lines.Head.Length - 1 do
            map.[row, col] <- lines.[row].[col]
    map

let execute =
    //let path = "day23/test_input_01.txt"
    let path = "day23/day23_input.txt"
    let map = LocalHelper.GetLinesFromFile path |> List.ofSeq |> parseInput
    let queue = new Queue<Coord>()
    let distances = new Dictionary<(int*int*int), int>()

    let addDistance (row: int) (col: int) (dir: int) (distance: int) =
        let key = (row, col, dir)
        if not (distances.ContainsKey(key)) then
            distances.Add(key, distance) |> ignore
        else
            distances[key] <- distance
        queue.Enqueue({ Row = row; Col = col; Dir = dir }) |> ignore
     
    let isValidPosition (row: int) (col: int) =
        0 <= row && row < map.GetLength(0) && 0 <= col && col < map.GetLength(1) && map.[row, col] <> '#'
    
    let isOpposite (dir1: int) (dir2: int) =
        (dir1 ^^^ dir2) = 2

    addDistance 0 1 2 0
    while queue.Count > 0 do
        let coord = queue.Dequeue()
        let currentDistance = distances[(coord.Row, coord.Col, coord.Dir)]
        let el = map.[coord.Row, coord.Col]
        let dirIdx = 
            match Array.tryFindIndex (fun c -> c = el) directions with
            | Some i -> i
            | None -> -1
        
        for d in 0..numOfDirs - 1 do
            if not ((isOpposite d coord.Dir) || (dirIdx >= 0 && d <> dirIdx)) then
                let newRow = coord.Row + rowDirs.[d]
                let newCol = coord.Col + colDirs.[d]
                if isValidPosition newRow newCol then
                    addDistance newRow newCol d (currentDistance + 1)
    let longestDistance = 
        distances 
        |> Seq.filter (fun keyvalue ->  
            let (row, col, _) = keyvalue.Key
            row = map.GetLength(0) - 1 && col = map.GetLength(1) - 2)
        |> Seq.map (fun keyvalue -> keyvalue.Value)
        |> Seq.max
    longestDistance