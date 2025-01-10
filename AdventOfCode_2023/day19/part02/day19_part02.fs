module day19_part02

open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_2023.Modules

type Op = {
    Category: string
    Comparer: string
    Value: int
    Destination: string
}

type Rule = {
    Id: string
    Ops: Op list
}

type Range = {
    Min: int
    Max: int
}

let parseRule (rule: string) =
    let rgx = @"(\w+){((\w)([<|>])(\d+):(\w+),)*(\w+)}"
    let parts = Regex.Match(rule, rgx).Groups
    let id = parts.[1].Value
    let categories = 
        seq {
            for c in parts.[3].Captures do
                yield c.Value
        } |> Array.ofSeq
    let comparers = 
        seq {
            for c in parts.[4].Captures do
                yield c.Value
        } |> Array.ofSeq
    let values =
        seq {
            for c in parts.[5].Captures do
                yield c.Value
        } |> Array.ofSeq
    let destinations =
        seq {
            for c in parts.[6].Captures do
                yield c.Value
        } |> Array.ofSeq
    let ops =
        seq {
            for idx in 0..destinations.Length - 1 do
                yield { 
                    Category = categories.[idx]; 
                    Comparer = comparers.[idx]; 
                    Value = int values.[idx]; 
                    Destination = destinations.[idx] 
                }
        } |> List.ofSeq
    let ending = {
        Category = "";
        Comparer = "";
        Value = 0;
        Destination = parts.[7].Value
    }

    { Id = id; Ops = ops @ [ending] } 

let parseMessage (input: string) =
    let rgx = @"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}"
    let parts = Regex.Match(input, rgx)
    Map.empty
        .Add("x", int parts.Groups.[1].Value)
        .Add("m", int parts.Groups.[2].Value)
        .Add("a", int parts.Groups.[3].Value)
        .Add("s", int parts.Groups.[4].Value)

let evaluate (a: string) (b: string) (c: int) (msg: Map<string, int>) =
    if b = ">" then
        msg[a] > c
    else
        msg[a] < c

let evaluateBiggerRange (ending: int) (value: int) =
    (ending > value, value + 1)

let evaluateLesserRange (start: int) (value: int) =
    (start < value, value - 1)

let updateMap key map value =
    map |> Map.map (fun k v -> if k = key then value else v)

let rec RunMessages
    (currentRule: Rule) (rules: Map<string, Rule>) 
    (ranges: Map<string, Range>) =

    let getRangeLen (map: Map<string, Range>) =
        map.Values |> Seq.map (fun v -> v.Max - v.Min + 1) |> Seq.map bigint |> Seq.fold (*) 1I

    let mutable mainRanges = ranges
    let mutable acc = 0I
    for opIdx in 0..currentRule.Ops.Length - 2 do
        let op = currentRule.Ops[opIdx]
        let mutable innerRanges = mainRanges
        let update = 
            match op.Comparer with
            | ">" ->
                let (bigger, value) = evaluateBiggerRange mainRanges[op.Category].Max op.Value
                if bigger then
                    let newMin = 
                        if value > innerRanges[op.Category].Min then 
                            value 
                        else 
                            innerRanges[op.Category].Min
                    let newRangeMap = { Min = newMin; Max = innerRanges[op.Category].Max }
                    innerRanges <- updateMap op.Category innerRanges newRangeMap  
                    if op.Destination = "A" then  
                        let l = getRangeLen innerRanges
                        acc <- acc + l
                    else
                        if op.Destination <> "R" then
                            acc <- acc + (RunMessages rules[op.Destination] rules innerRanges)
                (bigger, ">")
            | "<" ->
                let (lesser, value) = evaluateLesserRange mainRanges[op.Category].Min op.Value
                if lesser then
                    let newMax = 
                        if value < innerRanges[op.Category].Max then 
                            value 
                        else 
                            innerRanges[op.Category].Max
                    let newRangeMap = { Min = innerRanges[op.Category].Min; Max = newMax }
                    innerRanges <- updateMap op.Category innerRanges newRangeMap  
                    if op.Destination = "A" then 
                        let l = getRangeLen innerRanges
                        acc <- acc + l
                    else
                        if op.Destination <> "R" then
                            acc <- acc + (RunMessages rules[op.Destination] rules innerRanges)
                (lesser, "<")
            | _ -> (false, "")
        mainRanges <- 
            match update with
            | true, ">" ->
                let newRange = { Min = mainRanges[op.Category].Min; Max = op.Value }
                updateMap op.Category mainRanges newRange
            | true, "<" ->
                let newRange = { Min = op.Value; Max = mainRanges[op.Category].Max }
                updateMap op.Category mainRanges newRange
            | _ -> 
                mainRanges
    let lastOp = currentRule.Ops |> List.rev |> List.head
    match lastOp.Destination with
    | "A" -> 
        let l = getRangeLen mainRanges
        acc + l
    | "R" -> acc
    | _ -> acc + (RunMessages rules[lastOp.Destination] rules mainRanges)

let parseInput (input: string list) =
    let parts = Utilities.getGroupsOnSeparator input ""
    let rules = parts.[0] |> List.map parseRule
    let messages = parts.[1] |> List.map parseMessage
    let rulesMap = 
        rules 
        |> List.map(fun r -> (r.Id, r)) 
        |> Map.ofList
    (rulesMap, messages)

let execute =
    let path = "day19/day19_input.txt"
    let (rules, messages) = parseInput (LocalHelper.GetLinesFromFile path |> List.ofArray)

    let rangesMap =
        Map.empty
            .Add("x", { Min = 1; Max = 4000 })
            .Add("m", { Min = 1; Max = 4000 })
            .Add("a", { Min = 1; Max = 4000 })
            .Add("s", { Min = 1; Max = 4000 })

    let result = RunMessages rules["in"] rules rangesMap
    result