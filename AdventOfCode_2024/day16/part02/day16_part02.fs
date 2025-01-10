module day16_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic


type Position = {
    Row: int
    Col: int
}

type Cell = {
    Visited: bool
    Distance: int
    Paths: Position list list
}

let parseContent(lines: string array) =
    let maxrows = lines.Length
    let maxcols = lines[0].Length
    let map = Array2D.create maxrows maxcols '.'
    let mutable Start = { Row = -1; Col = -1 }
    let mutable End = { Row = -1; Col = -1 }

    for row in 0..(maxrows-1) do
        let line = lines[row].ToCharArray()        
        for col in 0..(maxcols-1) do
            let value = line[col]
            map[row, col] <- value
            if value = 'S' then
                Start <- { Start with Row = row; Col = col }
            elif value = 'E' then
                End <- { End with Row = row; Col = col; }

    (map, Start, End)

let neighbours (position: Position) dir =
    let neighbor =
        match dir with
        | 0 -> ({ Row = position.Row; Col = position.Col + 1 }, dir), 1 // EAST
        | 1 -> ({ Row = position.Row + 1; Col = position.Col }, dir), 1 // SOUTH
        | 2 -> ({ Row = position.Row; Col = position.Col - 1}, dir), 1 // WEST
        | 3 -> ({ Row = position.Row - 1; Col = position.Col }, dir), 1 // NORTH
        | _ -> failwith "error"
    let clockwisecell = ({ Row = position.Row; Col = position.Col }, (dir + 1) % 4), 1000
    let counterwisecell = ({ Row = position.Row; Col = position.Col }, (dir + 3) % 4), 1000
    [ neighbor; clockwisecell; counterwisecell ]

let dijkstraExplore (map: char[,]) startnode endnode =
    let (maxrows, maxcols) = (map.GetLength(0), map.GetLength(1))
    let graph = Array3D.create maxrows maxcols 4 { Visited = false; Distance = System.Int32.MaxValue; Paths = [] }
    let queue = PriorityQueue<Position * int, int>()

    let isValidCoord (map: char[,]) (position: Position) =
        position.Row >= 0 && position.Col >= 0 &&
        position.Row < maxrows && position.Col < maxcols && 
        map[position.Row,position.Col] <> '#'

    let alreadyVisited (position, dir) = 
        graph[position.Row, position.Col, dir].Visited

    let setVisited (position, dir) =
        graph[position.Row, position.Col, dir] <- { graph[position.Row, position.Col, dir] with Visited = true }

    let updateDistanceAndPaths distance paths (position, dir) =
            let currentNode = graph[position.Row, position.Col, dir]
            let newPaths = 
                if distance < currentNode.Distance then
                    paths
                elif distance = currentNode.Distance then
                    paths @ currentNode.Paths
                else
                    []

            graph[position.Row, position.Col, dir] <- 
                { currentNode with Distance = distance; Paths = newPaths }

    let rec consumePath () =
        if queue.Count = 0 then
            let lowestCost =
                [ 0..3 ]
                |> List.map (fun dir -> graph[endnode.Row, endnode.Col, dir].Distance)
                |> List.min
            
            let allpaths =
                [ 0..3 ]
                |> List.collect (fun dir ->
                    if graph[endnode.Row, endnode.Col, dir].Distance = lowestCost then
                        graph[endnode.Row, endnode.Col, dir].Paths
                    else
                        []
                ) |> List.map(fun p ->
                    p |> List.distinctBy id
                )

            let uniquePaths =
                List.concat allpaths
                |> List.distinctBy id

            (graph, lowestCost, uniquePaths)
        else
            let (currentPoint, currentDir) = queue.Dequeue()

            if alreadyVisited (currentPoint, currentDir) then consumePath ()
            else
                setVisited (currentPoint, currentDir)

                neighbours currentPoint currentDir
                |> Seq.filter (fun ((position, dir), _) -> 
                    isValidCoord map position && not (alreadyVisited (position, dir))
                )
                |> Seq.map (fun ((position, dir), distance) -> 
                    let d' = graph[currentPoint.Row, currentPoint.Col, currentDir].Distance
                    let paths = 
                        graph[currentPoint.Row, currentPoint.Col, currentDir].Paths
                        |> List.map (fun path -> path @ [position])
                    (position, dir), d' + distance, paths
                )
                |> Seq.filter (fun ((position, dir), distance, paths) -> 
                    distance <= graph[position.Row, position.Col, dir].Distance && not paths.IsEmpty
                )
                |> Seq.iter (fun ((position, dir), distance, paths) ->
                    updateDistanceAndPaths distance paths (position, dir)
                    queue.Enqueue((position, dir), distance)
                )

                consumePath ()

    let initialPath = [[startnode]]
    updateDistanceAndPaths 0 initialPath (startnode, 0)
    queue.Enqueue((startnode, 0), 0)

    consumePath ()

let execute() =
    let path = "day16/day16_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (map, startNode, endNode) = parseContent content

    let (_, _, uniquepaths) = dijkstraExplore map startNode endNode
    uniquepaths.Length
