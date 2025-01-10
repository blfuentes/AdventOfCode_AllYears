module Utilities_deprecated

open System.IO
open System.Text.RegularExpressions


let GetLinesFromFile(path: string) =
    File.ReadAllLines(__SOURCE_DIRECTORY__ + @"../../" + path)

let GetLinesFromFile2(path: string) =
    File.ReadAllLines(path)

let rec combination (num, list: 'a list) : 'a list list = 
    match num, list with
    | 0, _ -> [[]]
    | _, [] -> []
    | k, (x::xs) -> List.map ((@) [x]) (combination ((k-1), xs)) @ (combination (k, xs))

let possibleCombinations (combSize: int) (mainList: uint64 list) =
    seq {
        for init in mainList do
            let index = mainList |> List.findIndex(fun e -> e = init)
            if mainList.Length - combSize > index then
                yield mainList |> List.skip(index) |> List.take(combSize)
    } |> List.ofSeq

let getLinesGroupBySeparator (inputLines: string list) (separator: string) =
    let complete = 
        seq {
            for line in inputLines do
                yield! line.Split(' ')
        } |> List.ofSeq
    let folder (a) (cur, acc) = 
        match a with
        | _ when a <> separator -> a::cur, acc
        | _ -> [], cur::acc
    
    let result = List.foldBack folder (complete) ([List.last complete], []) 
    (fst result)::(snd result)


let getLinesGroupBySeparator2 (inputLines: string list) (separator: string) =
    let complete = 
        seq {
            for line in inputLines do
                yield! line.Split(' ')
        } |> List.ofSeq
    let folder (a) (cur, acc) = 
        match a with
        | _ when a <> separator -> a::cur, acc
        | _ -> [], cur::acc
        
    let result = List.foldBack folder (complete) ([], [])
    (fst result)::(snd result)

let folder (a) (cur, acc) = 
    match a with
    | _ when a <> 0 -> a::cur, acc
    | _ -> [], cur::acc

let split lst =
    let result = List.foldBack folder (lst) ([], [])
    (fst result)::(snd result)

let updateElement index element list = 
  list |> List.mapi (fun i v -> if i = index then element else v)

// XOR OPERATOR
let (^@) (a: bool) (b:bool) : bool =
    a <> b

let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None

