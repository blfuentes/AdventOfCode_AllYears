module day19_part01

open AdventOfCode_Utilities
open AdventOfCode_2015.Modules

let parseContent(lines: string) =
    let(replacementspart, start) = (lines.Split("\r\n\r\n")[0], lines.Split("\r\n\r\n")[1])
    (replacementspart.Split("\r\n")
    |> Array.map(fun l ->
        (l.Split(" => ")[0], l.Split( " => ")[1])
    ) |> Set.ofArray, start)

let generateReplacements(input: string) ((rinit, rend): string*string) =
    let allIndexes = Utilities.findAllIndexes input rinit
    let replaced = 
        allIndexes
        |> List.map(fun idx ->
            let init = input.Substring(0,idx)
            let ending = input.Substring(idx + rinit.Length)
            init + rend + ending
        )
    replaced

let allReplacements(input: string) (replacements: Set<string*string>) =
    replacements
    |> Seq.map(fun r ->
        generateReplacements input r
    )
    |> Seq.concat
    |> Set.ofSeq

let execute =
    let input = "./day19/day19_input.txt"
    let content = LocalHelper.GetContentFromFile input

    let (replacements, start) = parseContent content
    allReplacements start replacements
    |> Seq.length