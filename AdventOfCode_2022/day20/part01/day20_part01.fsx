open System
open System.Collections.Generic

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
open AdventOfCode_Utilities

let path = "day20/test_input_01.txt"
// let path = "day20/day20_input.txt"

let values = Utilities.GetLinesFromFile(path) |> List.ofArray



let rec moveElement (currentOrder: (int*int) list) (elements: (int * int) list) =
    match elements with
    | [] -> currentOrder
    | head :: tail ->
        let tmpIdx = fst head + snd head
        let newIdx =
            if tmpIdx > currentOrder.Length then tmpIdx - (currentOrder.Length - fst head)
            elif tmpIdx < 0 then currentOrder.Length + tmpIdx
        let newCurrentOrder = (currentOrder |> List.takeWhile(fun e -> fst e < newIdx)) @ [head] @ (currentOrder |> List.skipWhile(fun e -> fst e < newIdx)) 

let originalList = values |> List.mapi(fun idx e -> (idx, e))