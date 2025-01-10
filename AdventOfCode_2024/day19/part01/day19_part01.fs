module day19_part01

open AdventOfCode_2024.Modules
open System.Collections.Generic

let parseContent(lines: string) =
    let patterns = (lines.Split("\r\n\r\n")[0]).Split(", ")|> Set.ofArray
    let designs = (lines.Split("\r\n\r\n")[1]).Split("\r\n")
    (patterns, designs)

let checkIfValid (design: string) (patterns: Set<string>) maxSize (memo: Dictionary<string, bool>) = 
    let rec canBeDone (patterns: Set<string>) designpart =
        match memo.TryGetValue(designpart) with
        | found, value when found -> value
        | _ ->
           match designpart with
           | "" -> true
           | _ ->
                let minlength = min maxSize designpart.Length

                let found = 
                    [0..minlength]
                    |> Seq.map(fun l -> designpart.Substring(0, l))
                    |> Seq.filter(fun subpart -> Set.contains subpart patterns)
                    |> Seq.exists(fun subpart -> canBeDone patterns (designpart.Substring(subpart.Length)))
                
                memo.Add(designpart, found)
                found

    canBeDone patterns design

let execute() =
    let path = "day19/day19_input.txt"

    let content = LocalHelper.GetContentFromFile path
    let (patterns, designs) = parseContent content
    let maxPatternSize = 
        patterns
        |> Seq.maxBy (fun pattern -> pattern.Length)
        |> fun pattern -> pattern.Length

    let memo = Dictionary<string, bool>()
    designs
    |> Array.sumBy(fun d ->
        if checkIfValid d patterns maxPatternSize memo then 1 else 0
    )