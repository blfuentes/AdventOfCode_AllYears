module day22_part01

open AdventOfCode_2017.Modules

open System.Collections.Generic

type Status =
    | Clean
    | Infected

type Direction =
    | Up
    | Down
    | Left
    | Right

let parseContent (lines: string array) =
    let (maxrows, maxcols) = (lines.Length / 2, lines[0].Length / 2)
    let map = Dictionary<(int * int), Status>()
    let (startrow, startcol) = (0 - maxrows, 0 - maxcols)
    let mutable row = 0
    let mutable col = 0
    for r' in startrow..maxrows do
        col <- 0
        for c' in startcol..maxcols do
            let status =
                match lines[row][col] with
                | '.' -> Clean
                | '#' -> Infected
                | _ -> failwith "error"
            map.Add((r', c'), status)
            col <- col + 1
        row <- row + 1
    map

let turn (current: Direction) (switch: Direction) =
    match (current, switch) with
    | (Up, Left) -> ((0, -1), Left)
    | (Down, Left) -> ((0, 1), Right)
    | (Up, Right) -> ((0, 1), Right)
    | (Down, Right) -> ((0, -1), Left)
    | (Left, Left) -> ((1, 0), Down)
    | (Right, Left) -> ((-1, 0), Up)
    | (Left, Right) -> ((-1, 0), Up)
    | (Right, Right) -> ((1, 0), Down)
    | _ -> failwith "error turning"     

let printMap(map: Dictionary<(int*int), Status>) =
    let minRows = map.Keys |> Seq.map fst |> Seq.min 
    let maxRows = map.Keys |> Seq.map fst |> Seq.max
    let minCols = map.Keys |> Seq.map snd |> Seq.min 
    let maxCols = map.Keys |> Seq.map snd |> Seq.max

    for row in minRows..maxRows do
        for col in minCols..maxCols do
            if map.ContainsKey((row, col)) then
                printf "%s" (if map[(row, col)].IsClean then "." else "#")
            else
                printf "%s" "."
        printfn "%s" System.Environment.NewLine

let startBurst (map: Dictionary<(int*int), Status>) ((carrier, dir): (int*int)*Direction) (numOfBurst: int) =
    let rec burst ((c, d): (int*int)*Direction) (currentBurst: int) (numInfections: int)=
        if currentBurst = numOfBurst then
            numInfections
        else
            let currentnode =
                match map.TryGetValue(c) with
                | true, node -> node
                | false, _ -> 
                    map.Add(c, Clean)
                    map[c]
            let ((dr, dc), newdir), newinfections =
                if currentnode.IsInfected then
                    map[c] <- Clean
                    (turn d Right, numInfections)
                else
                    map[c] <- Infected
                    (turn d Left, numInfections+1)

            let (r, c) = c
            burst ((r + dr, c + dc), newdir) (currentBurst + 1) newinfections
    
    burst (carrier, dir) 0 0

let execute() =
    let path = "day22/day22_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let map = parseContent content

    let (initcarrier, dir) = ((0, 0), Up)
    startBurst map (initcarrier, dir) 10000