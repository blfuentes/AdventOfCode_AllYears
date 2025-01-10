module day19_part02

open AdventOfCode_2017.Modules

type PathKind =
    | Vertical
    | Horizontal
    | Cross
    | Empty

type Orientation =
    | North
    | South
    | West
    | East

type Cell = {
    Row: int
    Col: int
    Kind: PathKind
    Value: char option
}

let IsPathKind (cell: Cell) (map: Cell[,]) =
    let (up, down, left, right) = (map[cell.Row-1, cell.Col], map[cell.Row+1, cell.Col], map[cell.Row, cell.Col-1], map[cell.Row, cell.Col+1])
    match (up, down, left, right) with
    | (u, d, l, r) when u.Kind.IsVertical || d.Kind.IsVertical -> Vertical
    | (u, d, l, r) when l.Kind.IsHorizontal || r.Kind.IsHorizontal -> Horizontal
    | (u, d, l, r) when u.Kind.IsCross || d.Kind.IsCross -> Vertical
    | (u, d, l, r) when l.Kind.IsCross || r.Kind.IsCross -> Horizontal
    | (u, d, l, r) when l.Kind.IsVertical && r.Kind.IsVertical -> Horizontal
    | _ -> failwith "error"

let parseContent(lines: string array) =
    let (maxrows, maxcols) = (lines.Length, lines.[0].Length)
    let grid = Array2D.init maxrows maxcols (fun r c -> { Row = r; Col = c; Kind = PathKind.Empty; Value = None })

    let c' =
        [for row in 0..maxrows-1 do
            for col in 0..maxcols-1 do
                if lines[row][col] = '|' then
                    grid[row, col] <- { grid[row, col] with Kind = PathKind.Vertical }
                elif lines[row][col] = '-' then
                    grid[row, col] <- { grid[row, col] with Kind = PathKind.Horizontal }
                elif lines[row][col] = '+' then
                    grid[row, col] <- { grid[row, col] with Kind = PathKind.Cross }
                elif lines[row][col] <> ' ' then
                    grid[row, col] <- { grid[row, col] with Value = Some(lines[row][col]) }
                    yield grid[row, col]
        ]
    let checkpoints =
        c'
        |> List.map(fun c ->
            { c with Kind = IsPathKind c grid}
        )
    checkpoints
    |> List.iter(fun c ->
        grid[c.Row, c.Col] <- { grid[c.Row, c.Col] with Kind = c.Kind }
    )
    (grid, checkpoints)

let turn(currentDir: Orientation) (cell: Cell) (map: Cell[,]) =
    let (up, down, left, right) = (map[cell.Row-1, cell.Col], map[cell.Row+1, cell.Col], map[cell.Row, cell.Col-1], map[cell.Row, cell.Col+1])
    let newdir =
        match (currentDir, up, down, left, right) with
        | (cd, u, d, l, r) when (cd.IsNorth || cd.IsSouth) && l.Kind.IsHorizontal -> West
        | (cd, u, d, l, r) when (cd.IsNorth || cd.IsSouth) && r.Kind.IsHorizontal -> East
        | (cd, u, d, l, r) when (cd.IsWest || cd.IsEast) && u.Kind.IsVertical -> North
        | (cd, u, d, l, r) when (cd.IsWest || cd.IsEast) && d.Kind.IsVertical -> South
        | _ -> failwith "error turning"
    newdir

let walk(map: Cell [,]) =
    let (maxrows, maxcols) = (map.GetLength(0), map.GetLength(1))

    let startposition =
        [for col in 0..maxcols-1 do
            if map[0, col].Kind.IsVertical then
                yield map[0, col]
        ] |> List.head

    let dirdiff (o: Orientation) =
        match o with
        | North -> (-1, 0)
        | South -> (1,0)
        | West -> (0, -1)
        | East -> (0, 1)

    let rec move (currentdir: Orientation) ((dr, dc): int*int) (pos: Cell) (counter: int)=
        let newpos = map[pos.Row + dr, pos.Col + dc]
        if newpos.Kind.IsEmpty then
            counter
        else
            match newpos.Kind with
            | c when c.IsHorizontal || c.IsVertical ->
                if newpos.Value.IsSome then
                    move currentdir (dr, dc) newpos (counter + 1)
                else
                    move currentdir (dr, dc) newpos (counter + 1)
            | Cross ->
                let newdir = turn currentdir newpos map
                let newdiff = dirdiff newdir
                move newdir newdiff newpos (counter + 1)
            | _ -> failwith "error"

    move South (1, 0) startposition 1

let execute() =
    let path = "day19/day19_input.txt"
    let content = (LocalHelper.GetContentFromFile path).Split("\r\n")
    let (map, checkpoints) = parseContent content

    walk map