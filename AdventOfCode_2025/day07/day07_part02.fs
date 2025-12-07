module day07_part02

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

// slow naive...
//let countTimelines(splitters: HashSet<(int*int)>) =
//    let mutable goDownCalled = 0
//    let rec goDown (position: (int*int)) =
//        let (r, c) = position
//        if r >= maxRow then
//            goDownCalled <- goDownCalled + 1
//            ()
//        else
//            let nextPos = (r + 1, c)
//            if splitters.Contains(nextPos) then
//                printfn "Splitter at %A (Max: %d,%d)" nextPos maxRow maxCol
//                goDown (r + 1, c - 1) 
//                goDown (r + 1, c + 1)
//            else
//                goDown nextPos
//    goDown startPosition
//    goDownCalled

//let countTimelines(splitters: HashSet<(int*int)>) =
//    let toTry = new Queue<(int*int)>()

//    let mutable uniqueTimelines = 0L
//    toTry.Enqueue(startPosition)
//    while toTry.Count > 0 do
//        let position = toTry.Dequeue()
//        if uniqueTimelines % 100000L = 0L then
//            printfn "Unique timelines so far: %d" uniqueTimelines
//        let (r, c) = position
//        if r < maxRow then
//            let nextPos = (r + 1, c)
//            if splitters.Contains(nextPos) then
//                uniqueTimelines <- uniqueTimelines + 2L
//                toTry.Enqueue((r + 1, c - 1))
//                toTry.Enqueue((r + 1, c + 1))
//            else
//                toTry.Enqueue(nextPos)
//    uniqueTimelines

let countTimelines (splitters: HashSet<int*int>) =
    let dp = Array.create (maxCol + 2) 1L
    
    seq { maxRow .. -1 .. 0 }
    |> Seq.iter (fun row ->
        seq { 1 .. maxCol + 1 }
        |> Seq.iter (fun col ->
            if splitters.Contains(row, col - 1) then
                dp[col] <- dp[col - 1] + dp[col + 1]
        )
    )
    
    snd startPosition |> fun startCol -> dp[startCol + 1]

let execute() =
    //let path = "day07/test_input_07.txt"
    let path = "day07/day07_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (splitters, beamMap) = parseContent content
    countTimelines splitters