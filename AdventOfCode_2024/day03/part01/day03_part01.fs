module day03_part01

open AdventOfCode_2024.Modules
open System.Text.RegularExpressions

let parsecontent(lines: string array) =
    let parts =
        lines 
        |> Array.map(fun line ->
            let regex = @"mul\(\d+,\d+\)"
            let matches = Regex.Matches(line, regex)
            match matches.Count with
            | 0 -> [(0,0)]
            | _ ->
                let elems =
                    seq {
                        for m in matches do
                            let tosplit = m.Value.Replace("mul(", "").Replace(")", "")
                            yield ((int)(tosplit.Split(",")[0]), (int)(tosplit.Split(",")[1]))
                    }
                elems |> List.ofSeq
        )
    parts |> List.ofArray
  
let calculateLine(part: (int*int) list) =
    part
    |> List.map(fun (a, b) -> a*b)
    |> List.reduce (+)

let execute() =
    let path = "day03/day03_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let elements = parsecontent content
    elements
    |> List.map calculateLine
    |> List.sum