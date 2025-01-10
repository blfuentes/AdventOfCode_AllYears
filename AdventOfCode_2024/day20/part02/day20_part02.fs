module day20_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

type KindType =
    | Empty
    | Wall
    | Cheat
    | Start
    | End

type Coord ={
    Row: int
    Col: int
    Kind : KindType
}

let parseContent(lines: string array) =
    let map = Array2D.init lines.Length lines[0].Length (fun row col -> { Row = row; Col = col; Kind = Empty })
    let (maxrows, maxcols) = (map.GetLength(0), map.GetLength(1))
    let mutable startpoint = { Row = -1; Col = -1; Kind = Start }
    let mutable endpoint = { Row = -1; Col = -1; Kind = End }
    let wallspoints =
        [for row in 0..maxrows-1 do
            let line = lines[row].ToCharArray() |> Array.map string
            for col in 0..maxcols-1 do
                let value = line[col]
                match value with
                | "S" ->
                    map[row, col] <- { map[row, col] with Kind = Empty }
                    startpoint <- { startpoint with Row = row; Col = col }
                | "E" -> 
                    map[row, col] <- { map[row, col] with Kind = Empty }
                    endpoint <- { endpoint with Row = row; Col = col }
                | "#" ->
                    map[row, col] <- { map[row, col] with Kind = Wall }
                    yield map[row, col]
                | _ -> ignore()

        ]
            
    (map, wallspoints, startpoint, endpoint)

let directions = [ (-1, 0); (1, 0); (0, 1); (0, -1) ]

let neighbous (position: Coord) =
    [for (dRow, dCol) in directions do
        let nextRow = position.Row + dRow
        let nextCol = position.Col + dCol
        yield (nextRow, nextCol)
    ]

let isInBoundaries (row: int) (col: int) (maxRows: int) (maxCols: int) =
    row >= 0 && col >= 0 && row < maxRows && col < maxCols


let findShortestPath (graph: Coord[,]) (wallcheat: Coord option) (visited: HashSet<Coord>) (touchedWalls: Dictionary<Coord, int>) (start: Coord) (goal: Coord) =
    let maxRows = graph.GetLength(0)
    let maxCols = graph.GetLength(1)

    let queue = Queue<Coord * int>()
    let startingpoint = graph[start.Row, start.Col]
    queue.Enqueue((startingpoint, 0))

    let rec bfs (counter: int) =
        if queue.Count = 0 then (None, visited, touchedWalls)
        else
            let (current, path) = queue.Dequeue()

            if current.Row = goal.Row && current.Col = goal.Col then
                let _ = visited.Add(goal)
                (Some(path), visited, touchedWalls)
            else
                if not (visited.Contains(current)) && (current.Kind.IsEmpty || (wallcheat.IsSome && current.Row = wallcheat.Value.Row && current.Col = wallcheat.Value.Col)) then
                    let _ = visited.Add(current)

                    let neighbours = neighbous current
                    for (nextRow, nextCol) in neighbours do
                        if isInBoundaries nextRow nextCol maxRows maxCols then
                            let neighbor = graph[nextRow, nextCol]
                            if not (visited.Contains(neighbor)) then
                                queue.Enqueue((neighbor, path+1))
                            if neighbor.Kind.IsWall then
                                if touchedWalls.ContainsKey(neighbor) then
                                    if touchedWalls[neighbor] > visited.Count then
                                        touchedWalls[neighbor] <- visited.Count
                                else
                                    let _ = touchedWalls.Add(neighbor, visited.Count)
                                    ignore()
                bfs (counter+1)

    bfs 0

let tryToCheat(graph: Coord[,]) (wallpoints: Coord list) (start: Coord) (goal: Coord) =
    let maxRows = graph.GetLength(0)
    let maxCols = graph.GetLength(1)
    let touchedwalls = Dictionary<Coord, int>()
    let (initialLength, visited, _) = findShortestPath graph None (HashSet<Coord>())touchedwalls start goal
    
    let distances = Dictionary<(int*int), int>()
    visited
    |> Seq.iteri(fun idx c -> distances.Add((c.Row, c.Col), initialLength.Value - idx))

    let cheatLength (cStart: Coord) (cEnd: Coord) =
        abs(cStart.Row - cEnd.Row) + abs(cStart.Col - cEnd.Col)

    let buildSpatialRange (coords: seq<Coord>) =
        let bucketSize = 20
        
        let getBucketKey (coord: Coord) = (coord.Row / bucketSize, coord.Col / bucketSize)

        let spatialHash = Dictionary<(int*int), Coord list>()
        for coord in coords do
            let key = getBucketKey coord
            if spatialHash.ContainsKey(key) then
                spatialHash[key] <- coord :: spatialHash.[key]
            else
                spatialHash[key] <- [coord]

        let result = ResizeArray<(Coord) * (Coord)>()
        for kvp in spatialHash do
            let (bucketX, bucketY), points = kvp.Key, kvp.Value
            for dx in -1..1 do
                for dy in -1..1 do
                    let neighborKey = (bucketX + dx, bucketY + dy)
                    if spatialHash.ContainsKey(neighborKey) then
                        let neighbors = spatialHash[neighborKey]
                        for p1 in points do
                            for p2 in neighbors do
                                if p1 <> p2 && cheatLength p1 p2 <= 20 then
                                    result.Add((p1, p2))

        result
    
    let combinations = buildSpatialRange visited

    let cheattimes =
        [for (startcheat, endcheat) in combinations do
            let cheatdistance = cheatLength startcheat endcheat
            if cheatdistance <= 20 then
                let startcheatdistance = distances[(startcheat.Row, startcheat.Col)]
                let endcheatdistance = distances[(endcheat.Row, endcheat.Col)]
                yield (initialLength.Value - startcheatdistance) + cheatdistance + endcheatdistance
        ]

    let groupOfSavings =
        cheattimes
        |> List.map(fun t -> initialLength.Value - t)
        |> List.filter(fun t -> t > 0)
        |> List.groupBy id
        |> List.sortBy fst
        |> List.map(fun (k, v) -> (k, v.Length))

    groupOfSavings
    |> List.filter(fun (k, v) -> k >= 100)
    |> List.sumBy snd

let execute() =
    let path = "day20/day20_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (map, wallpoints, startpoint, endpoint) = parseContent(content)
    tryToCheat map wallpoints startpoint endpoint