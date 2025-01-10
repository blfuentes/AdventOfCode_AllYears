module day13_part01

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
 
let findPath(start: int*int) (endNode : int*int) (graph: IDictionary<(int*int), (int*int) list>) =
    let visited = HashSet<int*int>()
    let queue = Queue<(int*int) * (int*int) list>()

    queue.Enqueue(start, [])
    let _ = visited.Add(start)
    let rec search () =
        if queue.Count > 0 then
            let (node, path) = queue.Dequeue()

            if node = endNode then
                Some(path)
            else
                graph.[node]
                |> List.iter (fun neighbor ->
                    if not (visited.Contains(neighbor)) then
                        let _ = visited.Add(neighbor)
                        queue.Enqueue(neighbor, path @ [neighbor])
                )
                search ()
        else
            None

    search()

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
    let (targetx, targety) = (39,31)
    let map = buildMap((50,50), favouritenumber)
    let graph = buildGraph map
    let dictionary = Dictionary<(int * int), (int * int) list>()
    graph |> List.iter (fun (key, value) ->
        dictionary.Add(key, value)
    )
    printmap map 
    let walk = findPath (startx, starty) (targetx, targety)dictionary
    match walk with
    | Some(w) -> w.Length
    | None -> 0