module day19_part01

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

let rec RunMessages (currentRule: Rule) (rules: Map<string, Rule>) (message: Map<string, int>) =
    let rec runOps (ops: Op list) =
        match ops with
        | hop :: top ->
            let dest = 
                if hop.Category = "" then
                    hop.Destination
                else
                    if evaluate hop.Category hop.Comparer hop.Value message then
                        hop.Destination
                    else
                        ""
            if dest = "" then
                runOps top
            else
                match dest with
                | "A" -> ("A", message)
                | "R" -> ("R", message)
                | _ ->
                    let newRule = rules[dest]
                    RunMessages newRule rules message                                                 
        | [] -> failwith "Unexpected"
    runOps currentRule.Ops
    

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
    //let path = "day19/test_input_01.txt"
    //let path = "day19/test_input_02.txt"
    let path = "day19/day19_input.txt"
    let (rules, messages) = parseInput (LocalHelper.GetLinesFromFile path |> List.ofArray)
    let results = messages |> List.map (fun m -> RunMessages (rules["in"]) rules m )
    let accepted = 
        results 
        |> List.filter(fun r -> (fst r) = "A") 
        |> List.map(fun r -> (snd r).Values |> Seq.sum) 
        |> List.sum
    accepted