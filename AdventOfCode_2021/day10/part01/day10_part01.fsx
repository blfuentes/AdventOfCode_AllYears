open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic

let path = "day10_input.txt"
//let path = "test_input.txt"
//let path = "test_input_00.txt"

let inputPartsCollection = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.toList

let pairs = [[|"("; ")"|]; [|"["; "]"|]; [|"{"; "}"|]; [|"<"; ">"|]]

let returnPair (e: string) =
    match e with
    | "(" -> ")"
    | "[" -> "]"
    | "{" -> "}"
    | "<" -> ">"
    | ")" -> "("
    | "]" -> "]"
    | "}" -> "}"
    | ">" -> "<"
    | _ -> ""

let isOpener (e: string)=
    e = "(" || e = "[" || e = "{" || e = "<"

let rec processPart (part: string list, currentStack: Stack<string>) =
    match part with 
    | [] -> ""
    | _ ->
        let openers = part |> List.takeWhile(fun i -> isOpener(i)) 
        openers|> List.iter(fun e -> currentStack.Push(e))
        let closers = part |> List.skip(openers.Length) |> List.takeWhile(fun i -> not (isOpener i))            
        let allTrue:bool[] = closers |> List.map(fun c -> false) |> List.toArray
        for i in [0 .. closers.Length - 1] do
            let o = currentStack.Pop()
            allTrue.[i] <- returnPair(o) = closers.[i]
        match (allTrue |> Array.forall(fun i -> i)) with
        | true -> processPart (part |> List.skip(openers.Length + closers.Length), currentStack)
        | false -> closers.[(allTrue |> Array.findIndex(fun c -> not c))]

let processLine (line:string) =
    let openerStack = new Stack<string>()
    let result = processPart ((line.ToCharArray() |> Array.map string |> Array.toList), openerStack)
    result

let linesResults = inputPartsCollection |> List.map(fun l -> processLine l) |> List.groupBy(fun i -> i)
let calculate (entry: string * string list) =
    match fst entry with
    | ")" -> 3 * (snd entry).Length
    | "]" -> 57 * (snd entry).Length
    | "}" -> 1197 * (snd entry).Length
    | ">" -> 25137 * (snd entry).Length
    | _ -> 0

let value = linesResults |> List.sumBy(fun r -> calculate r)
value

let check1 = "{([(<{}[<>[]}>{[]{[(<()>"
let result1 = processLine check1
let check2 = "[[<[([]))<([[{}[[()]]]"
let result2 = processLine check2
let check3 = "[{[{({}]{}}([{[{{{}}([]"
let result3 = processLine check3
let check4 = "[<(<(<(<{}))><([]([]()"
let result4 = processLine check4
let check5 = "<{([([[(<>()){}]>(<<{{"
let result5 = processLine check5
            