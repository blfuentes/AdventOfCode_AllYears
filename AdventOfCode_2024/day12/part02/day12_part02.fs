module day12_part02

open AdventOfCode_2024.Modules

type Movement =
    | Up
    | Down
    | Right
    | Left

let nextPosition movDir (row, col) =
    match movDir with
    | Up -> (row - 1, col)
    | Down -> (row + 1, col)
    | Right -> (row, col + 1)
    | Left -> (row, col - 1)

let parseContent (lines: string array) =
    let maxRows = lines.Length
    let maxCols = lines[0].Length
    let map = Array2D.create maxRows maxCols '.'

    for row in 0..maxRows - 1 do
        for col in 0..maxCols - 1 do
            map[row, col] <- lines[row][col]

    map
    
let isInBoundaries (row: int) (col: int) (maxRows: int) (maxCols: int) =
    row >= 0 && row < maxRows && col >= 0 && col < maxCols

let neighbours row col = 
    [ (row - 1, col)
      (row + 1, col)
      (row, col - 1)
      (row, col + 1)]

let buildRegions (garden: char[,]) (maxRows, maxCols) =
    let visited = Array2D.create maxRows maxCols false
    let mutable regions = []

    let rec floodFillArea row col element (currentArea: (int*int) list) =
        if not (isInBoundaries row col maxRows maxCols) then currentArea
        elif visited[row, col] || garden[row, col] <> element then currentArea
        else
            visited[row, col] <- true
            let newPoints = (row, col) :: currentArea
            let mutable points = newPoints
            neighbours row col
            |> List.iter(fun (rowIdx, colIdx)->
                points <- floodFillArea rowIdx colIdx element points
            )
            points

    for row in 0..maxRows - 1 do
        for col in 0..maxCols - 1 do
            if not visited[row, col] then
                let points = floodFillArea row col garden[row, col] []
                if not points.IsEmpty then
                    regions <- (points.Length, points) :: regions

    regions

let rec growDimension (region: Set<int*int>) movDir (visitedSoFar: Set<int*int>) (row, col) =
    let nextPoint =
        match movDir with
        | Up | Down -> nextPosition Right (row, col)
        | Left | Right -> nextPosition Down (row, col)
    
    if Set.contains nextPoint region && 
        not (Set.contains (nextPosition movDir nextPoint) region)then
        growDimension region movDir (Set.add (row, col) visitedSoFar) nextPoint
    else
        Set.add (row, col) visitedSoFar

let rec consumeRegion movDir (region: Set<int*int>) (regionPoints: (int*int) list) (visited: Set<int*int>) numOfSides =
    match regionPoints with
    | [] -> numOfSides
    | currentPoint :: tailPoints ->
        if Set.contains currentPoint visited then
            consumeRegion movDir region tailPoints visited numOfSides
        elif Set.contains (nextPosition movDir currentPoint) region then
            consumeRegion movDir region tailPoints (Set.add currentPoint visited) numOfSides
        else
            let newVisited = growDimension region movDir visited currentPoint
            consumeRegion movDir region tailPoints newVisited (numOfSides + 1)

let exploreRegion (region: (int*int) list) =
    [Up; Right; Down; Left]
    |> Seq.sumBy (fun movDir -> 
        let r' = region |> Set.ofList
        let r'' = Set.toList r'
        consumeRegion movDir r' r'' Set.empty 0
    )

let execute() =
    let path = "day12/day12_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let garden = parseContent content
    let (maxRows, maxCols) = (garden.GetLength(0), garden.GetLength(1))
    let regions = buildRegions garden (maxRows,maxCols)
    regions
    |> List.sumBy(fun (size, points) ->
        size * exploreRegion points
    )