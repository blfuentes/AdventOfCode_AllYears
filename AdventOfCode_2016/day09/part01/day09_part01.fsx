#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System.Text.RegularExpressions
open AdventOfCode_Utilities
open AdventOfCode_2016.Modules

//let path = "day09/test_input_01.txt"
let path = "day09/day09_input.txt"

let content = (LocalHelper.GetContentFromFile path).ToCharArray() |> Array.map string

let rec processContent (content: string array) (currentIdx: int) =
    match currentIdx = content.Length with
    | true -> content.Length
    | false -> 
        let regex = new Regex("\((?<size>\d+)x(?<times>\d+)\)")
        let tmpContent = (String.concat "" content)
        let found = regex.Match( tmpContent, currentIdx)
        match found.Success with
        | true ->
            let endIdx = (found.Value.Length + found.Index)
            let substring = tmpContent.Substring(found.Index, found.Value.Length)
            printfn "starts at %i and ends at %i: %s" found.Index endIdx substring
            let size = (int)found.Groups["size"].Value
            let times = (int)found.Groups["times"].Value
            let toRepeat = tmpContent.Substring(endIdx, size)
            let repeatedContent = toRepeat |> String.replicate(times)
            //printfn "newContent: %s" repeatedContent
            let newContent = tmpContent.Substring(0, found.Index) + repeatedContent + tmpContent.Substring(endIdx + size)
            //printfn "finalContent: %s" newContent
            processContent (newContent.ToCharArray() |> Array.map string) (currentIdx + size * times)
        | false ->           
            content.Length

//let content = "ADVENT".ToCharArray() |> Array.map string
//let content = "A(1x5)BC".ToCharArray() |> Array.map string
//let content = "(3x3)XYZ".ToCharArray() |> Array.map string
//let content = "A(2x2)BCD(2x2)EFG".ToCharArray() |> Array.map string
//let content = "(6x1)(1x3)A".ToCharArray() |> Array.map string
//let content = "X(8x2)(3x3)ABCY".ToCharArray() |> Array.map string


processContent content 0