module day12_part01

open Day12_Helper.Day12_Helper
open AdventOfCode_2023.Modules

type ConditionRecord = { 
    springs: char list
    blocks: int list
}

let parseNums s =
    splitClean ',' s |> Seq.map int |> Seq.toList

let parseLine line =
    match splitClean ' ' line with
    | [| springs; nums |] ->
        { springs = springs  |> System.String.Concat |> Seq.toList
          blocks = nums  |> System.String.Concat |> parseNums }
    | _ -> failwith "parse error"

let rec springCombinations (memo: Map<list<char> * list<int>, bigint> ref) springs blocks =
    let notValid sps bs =
        match sps, bs with
        | s :: rest, bs when List.contains s [ '?'; '.' ] -> springCombinations memo rest bs
        | [], [] -> 1I
        | _ -> 0I

    let rec isValid sps bs =
        match sps, bs with
        | [], (0 :: remBlocks) -> springCombinations memo sps remBlocks
        | s :: rest, 0 :: remBlocks when List.contains s [ '?'; '.' ] -> springCombinations memo rest remBlocks
        | s :: rest, block :: remBlocks when List.contains s [ '?'; '#' ] -> isValid rest (block - 1 :: remBlocks)
        | _ -> 0I

    match Map.tryFind (springs, blocks) memo.Value with
    | Some x -> x
    | None ->
        let total = notValid springs blocks + isValid springs blocks
        memo.Value <- Map.add (springs, blocks) total memo.Value
        total

let lineCombos line =
    springCombinations (ref Map.empty) line.springs line.blocks


let execute =
    let path = "day12/day12_input.txt"    
    LocalHelper.ReadLines path
    |> Seq.map parseLine 
    |> Seq.map lineCombos
    |> Seq.sum