module day09_part02

open AdventOfCode_2016.Modules
open System.Text.RegularExpressions
open System.Numerics

let rec processContent (content: string) (currentLength: bigint)=
    let regex = new Regex("\((?<size>\d+)x(?<times>\d+)\)")
    let found = regex.Match( content)
    match found.Success with
    | true ->
        let endIdx = (found.Value.Length + found.Index)
        let size = (int)found.Groups["size"].Value
        let times = BigInteger.Parse(found.Groups["times"].Value)
        currentLength + bigint(found.Index) + 
            times * (processContent (content.Substring(endIdx, size)) 0I) +
                (processContent (content.Substring(endIdx + size)) 0I)

    | false ->           
        bigint(content.Length)


let execute =
    let path = "day09/day09_input.txt"
    let content = (LocalHelper.GetContentFromFile path)
    (processContent content 0I)