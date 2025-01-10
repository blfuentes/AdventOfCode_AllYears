module day07_part02

open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2015.Modules.LocalHelper

type Operation = {
    op1: string
    op2: string
    operator: string
    result: string
}

let AND = (&&&)
let OR = (|||)
let LSHIFT = (<<<)
let RSHIFT = (>>>)
let NOT = (~~~)

let parseInstruction (input: string) =
    let regex = Regex(@"([a-z]+|\d+|AND|OR|LSHIFT|RSHIFT|NOT|\w+)")
    let parts = regex.Matches(input)
    match parts.Count with
    | 2 ->
        {
            op1 = parts.[0].Value
            op2 = ""
            operator = ""
            result = parts.[1].Value
        }
    | 3 ->
        {
            op1 = parts.[1].Value
            op2 = ""
            operator = parts.[0].Value
            result = parts.[2].Value
        }
    | 4 ->
        {
            op1 = parts.[0].Value
            op2 = parts.[2].Value
            operator = parts.[1].Value
            result = parts.[3].Value
        }
    | _ -> failwith "Invalid instruction"

let isNumber (input: string) =
    let regex = Regex(@"\d+")
    regex.IsMatch(input) 

let value (input: string) (currentMap: Dictionary<string, uint16>) =
    if isNumber input then
        uint16 input
    else
        currentMap[input]

let isWired (input: string) (currentMap: Dictionary<string, uint16>) =
    currentMap.ContainsKey(input) || isNumber input

let rec evaluate (valueOfA:uint16) (instructions: Operation list) (currentMap: Dictionary<string, uint16>) =
    match instructions with
    | [] -> currentMap
    | op :: tail ->
        let remainingsIns' =
            match op.operator with
            | "AND" -> 
                if isWired op.op1 currentMap && isWired op.op2 currentMap then
                    currentMap[op.result] <- AND (value op.op1 currentMap) (value op.op2 currentMap)
                    []
                else
                    [op]
            
            | "OR" -> 
                if isWired op.op1 currentMap && isWired op.op2 currentMap then
                    currentMap[op.result] <- OR (value op.op1 currentMap) (value op.op2 currentMap)
                    []
                else
                    [op]
                
            | "LSHIFT" -> 
                if isWired op.op1 currentMap && isWired op.op2 currentMap then
                    currentMap[op.result] <- LSHIFT (value op.op1 currentMap) ((int)(value op.op2 currentMap))
                    []
                else
                    [op]
            
            | "RSHIFT" ->
                if isWired op.op1 currentMap && isWired op.op2 currentMap then
                    currentMap[op.result] <- RSHIFT (value op.op1 currentMap) ((int)(value op.op2 currentMap))
                    []
                else
                    [op]
            
            | "NOT" -> 
                if isWired op.op1 currentMap then
                    currentMap[op.result] <- NOT (value op.op1 currentMap)
                    []
                else
                    [op]  
            | _ ->
                if isWired op.op1 currentMap then
                    currentMap[op.result] <- if valueOfA <> 0us && op.result = "b" then valueOfA else value op.op1 currentMap
                    []
                else
                    [op]

        evaluate valueOfA (tail @ remainingsIns') currentMap

    

let execute =
    let path = "day07/day07_input.txt"

    let lines = GetLinesFromFile path |> List.ofSeq
    let instructions = lines |> List.map parseInstruction

    let valueOfA = evaluate 0us instructions (Dictionary<string, uint16>())
    let valueOfA' = valueOfA["a"]
    let result = evaluate valueOfA' instructions (Dictionary<string, uint16>())
    (int)result["a"]