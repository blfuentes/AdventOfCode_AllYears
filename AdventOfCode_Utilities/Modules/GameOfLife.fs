module AdventOfCode_Utilities.Modules.GameOfLife

open System
open System.Threading

let width = 30
let height = 30
let generations = 1000000
let alive = 'O'
let dead = '.'

let neighbors = [| (-1,-1); (-1,0); (-1,1); (0,-1); (0,1); (1,-1); (1,0); (1,1) |]

let createGrid() =
    let rnd = System.Random()
    Array2D.init height width (fun _ _ -> if rnd.NextDouble() > 0.7 then alive else dead)

let countLiveNeighbors (grid: char[,]) x y =
    neighbors
    |> Array.sumBy (fun (dx, dy) ->
        let nx, ny = x + dx, y + dy
        if nx >= 0 && nx < height && ny >= 0 && ny < width && grid[nx, ny] = alive then 1 else 0)

let step (grid: char[,]) =
    Array2D.init height width (fun x y ->
        let live = countLiveNeighbors grid x y
        match grid[x, y], live with
        | c, n when c = alive && (n = 2 || n = 3) -> alive
        | c, n when c = dead && n = 3 -> alive
        | _ -> dead)

let printGrid (grid: char[,]) =
    let buffer = System.Text.StringBuilder()
    Console.Clear()
    for x in 0 .. height - 1 do
        for y in 0 .. width - 1 do
            buffer.Append(grid[x, y]) |> ignore
        buffer.AppendLine() |> ignore
    Console.Write(buffer.ToString())
    Thread.Sleep(500)

let run() =
    let mutable grid = createGrid()
    while true do
        printGrid grid
        grid <- step grid
    //let mutable grid = createGrid()
    //for gen in 1 .. generations do
    //    printGrid grid
    //    grid <- step grid
    //Console.WriteLine("Simulation finished.")

run()
