module day08_part01

open AdventOfCode_2024.Modules
open AdventOfCode_Utilities

type Coord = {
    Row: int;
    Col: int;
}

type Antenna = {
    Name: char;
    Position : Coord;
}

type AntennaMap = {
    MaxRow: int;
    MaxCol: int;
}

let parseContent (lines: string array) =
    let maxRows = lines.Length
    let maxCols = lines[0].Length

    let antennas = 
        [for row in [0..maxRows-1] do
            for col in [0..maxCols-1] do
                let value = lines[row][col]
                if value <> '.' then
                    yield { Name = value; Position = {Row = row; Col = col} }]
    ({ MaxRow = maxRows; MaxCol = maxCols } , antennas)

let inBoundaries (coord: Coord) (maxRows: int) (maxCols: int) =
    coord.Row >= 0 && coord.Row < maxRows &&
    coord.Col >= 0 && coord.Col < maxCols

let mirror (coordA: Coord) (coordB: Coord) =
    let mirrored1 = (2 * coordB.Row - coordA.Row, 2 * coordB.Col - coordA.Col)
    let mirrored2 = (2 * coordA.Row - coordB.Row, 2 * coordA.Col - coordB.Col)

    (
        { Row = fst mirrored1; Col = snd mirrored1 },
        { Row = fst mirrored2; Col = snd mirrored2 }
    )

let calculateAntinode (antennas: Antenna list) (mapAntennas: AntennaMap) =
    let combinations = Utilities.combination 2 antennas
    let maxRows = mapAntennas.MaxRow
    let maxCols = mapAntennas.MaxCol

    let rAntennas =
        [for comb in combinations do
            let (coordA, coordB) = comb.Item(0).Position, comb.Item(1).Position
            let (fromFirst, fromSecond) = mirror coordA coordB
            if inBoundaries fromFirst maxRows maxCols then
                yield fromFirst
            if inBoundaries fromSecond maxRows maxCols then
                yield fromSecond
    ]
    rAntennas
    

let execute() =
    let path = "day08/day08_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (mapAntennas, antennas) = parseContent content
    let groups = antennas |> List.groupBy _.Name
    groups
    |> List.collect(fun g ->
        calculateAntinode (snd g) mapAntennas
    )
    |> List.distinct
    |> List.length