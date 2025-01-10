module day04_part02

open AdventOfCode_2024.Modules

let parseContent (lines: string array) =
    let map = Array2D.create lines.Length lines[0].Length ""
    for rowIdx in [0..lines.Length - 1] do
        for colIdx in [0..lines[0].Length - 1] do         
            let row = lines[rowIdx].ToCharArray()
            map[rowIdx, colIdx] <- (string)row[colIdx]
    map

let buildSubArray (matrix: string[,]) (size: int)=
    let rows = Array2D.length1 matrix
    let cols = Array2D.length2 matrix

    [ for i in 0 .. rows - size do
        for j in 0 .. cols - size do
            yield
                Array2D.init size size (fun x y -> matrix[i + x, j + y]) ]

let hasCrossMAS (map: string [,]) =
    let middle = map[1,1] = "A"
    let lefttop = (map[0,0] = "M" && map[2,2] = "S") || (map[0,0] = "S" && map[2,2] = "M")
    let righttop = (map[0,2] = "M" && map[2,0] = "S") || (map[0,2] = "S" && map[2,0] = "M")

    if middle && lefttop && righttop then 1 else 0

let countXmas(map: string [,]) =
    buildSubArray map 3
    |> List.sumBy(fun m -> hasCrossMAS m)

let execute() =
    let path = "day04/day04_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let map = parseContent content
    countXmas map