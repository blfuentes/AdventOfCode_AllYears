module day18_part01

open AdventOfCode_2019.Modules
open System.Collections.Generic
open System

type Cell =
    | Entrance
    | Open
    | Wall
    | Door of char
    | Key of char

let parseCell =
    function
    | '@' -> Entrance
    | '.' -> Open
    | '#' -> Wall
    | c when Char.IsUpper(c) -> Door (Char.ToLower(c))
    | c -> Key (c)

type Maze =
    { Grid : Cell [] []
      Entrance : int * int 
      Keys : Map<char, int * int> }

    static member create grid =
        ({ Grid = grid; Entrance = (-1, -1); Keys = Map.empty }, Array.indexed grid)
        ||> Array.fold (fun g (y, row) ->
            (g, Array.indexed row)
            ||> Array.fold (fun g (x, cell) ->
                    match cell with
                    | Entrance -> { g with Entrance = (x, y) }
                    | Key k -> { g with Keys = Map.add k (x, y) g.Keys }
                    | _ -> g))

let parseContent(lines: string array) =
    let cells =
        lines
        |> Seq.map ((Seq.map parseCell >> Seq.toArray) >> Seq.toArray)
        |> Seq.toArray
    Maze.create cells

let astar start isDest heuristic getEdges rep =
    let seen = new HashSet<_>()
    let rec astar' fringe =
        if Set.isEmpty fringe then None
        else
            let (_, negDist : int, vertex) as minElem = Set.minElement fringe
            let fringe' = Set.remove minElem fringe
            let dist = -negDist
            if seen.Contains(rep vertex) then astar' fringe'
            elif isDest vertex then Some (vertex, dist)
            else
                seen.Add (rep vertex) |> ignore
                getEdges vertex
                |> Seq.map (fun (v, d) ->
                    let dist = dist + d
                    (heuristic v) + dist, -dist, v)
                |> Set.ofSeq
                |> Set.union fringe'
                |> astar'
    astar' (Set.ofList [0, 0, start])

let bfs start isDest getEdges rep = astar start isDest (fun _ -> 0) getEdges rep

let neighbours (x, y) = [| (x + 1, y); (x - 1, y); (x, y + 1); (x, y - 1) |]
let diagNeighbours (x, y) = [| (x + 1, y + 1); (x - 1, y + 1); (x + 1, y - 1); (x - 1, y - 1) |]

let manhattan (x1, y1) (x2, y2) =
    abs (y1 - y2) + abs (x1 - x2)

type BetweenKeySearchState =
    { Pos : int * int
      PassedDoors : Set<char> }

let shortestPathBetweenKeys entrance maze fromKey toKey =
    let startPos = if fromKey = '@' then entrance else maze.Keys.[fromKey]
    let endPos = if toKey = '@' then entrance else maze.Keys.[toKey]

    let getCell (x, y) = Array.tryItem y maze.Grid |> Option.bind (Array.tryItem x) |> Option.defaultValue Wall
    let getEdges state =
        neighbours state.Pos
        |> Array.choose (fun pos ->
            match getCell pos with
            | Wall -> None
            | Door k -> Some { state with Pos = pos; PassedDoors = Set.add k state.PassedDoors }
            | _ -> Some { state with Pos = pos })
        |> Array.map (fun s -> s, 1)

    let isDest s = s.Pos = endPos
    let heuristic s = manhattan endPos s.Pos
    let rep s = s.Pos

    let result = astar { Pos = startPos; PassedDoors = Set.empty } isDest heuristic getEdges rep
    match result with
    | Some ({ PassedDoors = p }, dist) -> Some (dist, p)
    | None -> None

type AllKeyPart1SearchState =
    { CurrentKey : char
      Keys : Set<char> }

let shortestPathAllKeysPart1 keys (distsBetween : Map<char * char, int * Set<char>>) =
    let keyCount = Set.count keys
    let getEdges state =
        Set.difference keys state.Keys
        |> Seq.toArray
        |> Array.choose (fun k -> 
            let dist, doors = distsBetween.[state.CurrentKey, k]
            if Set.isSubset doors state.Keys then
                Some ({ state with CurrentKey = k; Keys = Set.add k state.Keys }, dist)
            else None)

    let isDest s = s.Keys.Count = keyCount
    let rep s = s

    bfs { CurrentKey = '@'; Keys = set ['@'] } isDest getEdges rep

let solvePart1 (maze : Maze) =
    let keys = maze.Keys |> Map.add '@' maze.Entrance |> Map.toArray |> Array.map fst
    let shortestPaths = 
        Array.allPairs keys keys 
        |> Array.where (fun (a, b) -> a <> b)
        |> Array.map (fun (a, b) -> (a, b), shortestPathBetweenKeys maze.Entrance maze a b |> Option.get)
        |> Map.ofArray
    shortestPathAllKeysPart1 (set keys) shortestPaths
    |> Option.get
    |> snd

let execute =
    let path = "day18/day18_input.txt"
    let content = LocalHelper.GetLinesFromFile path |> parseContent
    solvePart1 content