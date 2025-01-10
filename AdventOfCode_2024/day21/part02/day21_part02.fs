module day21_part02

open AdventOfCode_2024.Modules
open System.Collections.Generic

type Coord = {
    Row: int
    Col: int
}

type KeySequence = {
    Pos: Coord
    Sequence: string
}

let keypositions = Map.ofList([
    ("7", { Row = 0; Col = 0 });
    ("8", { Row = 0; Col = 1 });
    ("9", { Row = 0; Col = 2 });
    ("4", { Row = 1; Col = 0 });
    ("5", { Row = 1; Col = 1 });
    ("6", { Row = 1; Col = 2 });
    ("1", { Row = 2; Col = 0 });
    ("2", { Row = 2; Col = 1 });
    ("3", { Row = 2; Col = 2 });
    ("NONE", { Row = 3; Col = 0 });
    ("0", { Row = 3; Col = 1 });
    ("A", { Row = 3; Col = 2 })
])

let directions = Map.ofList([
    ("NONE", { Row = 0; Col = 0 });
    ("^", { Row = 0; Col = 1 });
    ("A", { Row = 0; Col = 2 });
    ("<", { Row = 1; Col = 0 });
    ("v", { Row = 1; Col = 1 });
    (">", { Row = 1; Col = 2 })
])

let DiffDirections = Map.ofList([
    ("^", { Row = -1; Col = 0 });
    (">", { Row = 0; Col = 1 });
    ("v", { Row = 1; Col = 0 });
    ("<", { Row = 0; Col = -1 })
])

let inputToSequence (input: Map<string, Coord>) ((start, finish): string * string) : string list =
    let initPos = { Row = input[start].Row; Col = input[start].Col }

    let rec bfs (queue: KeySequence list) (distances: Map<string, int>) (allPaths: string list) =
        match queue with
        | [] -> 
            allPaths |> List.sortBy String.length
        | current :: rest ->
            let memokey = sprintf "%d-%d" current.Pos.Row current.Pos.Col
            let isFinished = 
                current.Pos.Row = input[finish].Row && current.Pos.Col = input[finish].Col

            if isFinished then
                bfs rest distances ((current.Sequence + "A") :: allPaths)
            elif distances |> Map.tryFind memokey |> Option.exists (fun d -> d < current.Sequence.Length) then
                bfs rest distances allPaths
            else
                let neighbors, updatedDistances =
                    DiffDirections
                    |> Map.fold (fun (accNeighbors, accDistances) dir diff ->
                        let pos = { Row = current.Pos.Row + diff.Row; Col = current.Pos.Col + diff.Col }
                        let newMemokey = sprintf "%d-%d" pos.Row pos.Col
                        if input["NONE"].Row = pos.Row && input["NONE"].Col = pos.Col then
                            accNeighbors, accDistances
                        else
                            let matchingButton =
                                input
                                |> Map.tryFindKey (fun _ button -> button.Row = pos.Row && button.Col = pos.Col)

                            match matchingButton with
                            | Some _ ->
                                let newPath = current.Sequence + dir
                                if accDistances |> Map.tryFind newMemokey |> Option.forall (fun d -> d >= newPath.Length) then
                                    let newElement = { Pos = pos; Sequence = newPath }
                                    newElement :: accNeighbors, accDistances |> Map.add newMemokey newPath.Length
                                else
                                    accNeighbors, accDistances
                            | None -> accNeighbors, accDistances
                    ) ([], distances)

                bfs (rest @ neighbors) updatedDistances allPaths

    if start = finish then
        ["A"]
    else
        bfs [{ Pos = initPos; Sequence = "" }] Map.empty []


let rec sequenceOfKeys(input: Map<string, Coord>)((code, robot): string*int) (mymemo: Dictionary<string,int64>): int64 =
    let mutable memokey = sprintf "%s-%d" code robot

    match mymemo.TryGetValue(memokey) with
    | true, value -> value
    | _ ->
        let mutable current = "A"
        let mutable len = 0L
        [0..(code.Length - 1)]
        |> List.iter(fun idx ->
            let moves = inputToSequence input (current, code[idx].ToString())
            if robot = 0 then
                len <- len + ((int64)(moves[0].Length))
            else
                let minlen = 
                    moves
                    |> List.fold (fun acc mov -> min acc (sequenceOfKeys directions (mov, robot - 1) mymemo)) System.Int64.MaxValue

                len <- len + minlen

            current <- code[idx].ToString()        
        )
        mymemo.Add(memokey, len)
        len

let sumOfComplexity (content: string array) (complexitydepth: int) : int64 =
    let rec sumOfComplexityHelper (content: string array) (complexitydepth: int) (index: int) (acc: int64) : int64 =
        if index >= content.Length then
            acc
        else
            let code = content[index]
            let lenb = sequenceOfKeys keypositions (code, complexitydepth) (Dictionary<string, int64>())
            let numValue = int64(code.Substring(0, 3))
            let newAcc = acc + lenb * numValue
            sumOfComplexityHelper content complexitydepth (index + 1) newAcc

    sumOfComplexityHelper content complexitydepth 0 0L    

let execute() =
    let path = "day21/day21_input.txt"

    let content = LocalHelper.GetLinesFromFile path
    sumOfComplexity content 25