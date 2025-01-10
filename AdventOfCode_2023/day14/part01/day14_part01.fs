module day14_part01

open AdventOfCode_2023.Modules

type FloorType = Rounded | Damaged | Empty

type Tile = { FloorType:FloorType; X:int; Y:int; InitialLoad: int }

let charToTile (c: char) (row: int) (col: int) (length: int)=
    let tTile =
        match c with
        | 'O' -> Rounded
        | '#' -> Damaged
        | _ -> Empty
    { FloorType = tTile; X = row; Y = col; InitialLoad = length - row }

let tileToChar (t:Tile) =
    match t.FloorType with
    | Rounded -> 'O'
    | Damaged -> '#'
    | Empty -> '.'

let parseGroup (lines:string list) =
    let rows = lines.Length
    let cols = lines.[0].Length
    let group = Array2D.zeroCreate<Tile> rows cols
    for i in 0..rows - 1 do
        for j in 0..cols - 1 do
            let value = lines.[i].[j]
            let tile = charToTile value i j rows
            group.[i,j] <- tile
    group

let gridToArray (group: Tile[,]) =
    let rows = group.GetLength(0)
    let cols = group.GetLength(1)
    let list = Array.init rows (fun i -> Array.init cols (fun j -> group.[i,j]))
    list

let rec findNearestTile (from: Tile) (tiles: Tile array) =
    match tiles.Length with
    | 0 -> from
    | _ ->
        if tiles.[0].FloorType = Empty then
            findNearestTile tiles.[0] (tiles |> Array.skip 1)
        else
            from

let getNewPos (tile: Tile) (map: Tile[,]) (direction: int[]) =
    match direction with
    | [| r; c |] when r = -1 && c = 0 ->
        let fullCol = map.[*, tile.Y] |> Array.filter (fun t -> t.X < tile.X) |> Array.rev
        findNearestTile tile fullCol

    | _ -> failwith "Not implemented"

let rec moveNorth (group:Tile[,]) (remainingTiles: Tile array) =
    match remainingTiles.Length with
    | 0 -> group
    | _ ->
            //let group' = group |> Array2D.copy
            let newPos = getNewPos remainingTiles.[0] group [| -1; 0 |]
            group.[remainingTiles.[0].X, remainingTiles.[0].Y] <- { FloorType = Empty; X = remainingTiles.[0].X; Y = remainingTiles.[0].Y; InitialLoad = remainingTiles.[0].InitialLoad }
            group.[newPos.X, newPos.Y] <- { FloorType = remainingTiles.[0].FloorType; X = newPos.X; Y = newPos.Y; InitialLoad = newPos.InitialLoad }
            moveNorth group (remainingTiles |> Array.skip 1)

let printGroup (group:Tile[,]) =
    for i in 0..group.GetLength(0)-1 do
        for j in 0..group.GetLength(1)-1 do
            printf "%c" (tileToChar group.[i,j])
        printfn ""

let execute =
    let path = "day14/day14_input.txt"
    let lines = LocalHelper.ReadLines path |> Seq.toList
    let map = parseGroup lines
    let roundedTiles = gridToArray map |> Array.concat |> Array.filter (fun t -> t.FloorType = Rounded)
    let result = moveNorth map roundedTiles
    let newRoundedTiles = gridToArray result |> Array.concat |> Array.filter (fun t -> t.FloorType = Rounded)
    newRoundedTiles |> Array.sumBy _.InitialLoad