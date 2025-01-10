open AdventOfCode_2023.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day08/test_input_01.txt"
//let path = "day08/test_input_02.txt"
let path = "day08/day08_input.txt"

type Node = {
    Root: string
    Left: string
    Right: string
}

   
let rec countSteps (nodes: Node list) (currentNode: Node) (steps: int) (instructions: string array) (iIdx: int) =
    let ins = instructions.[iIdx]
    let toCheck = if ins = "L" then currentNode.Left else currentNode.Right
    let nodes' = nodes |> List.filter(fun n -> n.Root = toCheck)
    match nodes' with
    | [] -> failwith "Unexpected"
    | head :: _ ->
        if head.Root = "ZZZ" then steps + 1
        else
            let steps = steps + 1
            countSteps nodes head steps instructions ((iIdx + 1) % instructions.Length)
        

let execute =
    let lines = GetLinesFromFile path
    let instructions = lines.[0].ToCharArray() |> Array.map string
    let nodes = 
        lines 
        |> Array.skip 2 
        |> Array.map (fun l -> Regex.Matches(l, @"\w+") 
                            |> Seq.toArray)
        |> Array.map (fun m -> { 
                                        Root = m.GetValue(0).ToString(); 
                                        Left = m.GetValue(1).ToString(); 
                                        Right = m.GetValue(2).ToString()
                                        } ) |> Array.toList
    let initNode = nodes |> List.filter(fun n -> n.Root = "AAA") |> List.head
    let result = countSteps nodes initNode 0 instructions 0
    result