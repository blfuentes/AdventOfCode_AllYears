module day08_part02

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
    ({ MaxRow = maxRows; MaxCol = maxCols }, antennas)

let inBoundaries (coord: Coord) (maxRows: int) (maxCols: int) =
    coord.Row >= 0 && coord.Row < maxRows &&
    coord.Col >= 0 && coord.Col < maxCols

let mirroredForRep (coordA: Coord) (coordB: Coord) rep =
    let rowDistance = coordB.Row - coordA.Row
    let colDistance = coordB.Col - coordA.Col

    let mirrored1 = { Row = coordB.Row + rep * rowDistance; Col = coordB.Col + rep * colDistance }
    let mirrored2 = { Row = coordA.Row - rep * rowDistance; Col = coordA.Col - rep * colDistance }
    (mirrored1, mirrored2)

let calculateAntinode (antennas: Antenna list) (mapAntenna: AntennaMap) =
    let combinations = Utilities.combination 2 antennas
    let maxRows, maxCols = mapAntenna.MaxRow, mapAntenna.MaxCol
    
    // Add antennas for the specific radius until the radius is out of range
    let rec generatePositions coordA coordB currentAntennas antennaRadius fOutOfRange sOutOfRange =
        if fOutOfRange && sOutOfRange then
            currentAntennas
        else
            let mirrorA, mirrorB = mirroredForRep coordA coordB antennaRadius
            let (newAntennas, newfOutOfRange) = 
                if not fOutOfRange && inBoundaries mirrorA maxRows maxCols then
                    (mirrorA :: currentAntennas, false)
                else
                    (currentAntennas, true)
            
            let (newAntenas, newsOutOfRange) = 
                if not sOutOfRange && inBoundaries mirrorB maxRows maxCols then
                    (mirrorB :: newAntennas, false)
                else
                    (newAntennas, true)
            
            generatePositions coordA coordB newAntenas (antennaRadius + 1) newfOutOfRange newsOutOfRange

    combinations
    |> List.collect (fun comb ->
        // Add the initial antennas
        let initialPositions = [comb.Item(0).Position; comb.Item(1).Position]

        // Call initial case                    
        generatePositions (comb.Item(0).Position) (comb.Item(1).Position) initialPositions 1 false false
    )


let execute() =
    let path = "day08/day08_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (mapAntenna, antennas) = parseContent content
    let groups = antennas |> List.groupBy _.Name
    groups
    |> List.collect(fun g ->
        calculateAntinode (snd g) mapAntenna
    )
    |> List.distinct
    |> List.length