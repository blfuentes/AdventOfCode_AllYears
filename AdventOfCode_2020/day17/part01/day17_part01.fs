﻿module day17_part01

open AdventOfCode_2020.Modules

let enumerate s = s |> Seq.zip (Seq.initInfinite (fun n -> n))

let parseLine row line =
    enumerate line
    |> Seq.collect (fun (col, c) ->
        match c with
        | '#' -> [(row, col, 0)]
        | _ -> [])

let parseContent(lines: string array) =
    (enumerate lines)
    |> Seq.collect (fun (row, line) -> parseLine row line)
    |> Seq.toList

// from https://stackoverflow.com/questions/9213761/cartesian-product-two-lists
let cartesian xs ys =
    xs |> Seq.collect (fun x -> ys |> Seq.map (fun y -> x, y))

let cartesian3 xs ys zs =
    xs
    |> cartesian ys
    |> cartesian zs
    |> Seq.map (fun (c, (b, a)) -> (a, b, c))
    |> Seq.toList

let neighbors (row, col, layer) =
    cartesian3 [row - 1; row; row + 1] [col - 1; col; col + 1] [layer - 1; layer; layer + 1]
    |> Seq.filter ((<>) (row, col, layer))
    |> Seq.toList

let liveNeighbors grid cell =
    cell
    |> neighbors
    |> List.filter (fun n -> List.contains n grid)
    |> List.length

let step grid =
    let activeCells =
        grid
        |> List.collect (fun cell -> cell :: neighbors cell)
        |> List.distinct

    let isAlive oldGrid cell =
        let n = liveNeighbors oldGrid cell
        if (List.contains cell oldGrid) && n = 2 then true else (n = 3)

    let newGrid =
        activeCells
        |> List.filter (isAlive grid)

    newGrid

let execute =
    let path = "day17/day17_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let map = parseContent content

    map
    |> step
    |> step
    |> step
    |> step
    |> step
    |> step
    |> List.length