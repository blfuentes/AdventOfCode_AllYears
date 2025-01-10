module day18_part01

open AdventOfCode_2024.Modules
open System.Collections.Generic

type KindType =
    | Empty
    | Corrupted

type Coord = {
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
        )
            
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

let buildCorruptedMap(map: Coord[,])(corrupted: Coord[])(numOfBytes: int) =
    corrupted
    |> Array.take(numOfBytes)
    |> Array.iter(fun c ->
        map[c.Y, c.X] <- { map[c.X, c.Y] with Kind = Corrupted }
    )

let execute() =
    let path = "day18/day18_input.txt"
    let (size, numOfBytes) = (70, 1024)

    let content = LocalHelper.GetLinesFromFile path

    let (map, corrupted) = parseContent content size
    let start = { Y = 0; X = 0; Kind = Empty }
    let endnode = { Y = size; X = size; Kind = Empty }
    buildCorruptedMap map corrupted numOfBytes
    let path = findShortestPath map start endnode
    path.Value