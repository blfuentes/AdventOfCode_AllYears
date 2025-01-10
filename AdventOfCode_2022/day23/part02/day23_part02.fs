module day23_part02

open AdventOfCode_2022.Modules
open AdventOfCode_Utilities
open FSharp.Collections.ParallelSeq

let path = "day23/day23_input.txt"
let input =
    LocalHelper.GetLinesFromFile path
    |> Array.map _.ToCharArray()

let initialElves =
    seq {
        for row in 0 .. Array.length input - 1 do
            for col in 0 .. Array.length input[row] - 1 do
                if input[row][col] = '#' then row, col
    }
    |> Set.ofSeq

type Direction =
    | North
    | South
    | West
    | East

let directions = [| North; South; West; East |]

let neighbors dir (r, c) =
    seq {
        for i in -1 .. 1 do
            match dir with
            | North -> r - 1, c + i
            | South -> r + 1, c + i
            | West -> r + i, c - 1
            | East -> r + i, c + 1
    }

let hasNeighbors elves (r, c) =
    seq {
        for i in -1 .. 1 do
            for j in -1 .. 1 do
                if i <> 0 || j <> 0 then (r + i, c + j)
    }
    |> Seq.exists (fun n -> Set.contains n elves)

let go dir (r, c) =
    match dir with
    | North -> r - 1, c
    | South -> r + 1, c
    | West -> r, c - 1
    | East -> r, c + 1

let step elves firstDirection =
    let sortedDirections =
        seq { firstDirection .. firstDirection + 3 }
        |> Seq.map (fun i -> directions[i % 4])

    let proposals =
        elves
        |> Seq.filter (fun pt -> hasNeighbors elves pt)
        |> Seq.map (fun pt ->
            sortedDirections
            |> Seq.map (fun dir -> dir, neighbors dir pt)
            |> Seq.tryFind (fun (_, ns) -> not (Seq.exists (fun n -> Set.contains n elves) ns))
            |> Option.map (fun (dir, _) -> pt, go dir pt))
        |> Seq.filter Option.isSome
        |> Seq.map Option.get
        |> Seq.fold
            (fun map (source, target) ->
                Map.change
                    target
                    (function
                    | None -> Some [ source ]
                    | Some l -> Some(source :: l))
                    map)
            Map.empty

    proposals
    |> Map.filter (fun _ v -> List.length v = 1)
    |> Map.map (fun _ v -> List.exactlyOne v)
    |> Map.fold (fun es target source -> es |> Set.remove source |> Set.add target) elves

let countEmpty elves =
    let rs = elves |> Set.map fst
    let cs = elves |> Set.map snd

    [ for r in Seq.min rs .. Seq.max rs do
          for c in Seq.min cs .. Seq.max cs do
              (r, c) ]
    |> Seq.filter (fun pt -> not (Set.contains pt elves))
    |> Seq.length

let part2 =
    Seq.unfold
        (fun (elves, i) ->
            let newElves = step elves i

            if newElves = elves then
                None
            else
                Some(i + 1, (newElves, i + 1)))
        (initialElves, 0)
    |> Seq.last
    |> (+) 1 // The answer is the first repeated round, which is the last unique + 1
        
let execute =
    part2