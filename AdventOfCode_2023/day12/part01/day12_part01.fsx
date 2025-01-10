#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../../AdventOfCode_2023/Modules/LocalHelper.fs"
open System
open System.Collections.Generic

open AdventOfCode_2023.Modules
open AdventOfCode_Utilities

type ConditionRecord = {
    mask: char array
    definition: int array
}

let parseRecordEntry (entry: string) =
    let parts = entry.Split " "
    let mask = parts.[0].ToCharArray()
    let definition = parts.[1].Split(",") |> Array.map int
    { definition = definition; mask = mask }

let rec matchesDefinition (mask: char array) (testcase: char array) =
    match testcase.Length with
    | 0 -> true
    | _ ->
        if mask.[0] = testcase.[0] || mask.[0] = '?' then
            matchesDefinition (mask.[1..]) (testcase.[1..])
        else
            false

let generatePart (index: int) (comb: int list) (conRecord: ConditionRecord) =
    let generalPart = String.replicate conRecord.definition.[index] "#"
    let splitPart = String.replicate comb.[index + 1] "."
    generalPart + splitPart

let rec generateCombinations (conRecord: ConditionRecord) (possibleSplits: int list list) (maxLength: int) (combinations: char array list)=
    match possibleSplits with
    | comb :: tail ->
        let start = String.replicate comb.[0] "."

        let parts =
            seq {
                for idx in 0..conRecord.definition.Length - 1 do
                    yield generatePart idx comb conRecord
            } |> String.concat ""
        let newDefinition = (start + parts).ToCharArray()
        let combinations' =
            if matchesDefinition conRecord.mask newDefinition then
                combinations @ [newDefinition]
            else
                combinations
        generateCombinations conRecord tail maxLength combinations'
    | [] -> 
        combinations

let getAllCombinations (lists: int list list) =
    combinationsOfLists lists

let countPossible (conRecord: ConditionRecord) (separators: int list list) =
    let maxLength = conRecord.mask.Length
    //let valuesLength = conRecord.definition |> Array.sum
    //let possibleSplitCombinations = separators |> List.filter(fun l -> (l |> List.sum) + valuesLength = maxLength)
    let validCombinations = generateCombinations conRecord separators maxLength []
    printfn "Combinations for %A" conRecord
    //validCombinations |> List.iter(fun c -> printfn "%s" (String.Join("", c)))
    validCombinations.Length

//let sample = "???.### 1,1,3"

//matchesDefinition("???.###".ToCharArray()) ("#.#.###.".ToCharArray())

//let conRecord = parseRecordEntry sample
//let separators = getAllCombinations [[0..10];[1..10]; [1..10]; [0..10]]
//let result = countPossible conRecord separators

//let getAuxLists (numberOfLists: int) =
//    List.replicate (numberOfLists - 1) [1..13]

let getAuxLists (numberOfLists: int) (maskl: int)=
    List.replicate (numberOfLists - 1) [1..(maskl - numberOfLists)]

let execute =
    let path = "day12/test_input_01.txt"
    //let path = "day12/test_input_02.txt"
    //let path = "day12/day12_input.txt"    
    let condRecords = LocalHelper.ReadLines path |> Seq.map parseRecordEntry |> List.ofSeq
    let diffCondRecords = condRecords |> List.groupBy _.definition.Length 
    let perms = new Dictionary<(int*int), int list list>()
    for defLength in diffCondRecords do
        for cr in (snd defLength) do
            let numOfLists = (fst defLength)
            let maskl = cr.mask.Length
            printfn "Generating perms for mask of %i and %i elements" (fst defLength) maskl
            let auxLists = getAuxLists numOfLists maskl
            let allLists = [[0..(maskl - numOfLists)]] @ auxLists @ [[0..(maskl - numOfLists)]]
            let allperms = getAllCombinations allLists
            perms.Add((fst defLength, cr.mask.Length), allperms)

    printfn "Starting the computation"
    let result = 
        condRecords |> Seq.map(fun conRecord -> 
            let maxLength = conRecord.mask.Length
            let valuesLength = conRecord.definition |> Array.sum
            let firstParsing = perms[(conRecord.definition.Length, conRecord.mask.Length)]
            let secondParsing = firstParsing |> List.filter (fun p -> (p |> List.sum) + valuesLength = maxLength)
            countPossible conRecord secondParsing
        ) |> Seq.sum
    result
execute