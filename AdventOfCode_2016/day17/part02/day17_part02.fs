module day17_part02

open AdventOfCode_2016.Modules

open System
open System.Collections.Generic
open System.Text

type DoorStateKind =
    | Open
    | Closed

type Pos = {
    Row: int
    Col: int
}

let md5 = System.Security.Cryptography.MD5.Create()

let hash (s:string) =
    Encoding.Default.GetBytes(s)
    |> md5.ComputeHash
    |> (fun h -> BitConverter.ToString(h).ToLower().Replace("-",""))

let doorState (door: char) =
    match door with 
    | d when d = 'b' || d = 'c' || d = 'd' || d = 'e'|| d = 'f' ->
        Open
    | _ -> Closed

let doorStates (doors : string) =
    let states =
        doors.ToCharArray()
        |> Array.take(4)
        |> Array.map doorState
    [
        ((states[0], "U"), (-1, 0));
        ((states[1], "D"), (1, 0));
        ((states[2], "L"), (0, -1));
        ((states[3], "R"), (0, 1))
    ]

let findLongestPath (init: string) ((start, goal): (Pos*Pos)) =
    let (maxRows, maxCols) = (4, 4)

    let isInBoundaries pos =
        pos.Row >= 0 && pos.Col >= 0 && pos.Row < maxRows && pos.Col < maxCols

    let queue = Queue<Pos*string>()
    queue.Enqueue(start, "")

    let mutable longestPath = ""

    let rec walk (currentpath: string) =
        if queue.Count = 0 then None
        else
            let (current, path) = queue.Dequeue()
            if current = goal then
                if path.Length > longestPath.Length then
                    longestPath <- path
                walk currentpath
            else
                let doorhash = hash (currentpath + path)
                let validStates = doorStates doorhash |> List.filter (fun ((s, _), _) -> s.IsOpen)
                for ((_, d), (dr, dc)) in validStates do
                    let nextpos = { Row = current.Row + dr; Col = current.Col + dc }
                    if isInBoundaries nextpos then
                        queue.Enqueue((nextpos, path + d))
                walk currentpath

    walk init |> ignore
    Some(longestPath)

let execute =
    let path = "day17/day17_input.txt"

    let content = LocalHelper.GetContentFromFile path
    match findLongestPath content ({ Row = 0; Col = 0 }, {  Row = 3; Col = 3 }) with
    | None -> 0
    | Some(p) -> p.Length