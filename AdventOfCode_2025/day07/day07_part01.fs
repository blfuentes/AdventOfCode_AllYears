module day07_part01
  
open AdventOfCode_2025.Modules
open System.Collections.Generic

let mutable startPosition = (0, 0)
let mutable maxRow = 0
let mutable maxCol = 0

let parseContent (lines: string array) =
    maxRow <- lines.Length - 1
    maxCol <- lines[0].Length - 1
    let splitters = new HashSet<(int*int)>()
    let beamMap = Array2D.init (maxRow+1) (maxCol+1) (fun r c ->
        match lines[r][c] with
        | '^' -> 
            splitters.Add((r, c)) |> ignore;
        | '.' -> ()
        | 'S' -> startPosition <- (r, c)
        | _ -> ()
        lines[r][c]
    )
    (splitters, beamMap)

// recusrsive...
//let countSplitTimes(splitters: HashSet<(int*int)>) =
//    let visited = new HashSet<(int*int)>()
//    let rec goDown (position: (int*int)) =
//        let (r, c) = position
//        if r < 0 || r > maxRow || c < 0 || c > maxCol then
//            ()
//        else
//            let nextPos = (r + 1, c)
//            if splitters.Contains(nextPos) then
//                if visited.Contains(nextPos) then
//                    ()
//                else
//                    visited.Add(nextPos) |> ignore
//                    goDown (r + 1, c - 1) 
//                    goDown (r + 1, c + 1)
//            else
//                goDown nextPos
//    goDown startPosition
//    visited.Count

let countSplitTimes(splitters: HashSet<(int*int)>) =
    let visited = new HashSet<(int*int)>()    
    let toTry = new Queue<(int*int)>()

    toTry.Enqueue(startPosition)
    while toTry.Count > 0 do
        let position = toTry.Dequeue()
        let (r, c) = position
        if r < maxRow then
            let nextPos = (r + 1, c)
            if splitters.Contains(nextPos) then
                if visited.Add(nextPos) then
                    toTry.Enqueue((r + 1, c - 1))
                    toTry.Enqueue((r + 1, c + 1))
            else
                toTry.Enqueue(nextPos)
    visited.Count

let execute() =
    //let path = "day07/test_input_07.txt"
    let path = "day07/day07_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (splitters, beamMap) = parseContent content
    countSplitTimes splitters