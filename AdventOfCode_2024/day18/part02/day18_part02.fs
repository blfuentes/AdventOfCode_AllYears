module day18_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

type KindType =
    | Empty
    | Corrupted

type Coord ={
    X: int
    Y: int
    Kind : KindType
}

let parseContent(lines: string array) (size: int) =
    let map = Array2D.init (size+1) (size+1) (fun row col -> { X = col; Y = row; Kind = Empty })
    let corrupted = 
        lines
        |> Array.map(fun l ->
            { X = (int)(l.Split(",")[0]); Y = (int)(l.Split(",")[1]); Kind = Corrupted }
        ) |> List.ofArray
            
    (map, corrupted)

let printMap(graph: Coord[,]) =
    let maxRows = graph.GetLength(0)
    let maxCols = graph.GetLength(1)
    for row in 0..(maxRows-1) do
        for col in 0..(maxCols-1) do
            printf "%s" (if graph[row,col].Kind.IsCorrupted then "#" else ".")
        printfn ""

let findShortestPath (graph: Coord[,]) (start: Coord) (goal: Coord) =
    let maxRows = graph.GetLength(0)
    let maxCols = graph.GetLength(1)

    let isInBoundaries (row: int) (col: int) =
        row >= 0 && col >= 0 && row < maxRows && col < maxCols

    let directions = [ (-1, 0); (1, 0); (0, 1); (0, -1) ]

    let queue = Queue<Coord * int>()
    let visited = HashSet<Coord>()

    queue.Enqueue((start, 0))

    let rec bfs () =
        if queue.Count = 0 then None
        else
            let (current, path) = queue.Dequeue()

            if current = goal then
                Some(path)
            else
                if not (visited.Contains(current)) && current.Kind.IsEmpty then
                    let _ = visited.Add(current)

                    for (dRow, dCol) in directions do
                        let nextRow = current.Y + dRow
                        let nextCol = current.X + dCol
                        if isInBoundaries nextRow nextCol then
                            let neighbor = graph[nextRow, nextCol]
                            if not (visited.Contains(neighbor)) then
                                queue.Enqueue((neighbor, path+1))
                bfs ()

    bfs ()

let findCorrupted(map: Coord[,]) (corrupted: Coord list) (start: Coord) (goal: Coord) =
    let rec walk (remainingCorrupted: Coord list) =
        match remainingCorrupted with
        | c :: tail ->
            map[c.Y, c.X] <- { map[c.X, c.Y] with Kind = Corrupted }
            let canFinish = findShortestPath map start goal
            match canFinish with
            | Some(p) -> walk tail
            | None -> c
        | _ -> failwith "error!"
    walk corrupted

let execute() =
    let path = "day18/day18_input.txt"
    let (size, numOfBytes) = (70, 1024)

    let content = LocalHelper.GetLinesFromFile path
    let (map, corrupted) = parseContent content size
    let start = { X = 0; Y = 0; Kind = Empty }
    let endnode = { X = size; Y = size; Kind = Empty }
    let found = findCorrupted map corrupted start endnode
    sprintf "%d,%d" found.X found.Y