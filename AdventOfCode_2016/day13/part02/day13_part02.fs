module day13_part02

open AdventOfCode_2016.Modules
open System.Collections.Generic

let buildCell((x, y): int*int) (favouritenumber: int)=
    let r1 = x*x + 3*x + 2*x*y + y + y*y
    let r2 = r1 + favouritenumber
    let r3 = System.Convert.ToString(r2, 2).ToCharArray() |> Array.filter(fun c-> c = '1') |> Array.length
    if r3 % 2 = 0 then "." else "#"

let buildMap((maxcols, maxrows): int*int, favouritenumber: int) =
    let mymap = Array2D.create maxrows maxcols ""
    for row in 0..(maxrows - 1) do
        for col in 0..(maxcols - 1) do
            mymap[row, col] <- buildCell (col, row) favouritenumber
    mymap

let neighbours((row, col): int*int) (map: string[,]) =
    let (maxrows, maxcols) = (map.GetLength(0), map.GetLength(1))
    [
        for (r,c) in [(-1,0); (1,0); (0,-1);(0, 1)] do
            let (nX, nY) = (row+r, col+c)
            if nX >= 0 && nX < maxrows && nY >= 0 && nY < maxcols then
                yield (nX, nY)
    ]

let buildGraph(map: string[,]) =
    let (maxrows, maxcols) = (map.GetLength(0), map.GetLength(1))
    [for row in 0..(maxrows-1) do
        for col in 0..(maxcols-1) do
            if map[row, col] = "." then
                let neighbours =
                    [
                        for (nX, nY) in neighbours (row, col) map do
                            if map[nX, nY] = "." then
                                yield (nX, nY)
                    ]
                yield ((row, col), neighbours)                   
    ]
 
let findPath(start: int*int) (numerOfsteps:int) (graph: IDictionary<(int*int), (int*int) list>) =
    let rec search queue (visited: Set<int*int>) currentStep =
        if currentStep > numerOfsteps then
            visited.Count
        else
            let nextQueue = 
                queue 
                |> Seq.collect (fun node -> 
                    graph[node] 
                    |> List.filter (fun neighbor -> not (Set.contains neighbor visited))
                )
                |> Seq.distinct
                |> List.ofSeq

            let nextVisited = 
                nextQueue 
                |> List.fold (fun acc neighbor -> Set.add neighbor acc) visited

            search nextQueue nextVisited (currentStep + 1)

    search [start] (Set.singleton start) 1

let printmap (mymap: string[,])  =
    let maxrows, maxcols = (mymap.GetLength(0), mymap.GetLength(1))
    for row in 0..(maxrows - 1) do
        for col in 0..(maxcols - 1) do
            printf "%s" mymap[row, col]
        printfn ""

let execute =
    let path = "day13/day13_input.txt"
    let favouritenumber = LocalHelper.GetContentFromFile path |> int
    let (startx, starty) = (1, 1)
    let map = buildMap((100,100), favouritenumber)
    let graph = buildGraph map
    let dictionary = Dictionary<(int * int), (int * int) list>()
    graph |> List.iter (fun (key, value) ->
        dictionary.Add(key, value)
    )
    //printmap map 
    findPath (startx, starty) 50  dictionary