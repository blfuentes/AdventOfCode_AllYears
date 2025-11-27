module AdventOfCode_Utilities.Modules.PathSolving

open System.Collections.Generic

type Kind = Free | Wall | Start | Goal
type Coord = int * int
type Cell = { Pos: Coord; Kind: Kind }


let maze15x13 : int[,] = array2D [
    [1;1;1;1;1;1;1;1;1;1;1;1;1]
    [1;2;0;0;1;0;0;0;1;0;0;3;1]
    [1;0;1;0;1;0;1;0;1;0;1;1;1]
    [1;0;1;0;0;0;1;0;0;0;0;0;1]
    [1;0;1;1;1;0;1;1;1;1;1;0;1]
    [1;0;0;0;1;0;0;0;0;0;1;0;1]
    [1;1;1;0;1;1;1;1;1;0;1;0;1]
    [1;0;0;0;0;0;0;0;1;0;1;0;1]
    [1;0;1;1;1;1;1;0;1;0;1;0;1]
    [1;0;1;0;0;0;1;0;1;0;1;0;1]
    [1;0;1;0;1;0;1;0;1;0;1;0;1]
    [1;0;1;0;1;0;1;0;1;0;1;0;1]
    [1;0;0;0;1;0;0;0;1;0;0;0;1]
    [1;1;1;0;1;1;1;1;1;1;1;1;1]
    [1;1;1;1;1;1;1;1;1;1;1;1;1]
]

let buildMaze (data: int[,]) =
    let maxRows = data.GetLength(0)
    let maxCols = data.GetLength(1)
    let maze = Array2D.zeroCreate<Cell> maxRows maxCols 
    for rIdx in 0..maxRows-1 do
        for cIdx in 0..maxCols-1 do
            let t' = 
                match data[rIdx, cIdx] with
                | 0 -> Free
                | 1 -> Wall
                | 2 -> Start
                | 3 -> Goal
                | _ -> failwith "error"
            maze[rIdx, cIdx] <- { Pos = (rIdx, cIdx); Kind = t' }
    maze

///////////////
let themaze = buildMaze maze15x13
///////////////

let findKind (maze: Cell[,]) kind =
    maze
    |> Seq.cast<Cell>
    |> Seq.find (fun cell -> cell.Kind = kind)
    |> fun cell -> cell.Pos


printfn "=================== Maze Pathfinding Algorithms =================="
printfn "::::::::::::::::::: Flood Fill :::::::::::::::::::::"

// Flood fill
let floodFill (maze: Cell[,]) : Set<Coord> =
    let getNeighbors (maze: Cell[,]) (x, y) =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter (fun (r, c) ->
            r >= 0 && r < rows && c >= 0 && c < cols &&
            match maze[r, c].Kind with Wall -> false | _ -> true
        )

    let start = findKind maze Start
    let visited = HashSet<Coord>()
    let queue = Queue<Coord>()
    queue.Enqueue(start)
    visited.Add(start) |> ignore

    while queue.Count > 0 do
        let current = queue.Dequeue()
        for n in getNeighbors maze current do
            if not (visited.Contains(n)) then
                visited.Add(n) |> ignore
                queue.Enqueue(n)

    visited |> Set.ofSeq

let resultFloodFill = floodFill themaze
printfn "Floodfill: %A" resultFloodFill

printfn "::::::::::::::::::: Flood Fill All Paths :::::::::::::::::::::"

// Flood fill all paths
let floodFillAllPaths (maze: Cell[,]) : Coord list list =
    let getNeighbors (maze: Cell[,]) (x, y) =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter (fun (r, c) ->
            r >= 0 && r < rows && c >= 0 && c < cols &&
            match maze[r, c].Kind with Wall -> false | _ -> true
        )
    let start = findKind maze Start
    let goal = findKind maze Goal

    let results = ResizeArray<Coord list>()

    let rec flood visited path current =
        if current = goal then
            results.Add(List.rev (current :: path))
        else
            for neighbor in getNeighbors maze current do
                if not (Set.contains neighbor visited) then
                    flood (Set.add neighbor visited) (current :: path) neighbor

    flood (Set.singleton start) [] start
    results |> Seq.toList

let allPathsFloodFill = floodFillAllPaths themaze
let shortestFloodFill = allPathsFloodFill |> List.minBy List.length
printfn "Flood fill all paths"
allPathsFloodFill |> List.iteri(fun i p -> 
        printfn "Flood fill path %A. Length: %A :: %A" (i+1) p.Length p
)

printfn "::::::::::::::::::: Flood Fill Steps :::::::::::::::::::::"

// Floodfill steps
let floodFillSteps (maze: Cell[,]) : Dictionary<Coord, int> =
    let getNeighbors (maze: Cell[,]) (x, y) =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter (fun (r, c) ->
            r >= 0 && r < rows && c >= 0 && c < cols &&
            match maze[r, c].Kind with Wall -> false | _ -> true
        )
    let start = findKind maze Start

    let rows = maze.GetLength(0)
    let cols = maze.GetLength(1)

    let steps = Dictionary<Coord, int>()
    let queue = Queue<Coord>()
    queue.Enqueue(start)
    steps[start] <- 0

    while queue.Count > 0 do
        let current = queue.Dequeue()
        let currentSteps = steps.[current]
        for neighbor in getNeighbors maze current do
            if not (steps.ContainsKey(neighbor)) then
                steps[neighbor] <- currentSteps + 1
                queue.Enqueue(neighbor)

    steps

printfn "::::::::::::::::::: Flood Fill Steps with Parents ::::::::::::::::::::"

// Floodfill steps with parents
let floodFillStepsWithParents (maze: Cell[,]) : Dictionary<Coord, int> * Dictionary<Coord, Coord> =
    let getNeighbors (maze: Cell[,]) (x, y) =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter (fun (r, c) ->
            r >= 0 && r < rows && c >= 0 && c < cols &&
            match maze[r, c].Kind with Wall -> false | _ -> true
        )
    let start = findKind maze Start

    let rows = maze.GetLength(0)
    let cols = maze.GetLength(1)

    let steps = Dictionary<Coord, int>()
    let parent = Dictionary<Coord, Coord>()
    let queue = Queue<Coord>()
    queue.Enqueue(start)
    steps.[start] <- 0

    while queue.Count > 0 do
        let current = queue.Dequeue()
        let currentSteps = steps.[current]
        for neighbor in getNeighbors maze current do
            if not (steps.ContainsKey(neighbor)) then
                steps[neighbor] <- currentSteps + 1
                parent[neighbor] <- current
                queue.Enqueue(neighbor)

    steps, parent

let steps, parent = floodFillStepsWithParents themaze
let goal = themaze |> Seq.cast<Cell> |> Seq.find (fun c -> c.Kind = Goal) |> fun c -> c.Pos

let rec buildPath acc pos =
    if parent.ContainsKey(pos) then buildPath (pos :: acc) parent.[pos]
    else pos :: acc

let pathToGoal = buildPath [] goal
printfn "Steps to goal: %d" steps.[goal]
printfn "Path: %A" pathToGoal

printfn "::::::::::::::::::: Depth-First Search :::::::::::::::::::::"

// DFS
let dfs (maze: Cell[,]) =
    let getNeighbors (maze: Cell[,]) (x, y) =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter (fun (r, c) ->
            r >= 0 && r < rows && c >= 0 && c < cols &&
            match maze[r, c].Kind with Wall -> false | _ -> true
        )
    let start = findKind maze Start
    let goal = findKind maze Goal
    let visited = HashSet<Coord>()
    let parent = Dictionary<Coord, Coord>()

    let rec search pos =
        if pos = goal then true
        else
            visited.Add(pos) |> ignore
            getNeighbors maze pos
            |> List.exists (fun n ->
                if not (visited.Contains(n)) then
                    parent[n] <- pos
                    search n
                else false
            )

    let found = search start
    let rec buildPath acc pos =
        if pos = start then start :: acc
        elif parent.ContainsKey(pos) then buildPath (pos :: acc) parent[pos]
        else []

    let path = if found then buildPath [] goal else []
    path.Length, path

let resultDFS = dfs themaze
printfn "DFS: %A" resultDFS

printfn "::::::::::::::::::: Depth-First Search All Paths :::::::::::::::::::::"

// DFS All paths
let dfsAllPaths (maze: Cell[,]) : Coord list list =
    let start = findKind maze Start
    let goal = findKind maze Goal

    let rows = maze.GetLength(0)
    let cols = maze.GetLength(1)

    let isValid (x, y) =
        x >= 0 && x < rows && y >= 0 && y < cols &&
        match maze[x, y].Kind with Wall -> false | _ -> true

    let getNeighbors (x, y) =
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter isValid

    let results = ResizeArray<Coord list>()

    let rec dfs visited path current =
        if current = goal then
            results.Add(List.rev (current :: path))
        else
            for neighbor in getNeighbors current do
                if not (Set.contains neighbor visited) then
                    dfs (Set.add neighbor visited) (current :: path) neighbor

    dfs (Set.singleton start) [] start
    results |> Seq.toList

let allPathdsDfs = dfsAllPaths themaze
let shortestDfs = allPathdsDfs |> List.minBy List.length
printfn "DFS all paths"
allPathdsDfs |> List.iteri(fun i p -> 
        printfn "DFS path %A. Length: %A :: %A" (i+1) p.Length p
)

printfn "::::::::::::::::::: Depth-First Search Stack :::::::::::::::::::::"

// Simplified DFS using an explicit stack
let dfsStack (maze: Cell[,]) =
    let getNeighbors (maze: Cell[,]) (x, y) =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter (fun (r, c) ->
            r >= 0 && r < rows && c >= 0 && c < cols &&
            match maze[r, c].Kind with Wall -> false | _ -> true
        )
    let start = findKind maze Start
    let goal = findKind maze Goal
    let visited = HashSet<Coord>()
    let parent = Dictionary<Coord, Coord>()
    let stack = System.Collections.Generic.Stack<Coord>()
    stack.Push(start)
    visited.Add(start) |> ignore

    let mutable found = false
    while stack.Count > 0 && not found do
        let pos = stack.Pop()
        if pos = goal then found <- true
        else
            for n in getNeighbors maze pos do
                if not (visited.Contains(n)) then
                    visited.Add(n) |> ignore
                    parent.Add(n, pos)
                    stack.Push(n)

    let rec buildPath acc pos =
        if pos = start then start :: acc
        elif parent.ContainsKey(pos) then buildPath (pos :: acc) parent.[pos]
        else []
    let path = if found then buildPath [] goal else []
    path.Length, path

let resultDFSStack = dfsStack themaze
printfn "DFS stack: %A" resultDFSStack


printfn "::::::::::::::::::: Depth-First Search with Explicit Stack :::::::::::::::::::::"

// DFS using an explicit stack. Returns Some path from start to goal (inclusive) or None.
let dfsWithStack (maze: Cell[,]) : Coord list option =
    // Find coordinates of a cell value (first occurrence)
    let findValue (maze: Cell[,]) (value: Kind) : Coord option =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        let mutable found = None
        for r in 0 .. rows - 1 do
            for c in 0 .. cols - 1 do
                if maze[r, c].Kind = value && found.IsNone then
                    found <- Some (r, c)
        found
    let rows = maze.GetLength(0)
    let cols = maze.GetLength(1)

    match findValue maze Start, findValue maze Goal with
    | Some start, Some goal ->
        let stack = Stack<Coord>()
        let visited = HashSet<Coord>()
        let parent = Dictionary<Coord, Coord>()

        stack.Push(start)
        visited.Add(start) |> ignore
        parent.[start] <- start

        let mutable found = false
        let mutable goalCoord = start

        // neighbor deltas: up, right, down, left (adjust ordering if you prefer)
        let deltas = [(-1,0); (0,1); (1,0); (0,-1)]

        while stack.Count > 0 && not found do
            let cur = stack.Pop()
            if cur = goal then
                found <- true
                goalCoord <- cur
            else
                let (r, c) = cur
                for (dr, dc) in deltas do
                    let nr = r + dr
                    let nc = c + dc
                    if nr >= 0 && nr < rows && nc >= 0 && nc < cols then
                        let cell = maze[nr, nc]
                        if not cell.Kind.IsWall then // not a wall
                            let ncoord = (nr, nc)
                            if not (visited.Contains ncoord) then
                                visited.Add(ncoord) |> ignore
                                parent.[ncoord] <- cur
                                stack.Push(ncoord)

        if not found then None
        else
            // Reconstruct path from goal to start
            let rec build acc node =
                if node = start then
                    List.rev (start :: acc)
                else
                    let p = parent.[node]
                    build (node :: acc) p
            Some (build [] goalCoord)

    | _ -> None

// Utility to print maze with path overlay
let printMazeWithPath (maze: Cell[,]) (pathOpt: Coord list option) =
    let rows = maze.GetLength(0)
    let cols = maze.GetLength(1)
    let pathSet = 
        match pathOpt with
        | None -> Set.empty
        | Some lst -> lst |> Set.ofList
    for r in 0 .. rows - 1 do
        for c in 0 .. cols - 1 do
            let ch =
                if pathSet.Contains (r, c) then
                    if maze[r,c].Kind = Start then 'S' elif maze[r,c].Kind = Goal then 'G' else '*'
                else
                    match maze[r,c].Kind with
                    | Wall -> '#'
                    | Start -> 'S'
                    | Goal -> 'G'
                    | _ -> '.'
            printf "%c" ch
        printfn ""
    ()

let resultDfsStack = dfsWithStack themaze
printfn "DFS with stack: %A" resultDfsStack
//printMazeWithPath themaze resultDfsStack

printfn "::::::::::::::::::: Breadth-First Search :::::::::::::::::::::"

// BFS
let bfs (maze: Cell[,]) =
    let getNeighbors (maze: Cell[,]) (x, y) =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter (fun (r, c) ->
            r >= 0 && r < rows && c >= 0 && c < cols &&
            match maze[r, c].Kind with Wall -> false | _ -> true
        )
    let start = findKind maze Start
    let goal = findKind maze Goal
    let visited = HashSet<Coord>()
    let parent = Dictionary<Coord, Coord>()
    let queue = Queue<Coord>()
    queue.Enqueue(start)
    visited.Add(start) |> ignore

    let mutable found = false
    while queue.Count > 0 && not found do
        let current = queue.Dequeue()
        if current = goal then 
            found <- true
        else
            for n in getNeighbors maze current do
                if not (visited.Contains(n)) then
                    visited.Add(n) |> ignore
                    parent[n] <- current
                    queue.Enqueue(n)

    let rec buildPath acc pos =
        if pos = start then start :: acc
        elif parent.ContainsKey(pos) then buildPath (pos :: acc) parent[pos]
        else []

    let path = if found then buildPath [] goal else []
    path.Length, path

let resultBFS = bfs themaze
printfn "BFS: %A" resultBFS

printfn "::::::::::::::::::: Dijkstra :::::::::::::::::::::"

// DIJKSTRA
let dijkstra (maze: Cell[,]) =
    let getNeighbors (maze: Cell[,]) (x, y) =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter (fun (r, c) ->
            r >= 0 && r < rows && c >= 0 && c < cols &&
            match maze[r, c].Kind with Wall -> false | _ -> true
        )
    let start = findKind maze Start
    let goal = findKind maze Goal

    let dist = Dictionary<Coord, int>()
    let parent = Dictionary<Coord, Coord>()
    let visited = HashSet<Coord>()

    // Priority queue: (distance, position)
    let pq = SortedSet<(int * Coord)>(Comparer<(int * Coord)>.Create(fun (d1, c1) (d2, c2) ->
        if d1 <> d2 then compare d1 d2 else compare c1 c2
    ))

    dist[start] <- 0
    pq.Add((0, start)) |> ignore

    let mutable found = false
    while pq.Count > 0 && not found do
        let (d, current) = pq.Min
        pq.Remove((d, current)) |> ignore

        if visited.Contains(current) then
            () // already processed - skip duplicate
        elif current = goal then
            found <- true // early termination
        else
            visited.Add(current) |> ignore

            for neighbor in getNeighbors maze current do
                let alt = d + 1 // uniform cost
                if not (visited.Contains(neighbor)) && 
                   (not (dist.ContainsKey(neighbor)) || alt < dist[neighbor]) then
                    // Remove old entry if exists (to avoid duplicates)
                    if dist.ContainsKey(neighbor) then
                        pq.Remove((dist[neighbor], neighbor)) |> ignore
                    dist[neighbor] <- alt
                    parent[neighbor] <- current
                    pq.Add((alt, neighbor)) |> ignore

    // Path reconstruction
    let rec buildPath acc pos =
        if pos = start then start :: acc
        elif parent.ContainsKey(pos) then buildPath (pos :: acc) parent[pos]
        else []

    let path = if found then buildPath [] goal else []
    path.Length, path

let resultDijkstra = dijkstra themaze
printfn "Dijkstra: %A" resultDijkstra

// A*
let manhattan (x1, y1) (x2, y2) = abs(x1 - x2) + abs(y1 - y2)


printfn "::::::::::::::::::: A* Search :::::::::::::::::::::"

let aStar (maze: Cell[,]) =
    let getNeighbors (maze: Cell[,]) (x, y) =
        let rows = maze.GetLength(0)
        let cols = maze.GetLength(1)
        [ (x-1, y); (x+1, y); (x, y-1); (x, y+1) ]
        |> List.filter (fun (r, c) ->
            r >= 0 && r < rows && c >= 0 && c < cols &&
            match maze[r, c].Kind with Wall -> false | _ -> true
        )
    let start = findKind maze Start
    let goal = findKind maze Goal
    let gScore = Dictionary<Coord, int>()
    let fScore = Dictionary<Coord, int>()
    let parent = Dictionary<Coord, Coord>()
    let closedSet = HashSet<Coord>()
    let openSet = SortedSet<(int * Coord)>(Comparer<(int * Coord)>.Create(fun (f1, c1) (f2, c2) ->
        if f1 <> f2 then compare f1 f2 else compare c1 c2
    ))

    gScore[start] <- 0
    fScore[start] <- manhattan start goal
    openSet.Add((fScore[start], start)) |> ignore

    let mutable found = false
    while openSet.Count > 0 && not found do
        let (_, current) = openSet.Min
        openSet.Remove((fScore[current], current)) |> ignore
        
        if closedSet.Contains(current) then
            () // already processed - skip duplicate
        elif current = goal then
            found <- true // early termination
        else
            closedSet.Add(current) |> ignore
            
            for n in getNeighbors maze current do
                if not (closedSet.Contains(n)) then
                    let tentativeG = gScore[current] + 1
                    if not (gScore.ContainsKey(n)) || tentativeG < gScore[n] then
                        // Remove old entry if exists (to avoid duplicates)
                        if fScore.ContainsKey(n) then
                            openSet.Remove((fScore[n], n)) |> ignore
                        parent[n] <- current
                        gScore[n] <- tentativeG
                        fScore[n] <- tentativeG + manhattan n goal
                        openSet.Add((fScore[n], n)) |> ignore

    let rec buildPath acc pos =
        if pos = start then start :: acc
        elif parent.ContainsKey(pos) then buildPath (pos :: acc) parent[pos]
        else []

    let path = if found then buildPath [] goal else []
    path.Length, path

let resultAStar = aStar themaze
printfn "A Star: %A" resultAStar