module day09_part01

open AdventOfCode_2025.Modules

let parseContent(lines: string array) =
    lines
    |> Array.map(fun line ->
        (
            bigint.Parse(line.Split(',')[0]),
            bigint.Parse(line.Split(',')[1])
        )
    )

let findMaxRectangle(redTiles: (bigint * bigint) array) =
    let mutable maxArea = 0I
    for oIdx in 1..redTiles.Length - 1 do
        for tIdx in 0..oIdx - 1 do
            let (ox, oy) = redTiles[oIdx]
            let (tx, ty) = redTiles[tIdx]
            let tmpArea = (abs (ox - tx) + 1I) * (abs (oy - ty) + 1I)
            if tmpArea > maxArea then
                maxArea <- tmpArea
    maxArea                

let execute() =
    //let path = "day09/test_input_09.txt"
    let path = "day09/day09_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let redTiles = parseContent content
    findMaxRectangle redTiles
