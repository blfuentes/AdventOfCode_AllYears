module day16_part01

open AdventOfCode_2024.Modules
open System.Collections.Generic


type Position = {
    Row: int
    Col: int
}

type Cell = {
    Visited: bool
    Distance: int
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
    let graph = Array3D.create maxrows maxcols 4 { Visited = false; Distance = System.Int32.MaxValue }
    let queue = PriorityQueue<Position * int, int>()

    let isValidCoord (map: char[,]) (position: Position) =
        position.Row >= 0 && position.Col >= 0 &&
        position.Row < maxrows && position.Col < maxcols && 
        map[position.Row,position.Col] <> '#'

    let alreadyVisited (position, dir) = 
        graph[position.Row, position.Col, dir].Visited

    let setDistance distance (position, dir) =
        graph[position.Row, position.Col, dir] <- { graph[position.Row, position.Col, dir] with Distance = distance }

    let setVisited (position, dir) =
        graph[position.Row, position.Col, dir] <- { graph[position.Row, position.Col, dir] with Visited = true }

    let rec consumePath () =
        if queue.Count = 0 then
            let lowestCost =
                [ 0..3 ]
                |> List.map (fun dir -> graph[endnode.Row, endnode.Col, dir].Distance)
                |> List.min
            (graph, lowestCost)
        else
            let (currentPoint, currentDir) = queue.Dequeue()

            if alreadyVisited (currentPoint, currentDir) then consumePath ()
            else
                setVisited (currentPoint, currentDir)

                neighbours currentPoint currentDir
                |> Seq.filter (fun ((position, dir), distance) -> 
                    isValidCoord map position && not (alreadyVisited (position, dir))
                )
                |> Seq.map (fun ((position, dir), distance) -> 
                    let d' = graph[currentPoint.Row, currentPoint.Col, currentDir].Distance
                    (position, dir), d' + distance
                )
                |> Seq.filter (fun ((position, dir), distance) -> 
                    let d' = graph[position.Row, position.Col, dir].Distance
                    distance <= d'
                )
                |> Seq.iter (fun ((position, dir), distance) ->
                    setDistance distance (position, dir)
                    queue.Enqueue((position, dir), distance)
                )

                consumePath ()

    setDistance 0 (startnode, 0)
    queue.Enqueue((startnode, 0), 0)

    consumePath ()

let execute() =
    let path = "day16/day16_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (map, startNode, endNode) = parseContent content

    let (costGraph, minimalcost) = dijkstraExplore map startNode endNode
    minimalcost