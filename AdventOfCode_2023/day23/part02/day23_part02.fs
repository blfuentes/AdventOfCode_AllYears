module day23_part02

open System.Collections.Generic

open AdventOfCode_2023.Modules

type Square = {
    Row: int
    Col: int
}

type Edge = {
    vertex: int
    distance: int
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
    let path = "day23/day23_input.txt"
    let map = LocalHelper.GetLinesFromFile path |> List.ofSeq |> parseInput
    let special = new Dictionary<Square, int>()
    let specialList = new Queue<Square>()
    let mutable counter = 0

    let addSpecial (row: int) (col: int) =
        let currentSquare = { Row = row; Col = col }
        specialList.Enqueue(currentSquare) |> ignore
        special[currentSquare] <- counter
        counter <- counter + 1
     
    let isValidPosition (row: int) (col: int) =
        0 <= row && row < map.GetLength(0) && 0 <= col && col < map.GetLength(1) && map.[row, col] <> '#'
    
    let inBorders (row: int) (col: int) =
        0 <= row && row < map.GetLength(0) && 0 <= col && col < map.GetLength(1)

    addSpecial 0 1
    addSpecial (map.GetLength(0) - 1) (map.GetLength(1) - 2)
    for row in 0..map.GetLength(0) - 1 do
        for col in 0..map.GetLength(1) - 1 do
            if map.[row, col] <> '#' then
                let mutable cc = 0
                for d in 0..numOfDirs - 1 do
                    let newRow = row + rowDirs.[d]
                    let newCol = col + colDirs.[d]
                    if inBorders newRow newCol && (directions |> Array.contains(map.[newRow, newCol])) then
                        cc <- cc + 1
                if cc > 1 then
                    addSpecial row col

    let adj = new Dictionary<int, HashSet<Edge>>()
    for idx in 0..counter - 1 do
        adj.Add(idx, new HashSet<Edge>())
        let distances = Array2D.zeroCreate (map.GetLength(0)) (map.GetLength(1))
        let queue = new Queue<Square>()
        let addDistance (row: int) (col: int) (distance: int) =
            distances.[row, col] <- distance
            queue.Enqueue({ Row = row; Col = col }) |> ignore
        
        let tmpSpecialList = specialList.ToArray()
        addDistance (tmpSpecialList.[idx].Row) (tmpSpecialList.[idx].Col) 1
        while queue.Count > 0 do
            let currentSquare = queue.Dequeue()
            let currentDistance = distances.[currentSquare.Row, currentSquare.Col]
            if special.ContainsKey(currentSquare) && special[currentSquare] <> idx then
                adj[idx].Add({ vertex = special[currentSquare]; distance = currentDistance - 1 }) |> ignore
            else
                for d in 0..numOfDirs - 1 do
                    let newRow = currentSquare.Row + rowDirs.[d]
                    let newCol = currentSquare.Col + colDirs.[d]
                    if isValidPosition newRow newCol && distances.[newRow, newCol] = 0 then
                        addDistance newRow newCol (currentDistance + 1)
    let mutable res = 0
    let used = Array.create counter false
    let rec run u d =
        if u = 1 then
            res <- System.Math.Max(res, d)
        else
            used.[u] <- true
            for e in adj.[u] do
                if not used.[e.vertex] then
                    run e.vertex (d + e.distance)
            used.[u] <- false
    
    run 0 0
    res