open System.Text

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System.Collections.Generic
open System.Text.RegularExpressions
open AdventOfCode_Utilities
open AdventOfCode_2016.Modules
open System.Numerics

//let path = "day09/test_input_01.txt"
let path = "day09/day09_input.txt"

let content = (LocalHelper.GetContentFromFile path)

let rec processContent (content: string) (currentLength: int)=
    let regex = new Regex("\((?<size>\d+)x(?<times>\d+)\)")
    let found = regex.Match( content)
    match found.Success with
    | true ->
        let endIdx = (found.Value.Length + found.Index)
        let substring = content.Substring(found.Index, found.Value.Length)
        printfn "starts at %i and ends at %i: %s" found.Index endIdx substring
        let size = (int)found.Groups["size"].Value
        let times = (int)found.Groups["times"].Value
        //let toRepeat = content.Substring(endIdx, size)
        //let repeatedContent = toRepeat |> String.replicate(times)
        //printfn "newContent: %s" repeatedContent
        //let newContent = content.Substring(0, found.Index) + repeatedContent + content.Substring(endIdx + size)
        //printfn "finalContent: %s" newContent
        let toAddLength = size * times
        currentLength + 
            times * 
                (processContent (content.Substring(endIdx, size)) 0)

    | false ->           
        content.Length

    

let content = "ADVENT"
let content = "A(1x5)BC"
let content = "(3x3)XYZ"
let content = "A(2x2)BCD(2x2)EFG"
let content = "(6x1)(1x3)A"
let content = "X(8x2)(3x3)ABCY"
let content = "(27x12)(20x12)(13x14)(7x10)(1x12)A"
let content = "(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN"


(processContent content 0)