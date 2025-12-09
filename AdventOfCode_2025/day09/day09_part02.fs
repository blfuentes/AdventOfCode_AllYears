module day09_part02

open AdventOfCode_2025.Modules
open System.Collections.Generic

let parseContent(lines: string array) =
    lines
    |> Array.map(fun line ->
        (
            bigint.Parse(line.Split(',')[0]),
            bigint.Parse(line.Split(',')[1])
        )
    )

let inline sortPair x y = if x <= y then (x, y) else (y, x)

let rectangleIntersectsEdge (minX, minY, maxX, maxY) ((x1, y1), (x2, y2)) =
    let sMinX, sMaxX = sortPair x1 x2
    let sMinY, sMaxY = sortPair y1 y2
    minX < sMaxX && maxX > sMinX && minY < sMaxY && maxY > sMinY

let findMaxRectangle (redTiles: (bigint * bigint) array) =
    let edges = 
        [| yield! Array.pairwise redTiles
           yield (redTiles[redTiles.Length - 1], redTiles[0]) |]
    
    let checkRectangle (x1, y1) (x2, y2) =
        let minX, maxX = sortPair x1 x2
        let minY, maxY = sortPair y1 y2
        let size = (maxX - minX + 1I) * (maxY - minY + 1I)
        
        let intersects = 
            edges 
            |> Array.exists (rectangleIntersectsEdge (minX, minY, maxX, maxY))
        
        if intersects then 0I else size
    
    seq {
        for i in 0 .. redTiles.Length - 2 do
            for j in i + 1 .. redTiles.Length - 1 do
                yield checkRectangle redTiles[i] redTiles[j]
    }
    |> Seq.max

let execute() =
    //let path = "day09/test_input_09.txt"
    let path = "day09/day09_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let redTiles = parseContent content
    findMaxRectangle redTiles
