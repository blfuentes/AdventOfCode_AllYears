module day17_part02

open AdventOfCode_2023.Modules
open System.Collections.Generic

type Direction = None | UP | DOWN | LEFT | RIGHT

let turnLeft (direction: Direction) =
    match direction with
    | None -> None
    | UP -> LEFT
    | DOWN -> RIGHT
    | LEFT -> DOWN
    | RIGHT -> UP

let turnRight (direction: Direction) =
    match direction with
    | None -> None
    | UP -> RIGHT
    | DOWN -> LEFT
    | LEFT -> UP
    | RIGHT -> DOWN

let doStep 
    (grid: int[,]) (visited: Dictionary<(Direction*int), int>[,])
    (queue: PriorityQueue<(int*int*Direction*int), int>) (row: int) (col: int) 
    (dir: Direction) (currentHeat: int) 
    (numOfMoves: int) (minSteps: int) (maxSteps: int) =
    let dirRow =
        match dir with
        | UP -> -1
        | DOWN -> 1
        | _ -> 0
    let dirCol =
        match dir with
        | LEFT -> -1
        | RIGHT -> 1
        | _ -> 0

    let outOfBounds (row: int) (col: int) =
        row < 0 || row > grid.GetUpperBound(0) || col < 0 || col > grid.GetUpperBound(1)

    let rec explore (stop: bool) (cc: int) (heat: int) =
        if not stop && cc <= maxSteps then
            let newRow = row + cc * dirRow
            let newCol = col + cc * dirCol
            let newNumOfMoves = numOfMoves + cc
            if not (outOfBounds newRow newCol) && newNumOfMoves <= maxSteps then
                let newHeat = heat + grid[newRow, newCol]
                let stop =
                    if cc >= minSteps then
                        let doContinue =
                            if visited.[newRow, newCol].ContainsKey(dir, newNumOfMoves) then
                                let tempHeat = visited.[newRow, newCol][(dir, newNumOfMoves)]
                                tempHeat > newHeat
                            else
                                true
                        if doContinue  then
                            queue.Enqueue((newRow, newCol, dir, newNumOfMoves), newHeat)
                            let newVisited = new Dictionary<(Direction*int), int>()
                            for x in visited.[newRow, newCol] do
                                newVisited.Add(x.Key, x.Value)
                            newVisited[(dir, newNumOfMoves)] <- newHeat
                            visited.[newRow, newCol] <- newVisited
                        not doContinue
                    else
                        false
                explore stop (cc + 1) newHeat

    explore false 1 currentHeat 

let travel (grid: int[,]) (minSteps: int) (maxSteps: int) =
    let queue = new PriorityQueue<(int*int*Direction*int), int>()
    let visited = Array2D.create (grid.GetUpperBound(0) + 1) (grid.GetUpperBound(1) + 1) (new Dictionary<(Direction*int), int>())
    for i in 0..(grid.GetUpperBound(0) - 1) do
        for j in 0..(grid.GetUpperBound(1) - 1) do
            visited.[i,j] <- new Dictionary<(Direction*int), int>()
    queue.Enqueue((0, 0, RIGHT, 0), 0)
    queue.Enqueue((0, 0, DOWN, 0), 0)

    while(queue.Count > 0) do
        let (row, col, direction, numOfMoves) = queue.Dequeue()
        let heat = 
            if visited.[row, col].ContainsKey((direction, numOfMoves)) then
                visited.[row, col][(direction, numOfMoves)]
            else
                0

        if (numOfMoves < maxSteps) then
            doStep grid visited queue row col direction heat numOfMoves minSteps maxSteps
        if (numOfMoves >= minSteps) then
            doStep grid visited queue row col (turnLeft direction) heat 0 minSteps maxSteps
            doStep grid visited queue row col (turnRight direction) heat 0 minSteps maxSteps


    let maxRow = grid.GetUpperBound(0)
    let maxCol = grid.GetUpperBound(1)
    visited[maxRow, maxCol] |> Seq.minBy(fun x -> x.Value)

let parseInput (input: string list) =
    let grid = Array2D.create (input.Length) (input.[0].Length) 0
    for i in 0..(input.Length-1) do
        for j in 0..(input.[0].Length-1) do
            grid.[i,j] <- (int)(input.[i].[j].ToString())
    grid

let execute =
    let path = "day17/day17_input.txt"
    let lines = LocalHelper.ReadLines path |> List.ofSeq
    let matrix = parseInput lines
    let result = travel matrix 4 10
    result.Value