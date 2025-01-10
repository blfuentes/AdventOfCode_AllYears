module day12_part01

open AdventOfCode_2024.Modules

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
                if points <> [] then
                    regions <- (List.length points, points) :: regions

    regions

let calculatePerimeter (points: (int * int) list) (maxRows, maxCols) =
    let pointSet = Set.ofList points
    let mutable perimeter = 0

    for (row, col) in points do
        neighbours row col
        |> List.iter(fun (closerow, closecol) ->
            if not(isInBoundaries closerow closecol maxRows maxCols) || 
                not (Set.contains (closerow, closecol) pointSet) then
                    perimeter <- perimeter + 1
        )

    perimeter


let execute() =
    let path = "day12/day12_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let map = parseContent content
    let (maxRows, maxCols) = (map.GetLength(0), map.GetLength(1))
    let regions = buildRegions map (maxRows,maxCols)
    regions
    |> List.sumBy(fun (size, points) ->
        size * calculatePerimeter points (maxRows,maxCols)
    )