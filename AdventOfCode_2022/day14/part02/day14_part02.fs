module day14_part02


open AdventOfCode_2022.Modules

let path = "day14/day14_input.txt"

let getDirection (from: int []) (des: int []) =
    let row =
        if from.[0] > des.[0] then -1
        elif from.[0] < des.[0] then 1
        else 0
    let col =
        if from.[1] > des.[1] then -1
        elif from.[1] < des.[1] then 1
        else 0
    [|row; col|]

let generateCave (rocks: int [] [] []) =
    let cave = 
        seq {
            for lineIdx in 0..rocks.Length - 1 do
                let listOfRocks = rocks.[lineIdx] |> Array.pairwise
                for (firstRock, secondRock) in listOfRocks do
                    let direction = getDirection firstRock secondRock
                    let initRow = if direction.[0] >= 0 then firstRock.[0] else secondRock.[0]
                    let endRow = if direction.[0] >= 0 then secondRock.[0] else firstRock.[0]
                    let initCol = if direction.[1] >= 0 then firstRock.[1] else secondRock.[1]
                    let endCol = if direction.[1] >= 0 then secondRock.[1] else firstRock.[1]
                    for rowIdx in initRow..endRow do
                        for colIdx in initCol..endCol do
                            yield [|rowIdx; colIdx|]
        } |> List.ofSeq
    cave


let rec dropSand (dPoint: int[]) (cave: string array2d) =
    let newPoint = [|dPoint.[0]; dPoint.[1]|]
    // check down
    newPoint.[0] <- dPoint.[0] + 1
    if cave[newPoint.[0], newPoint.[1]] = "." then
        dropSand newPoint cave
    else
        // check left
        newPoint.[0] <- dPoint.[0] + 1
        newPoint.[1] <- dPoint.[1] - 1
        if cave[newPoint.[0], newPoint.[1]] = "." then
            dropSand newPoint cave
        else
            // check right
            newPoint.[0] <- dPoint.[0] + 1
            newPoint.[1] <- dPoint.[1] + 1
            if cave[newPoint.[0], newPoint.[1]] = "." then
                dropSand newPoint cave
            else
                cave[dPoint.[0], dPoint.[1]] <- "o"

let rec doPouring (counter: int) (pPoint: int[]) (cave: string array2d) =
    dropSand pPoint cave
    if cave[0, 500] = "o" then
        counter
    else
        doPouring (counter + 1) pPoint cave

let execute =
    let inputLines = LocalHelper.GetLinesFromFile(path)
    let parts = inputLines |> Array.map(fun l -> l.Split("->"))
                    |> Array.map(fun r -> r |> Array.map(fun sb -> [|int(sb.Split(",")[1]); int(sb.Split(",")[0])|]))
    let stones = generateCave parts
    let maxRow = (stones |> List.sortByDescending(fun r -> r.[0]) |> List.head).[0]
    let newMaxRow = maxRow + 2
    let pouringPoint = [|0; 500|]
    let coordsCave = Array2D.create (newMaxRow + 1) (1000) "."
    for r in stones do
        coordsCave[r.[0], r.[1]] <- "#"
    for col in 0..coordsCave.GetUpperBound(1) do
        coordsCave[newMaxRow, col] <- "#"
    coordsCave[0, 500] <- "+"
    doPouring 1 pouringPoint coordsCave 