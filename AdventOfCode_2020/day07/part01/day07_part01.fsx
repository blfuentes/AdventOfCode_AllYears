open System.IO
open System.Collections.Generic
open System
open AdventOfCode_2020.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules.Helpers
open AdventOfCode_Utilities

//let file = "test_input.txt"
let file = "day07_input.txt"
let path = "day07/" + file
let inputLines = GetLinesFromFile(path)

//let testLine ="light red bags contain 1 bright white bag, 2 muted yellow bags."
//testLine.Replace("bags", "").Replace("bag", "").Split([|"contain"|], StringSplitOptions.None)

//let testLine2 = "1 bright white"
//let element = 
//    match testLine2 with
//    | Regex @"(?<size>\d+)\s(?<color>\w+\s\w+)" [s; c] -> Some { Name = c.Trim(); Size = (s |> int); Content = [||] }
//    | _ -> None

let parseInput (input:string array) =
    seq {
        for line in input do
            let parts = line.Replace("bags", "").Replace("bag", "").Split([|"contain"|], StringSplitOptions.None)
            let content = 
                seq {
                    let contentList = parts.[1].Split(',') |> Array.map (fun x -> x.Trim())
                    for content in contentList do
                        let element = 
                            match content with
                            | Regex @"(?<size>\d+)\s(?<color>\w+\s\w+)" [s; c] -> Some { Name = c.Trim(); Size = (s |> int); Content = [] }
                            | _ -> None
                        if element.IsSome then
                            yield element.Value                        
                } |> List.ofSeq
            let element = 
                Some {
                    Name = parts.[0].Trim();
                    Size = 1;
                    Content = content
                }
            yield element.Value
    }

let rec countContainers (originalBag: ChristmasBag) (childcontainers: ChristmasBag list) (allcontainers: ChristmasBag list) =
    match childcontainers.IsEmpty with
    | true -> false
    | false ->
        match (childcontainers |> List.exists (fun c -> c.Name = originalBag.Name)) with
        | true -> true
        | false -> 
            (childcontainers |> List.map(fun c -> countContainers originalBag ((allcontainers |> List.find(fun s -> s.Name = c.Name)).Content) allcontainers) |> List.exists (fun c -> c))

let elements = parseInput inputLines |> List.ofSeq

let originalBag = { Name = "shiny gold"; Size = 1; Content = [] }

let result = 
    seq {
        for el in elements do
            if countContainers originalBag el.Content elements then
                yield el
    } |> List.ofSeq

result.Length
    