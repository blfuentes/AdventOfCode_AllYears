module day14_part02

open AdventOfCode_2023.Modules
open System

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

let printGroup (group:Tile[,]) =
    for i in 0..group.GetLength(0)-1 do
        for j in 0..group.GetLength(1)-1 do
            printf "%c" (tileToChar group.[i,j])
        printfn ""

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
    | [| r; c |] when r = 1 && c = 0 ->
        let fullCol = map.[*, tile.Y] |> Array.filter (fun t -> t.X > tile.X)
        findNearestTile tile fullCol
    | [| r; c |] when r = 0 && c = -1 ->
        let fullRow = map.[tile.X, *] |> Array.filter (fun t -> t.Y < tile.Y) |> Array.rev
        findNearestTile tile fullRow
    | [| r; c |] when r = 0 && c = 1 ->
        let fullRow = map.[tile.X, *] |> Array.filter (fun t -> t.Y > tile.Y)
        findNearestTile tile fullRow
    | _ -> failwith "Not implemented"

let rec moveOnDirection (group:Tile[,]) (remainingTiles: Tile array) (x:int) (y:int) =
    match remainingTiles.Length with
    | 0 -> group
    | _ ->
        let head = remainingTiles.[0]
        let newPos = getNewPos head group [| x; y |]
        group.[head.X, head.Y] <- { FloorType = Empty; X = head.X; Y = head.Y; InitialLoad = head.InitialLoad }
        group.[newPos.X, newPos.Y] <- { FloorType = head.FloorType; X = newPos.X; Y = newPos.Y; InitialLoad = newPos.InitialLoad }
        moveOnDirection group (remainingTiles |> Array.skip 1) x y

let rec performCycle (group:Tile[,]) (roundedTiles: Tile array) (maxCycles: int) (currentCycle: int) (scores: string list) =
    if currentCycle = maxCycles then
        group
    else
        let afterNorth = moveOnDirection group roundedTiles -1 0           
        let northRoundedTiles = gridToArray afterNorth |> Array.concat |> Array.filter (fun t -> t.FloorType = Rounded)

        let afterWest = moveOnDirection afterNorth northRoundedTiles 0 -1
        let westRoundedTiles = gridToArray afterWest |> Array.concat |> Array.filter (fun t -> t.FloorType = Rounded)

        let afterSouth = moveOnDirection afterWest (westRoundedTiles |> Array.rev) 1 0
        let southRoundedTiles = gridToArray afterSouth |> Array.concat |> Array.filter (fun t -> t.FloorType = Rounded)

        let afterEast = moveOnDirection afterSouth (southRoundedTiles |> Array.rev) 0 1
        let eastRoundedTiles = gridToArray afterEast |> Array.concat |> Array.filter (fun t -> t.FloorType = Rounded)
        
        let key = String.Join("", afterEast |> gridToArray |> Array.concat |> Array.map (fun e -> tileToChar e))

        if scores |> List.contains key then
            let prevHit = scores |> List.findIndex ((=) key)
            let period = currentCycle - prevHit
            let jumps = (((maxCycles-1) - currentCycle) / period)
            let jumpsTo = currentCycle + jumps * period
            let diff = maxCycles - 1 - jumpsTo
            let goalPrev = prevHit + diff   
            let goalPrevKey = scores |> Seq.item goalPrev
            let parts = goalPrevKey.ToCharArray() |> Array.chunkBySize (group.GetLength(0))
            let result = Array2D.create (group.GetLength(0)) (group.GetLength(1)) { FloorType = Empty; X = 0; Y = 0; InitialLoad = 0 }
            for row in 0..parts.Length - 1 do
                for col in 0..parts[0].Length - 1 do
                    let tile = parts.[row].[col]
                    let newTile = charToTile tile row col (group.GetLength(0))
                    result.[row, col] <- newTile
            result
        else               
            performCycle afterEast eastRoundedTiles maxCycles (currentCycle + 1) (scores @ [key])

let execute =
    let path = "day14/day14_input.txt"
    let lines = LocalHelper.ReadLines path |> Seq.toList
    let map = parseGroup lines
    let mapOfCyclesValue = []
    let roundedTiles = gridToArray map |> Array.concat |> Array.filter (fun t -> t.FloorType = Rounded)
    let result = performCycle map roundedTiles 1000000000 0 mapOfCyclesValue
    let newRoundedTiles = gridToArray result |> Array.concat |> Array.filter (fun t -> t.FloorType = Rounded)
    newRoundedTiles |> Array.sumBy _.InitialLoad