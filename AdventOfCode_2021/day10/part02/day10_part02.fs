module day10_part02

open System
open System.IO
open System.Collections.Generic

let path = "day10_input.txt"
//let path = "test_input.txt"
//let path = "test_input_00.txt"

let inputPartsCollection = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.toList

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
    | [] -> (true, currentStack)
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
        | false -> (false, currentStack)

let processLine (line:string) =
    let openerStack = new Stack<string>()
    let result = processPart ((line.ToCharArray() |> Array.map string |> Array.toList), openerStack)
    result

let rec getValue(queue: Queue<string>, value: bigint) =
    match queue.Count = 0 with
    | true -> value
    | false -> 
        let newElement = queue.Dequeue()
        match newElement with
        | ")" -> getValue(queue, (5I * value + 1I))
        | "]" -> getValue(queue, (5I * value + 2I))
        | "}" -> getValue(queue, (5I * value + 3I))
        | ">" -> getValue(queue, (5I * value + 4I))
        | _ -> 0I

let calculateStackStore(stack: Stack<string>) =
    let completionQueue = new Queue<string>()
    while stack.Count > 0 do completionQueue.Enqueue(returnPair(stack.Pop()))
    getValue(completionQueue, 0I)

let execute =
    let linesResults = inputPartsCollection |> List.map(fun l -> processLine l) |> List.filter(fun r -> fst r)
    let completionParts = linesResults |> List.map(fun p -> calculateStackStore (snd p))
    completionParts |> List.sort |> List.item(completionParts.Length / 2)