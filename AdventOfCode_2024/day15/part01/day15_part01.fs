module day15_part01

open AdventOfCode_2024.Modules
open AdventOfCode_Utilities

type TileType =
    | Wall
    | Empty
    | Robot
    | Box
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
    let map = Array2D.init maxrows maxcols (fun r c -> { Kind = None; Row = r; Col = c })
    let initposition =
        [for row in 0..(maxrows-1) do
            for col in 0..(maxcols-1) do
                let kind = 
                    match maplines[row][col] with
                    | '#' -> Wall
                    | '.' -> Empty
                    | 'O' -> Box
                    | '@' -> Robot
                    | _ -> None
                map[row, col] <- { map[row, col] with Kind = kind }
                if kind.IsRobot then yield map[row, col]
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
    | Box -> 'O'
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

let findBoxes(from : Tile, map: Tile[,], mov: MovType) =
    let (r, c) = direction mov
    let mutable cantake = true
    let mutable emptyfound = false
    let mutable current = map[from.Row, from.Col]
    let temptiles =
        [while cantake do
            current <- map[current.Row+r, current.Col+c]
            match current.Kind with
            | x when x.IsEmpty ->
                yield current
                emptyfound <- true
                cantake <- false
            | x when x.IsBox ->
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
    let rec doStep robot movs =
        match movs with
        | [] -> 
            true
        | m :: restmovs ->    
            let (r, c) = direction m
            let nextPos = map[robot.Row + r, robot.Col + c]
            match nextPos.Kind with
            | k when k.IsWall -> 
                doStep robot restmovs

            | k when k.IsEmpty -> // just walk one step further
                map[robot.Row, robot.Col] <- { map[robot.Row, robot.Col] with Kind = Empty }
                map[nextPos.Row, nextPos.Col] <- { map[nextPos.Row, nextPos.Col] with Kind = Robot }
                doStep { robot with Row = nextPos.Row; Col = nextPos.Col } restmovs

            | k -> // push boxes
                let tilesToMov = findBoxes (robot,map, m)
                if tilesToMov.Length > 0 then
                    for tIdx in 0..(tilesToMov.Length-2) do
                        let t' = tilesToMov.Item(tIdx)
                        map[t'.Row, t'.Col] <- { map[t'.Row, t'.Col] with Kind = Box }
                    map[robot.Row, robot.Col] <- { map[robot.Row, robot.Col] with Kind = Empty }
                    map[nextPos.Row, nextPos.Col] <- { map[nextPos.Row, nextPos.Col] with Kind = Robot }
                    doStep { robot with Row = nextPos.Row; Col = nextPos.Col } restmovs
                else
                    doStep robot restmovs

    doStep robotinit movements

let calculateGPS(tile: Tile) =
    if tile.Kind.IsBox then
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
