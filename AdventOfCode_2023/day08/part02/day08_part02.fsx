open AdventOfCode_2023.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day08/test_input_01.txt"
//let path = "day08/test_input_02.txt"
//let path = "day08/test_input_03.txt"
let path = "day08/day08_input.txt"

type Node = {
    Root: string
    Left: string
    Right: string
}

   
let rec countSteps (nodes: Node list) (currentNode: Node) (steps: bigint) (instructions: string array) (iIdx: int) (remainingFindings: int) =
    let ins = instructions.[iIdx]
    let toCheck = if ins = "L" then currentNode.Left else currentNode.Right
    let nodes' = nodes |> List.filter(fun n -> n.Root = toCheck)
    match nodes' with
    | [] -> failwith "Unexpected"
    | head :: _ ->
        if head.Root.EndsWith("Z") then
            printfn "Finding %i at %A" remainingFindings (steps + 1I)
            if remainingFindings > 0 then
                countSteps nodes head (steps + 1I) instructions ((iIdx + 1) % instructions.Length) (remainingFindings - 1)
            else
                steps + 1I
        else
            countSteps nodes head (steps + 1I) instructions ((iIdx + 1) % instructions.Length) remainingFindings 
        

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
    let initNodes = nodes |> List.filter(fun n -> n.Root.EndsWith("A"))
    let patterns = initNodes |> List.map(fun n -> countSteps nodes n 0I instructions 0 0)
    listLcmBig patterns