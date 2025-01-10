module day09_part01

open AdventOfCode_2016.Modules
open System.Text.RegularExpressions

let rec processContent (content: string) (currentIdx: int) =
    match currentIdx = content.Length with
    | true -> content.Length
    | false -> 
        let regex = new Regex("\((?<size>\d+)x(?<times>\d+)\)")
        let found = regex.Match( content, currentIdx)
        match found.Success with
        | true ->
            let endIdx = (found.Value.Length + found.Index)
            let size = (int)found.Groups["size"].Value
            let times = (int)found.Groups["times"].Value
            let toRepeat = content.Substring(endIdx, size)
            let repeatedContent = toRepeat |> String.replicate(times)
            let newContent = content.Substring(0, found.Index) + repeatedContent + content.Substring(endIdx + size)
            processContent newContent (currentIdx + size * times)
        | false ->           
            content.Length

let execute =
    let path = "day09/day09_input.txt"
    let content = LocalHelper.GetContentFromFile path
    processContent content 0