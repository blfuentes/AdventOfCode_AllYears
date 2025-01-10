module day15_part02

open AdventOfCode_2024.Modules
open AdventOfCode_Utilities

type TileType =
    | Wall
    | Empty
    | Robot
    | BoxLEFT
    | BoxRIGHT
    | None

type MovType =
    | UP
    | DOWN
    | RIGHT
    | LEFT

type Tile = {
    Kind:   TileType
    Row:    int
    Col:    int
}

let parseContent(lines: string) =
    let (map', movs') = (lines.Split("\r\n\r\n")[0],lines.Split("\r\n\r\n")[1])
    let maplines = map'.Split("\r\n") |> Array.map(fun l -> l.ToCharArray())
    let (maxrows, maxcols) = (maplines.Length, maplines[0].Length)
    let map = Array2D.init maxrows (maxcols*2) (fun r c -> { Kind = Empty; Row = r; Col = c })
    let initposition =
        [for row in 0..(maxrows-1) do
            let mutable index = 0
            for col in 0..(maxcols-1) do
                let kind = 
                    match maplines[row][col] with
                    | '#' -> Wall
                    | '.' -> Empty
                    | 'O' -> BoxLEFT
                    | '@' -> Robot
                    | _ -> None
                if kind.IsRobot then
                    map[row, index] <- { map[row, index] with Kind = kind }
                elif kind.IsWall then
                    map[row, index] <- { map[row, index] with Kind = kind }
                    map[row, index+1] <- { map[row, index+1] with Kind = kind }
                elif kind.IsBoxLEFT then
                    map[row, index] <- { map[row, index] with Kind = kind }
                    map[row, index+1] <- { map[row, index+1] with Kind = BoxRIGHT }
                if kind.IsRobot then yield map[row, index]
                index <- index+2
        ] |> List.head
    
    let movements =
        movs'.Replace("\r\n", "").ToCharArray()
        |> Array.map(fun m ->
            match m with
            | '^' -> UP
            | 'v' -> DOWN
            | '>' -> RIGHT
            | '<' -> LEFT
            | _ -> failwith "error!"
        ) |> List.ofArray
    (initposition, map, movements)

let symbolOfKind kind =
    match kind with
    | Wall -> '#'
    | Robot -> '@'
    | BoxLEFT -> '['
    | BoxRIGHT -> ']'
    | Empty -> '.'
    | _ -> 'X'

let symbolOfMov mov =
    match mov with
    | UP -> '^'
    | DOWN -> 'v'
    | RIGHT -> '>'
    | LEFT -> '<'

let direction mov =
    match mov with
    | UP -> (-1, 0)
    | DOWN -> (1, 0)
    | RIGHT -> (0, 1)
    | LEFT -> (0, -1)

let take(from : Tile list, map: Tile[,], mov: MovType) (steps: int)=
    let (r, c) = direction mov
    let rec canMove (currentparents: Tile list) (parentsToCheck: Tile list) (trylevel: bool) =
        if not trylevel then []
        else
            let newparentsTocheck =
                [for t in parentsToCheck do
                    let parents = 
                        match map[t.Row+r, t.Col+c] with
                        | x when x.Kind.IsBoxRIGHT ->
                            [map[x.Row, x.Col-1]; x]
                        | x when x.Kind.IsBoxLEFT ->
                            [x; map[x.Row, x.Col+1]]
                        | x -> [x]
                    
                    yield! parents
                ]
            if newparentsTocheck |> List.forall(fun p -> p.Kind.IsEmpty) then (currentparents@parentsToCheck@newparentsTocheck)
            else
                let noWallFound = newparentsTocheck |> List.forall(fun p -> not p.Kind.IsWall)
                let filteredparents = 
                    newparentsTocheck |> List.filter(fun p -> not p.Kind.IsEmpty)
                canMove (currentparents@parentsToCheck) filteredparents noWallFound

    canMove [] from true

let findBoxes(from : Tile, map: Tile[,], mov: MovType) (step: int) =
    let (r, c) = direction mov
    let mutable cantake = true
    let mutable emptyfound = false
    let mutable current = map[from.Row, from.Col]
    let temptiles =
        [while cantake do
            current <- map[current.Row+r, current.Col+c]
            match current.Kind with
            | x when x.IsBoxRIGHT && (mov.IsUP || mov.IsDOWN) ->
                let partnerLeft = map[current.Row, current.Col-1]    
                let availableboxes = take ([partnerLeft; current], map, mov) step
                yield! availableboxes
                cantake <- false
                emptyfound <- availableboxes.Length > 0
            | x when x.IsBoxLEFT && (mov.IsUP || mov.IsDOWN) ->
                let partnerRight = map[current.Row, current.Col+1]
                let availableboxes = take ([current; partnerRight], map, mov) step
                yield! availableboxes
                cantake <- false
                emptyfound <- availableboxes.Length > 0
            | x when x.IsEmpty && (mov.IsLEFT || mov.IsRIGHT) ->
                    yield current
                    emptyfound <- true
                    cantake <- false
            | x when (x.IsBoxLEFT || x.IsBoxRIGHT) && (mov.IsLEFT || mov.IsRIGHT) ->
                yield current
            | x when x.IsWall ->
                cantake <- false
            | _ -> failwith "error"
        ] |> List.rev
    if emptyfound then temptiles else []

let printMap(map: Tile[,]) =
    let (maxrows, maxcols) = (map.GetLength(0),map.GetLength(1))
    for row in 0..(maxrows-1) do
        for col in 0..(maxcols-1) do
            printf "%c" (symbolOfKind map[row,col].Kind)
        printfn ""

let move((robotinit, map, movements): Tile*Tile [,]* MovType list) =
    let rec doStep robot movs steps =
        match movs with
        | [] -> 
            true
        | m :: restmovs ->    
            let (r, c) = direction m
            let nextPos = map[robot.Row + r, robot.Col + c]
            match nextPos.Kind with
            | k when k.IsWall -> 
                doStep robot restmovs (steps+1)

            | k when k.IsEmpty -> // just walk one step further
                map[robot.Row, robot.Col] <- { map[robot.Row, robot.Col] with Kind = Empty }
                map[nextPos.Row, nextPos.Col] <- { map[nextPos.Row, nextPos.Col] with Kind = Robot }
                doStep { robot with Row = nextPos.Row; Col = nextPos.Col } restmovs (steps+1)

            | k -> // push boxes
                let tilesToMov = findBoxes (robot,map, m) steps
                if tilesToMov.Length > 0 then
                    map[robot.Row, robot.Col] <- { map[robot.Row, robot.Col] with Kind = Empty }    
                    for t in tilesToMov do
                        let possibleReplacement = map[t.Row-r, t.Col-c]
                        let replacementKind =
                            if tilesToMov |> List.exists(fun t -> t.Row = possibleReplacement.Row && t.Col = possibleReplacement.Col) then
                                possibleReplacement.Kind
                            else
                                Empty
                        if not t.Kind.IsEmpty then
                            map[t.Row+r, t.Col+c] <- { map[t.Row+r, t.Col+c] with Kind = t.Kind }
                        map[t.Row, t.Col] <- { map[t.Row, t.Col] with Kind = replacementKind }
                    map[nextPos.Row, nextPos.Col] <- { map[nextPos.Row, nextPos.Col] with Kind = Robot }

                    doStep { robot with Row = nextPos.Row; Col = nextPos.Col } restmovs (steps+1)
                else
                    doStep robot restmovs (steps+1)

    doStep robotinit movements 1

let calculateGPS(tile: Tile) =
    if tile.Kind.IsBoxLEFT then
        100 * tile.Row + tile.Col
    else
        0

let execute() =
    let path = "day15/day15_input.txt"
    let content = LocalHelper.GetContentFromFile path
    let (robotinit, map, movements) = parseContent content
    let _ = move (robotinit, map, movements)
    Utilities.flattenArray2D map
    |> Array.sumBy calculateGPS