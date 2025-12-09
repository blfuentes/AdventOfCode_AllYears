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

let rectangleIntersectsEdge (rectX, rectY, rectU, rectV) ((p1, q1), (r1, s1)) =
    let p, r = sortPair p1 r1
    let q, s = sortPair q1 s1
    rectX < r && rectU > p && rectY < s && rectV > q

let findMaxRectangle (redTiles: (bigint * bigint) array) =
    let edges = 
        [| yield! Array.pairwise redTiles
           yield (redTiles[redTiles.Length - 1], redTiles[0]) |]
    
    let checkRectangle (x1, y1) (x2, y2) =
        let x1', y1' = sortPair x1 x2
        let x2', y2' = sortPair y1 y2
        let size = (y1' - x1' + 1I) * (y2' - x2' + 1I)
        
        let intersects = 
            edges 
            |> Array.exists (rectangleIntersectsEdge (x1', x2', y1', y2'))
        
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
