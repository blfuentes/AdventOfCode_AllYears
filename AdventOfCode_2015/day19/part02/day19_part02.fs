module day19_part02

open AdventOfCode_Utilities
open AdventOfCode_2015.Modules

let parseContent(lines: string) =
    let(replacementspart, start) = (lines.Split("\r\n\r\n")[0], lines.Split("\r\n\r\n")[1])
    (replacementspart.Split("\r\n")
    |> Array.map(fun l ->
        (l.Split(" => ")[0], l.Split( " => ")[1])
    ) |> Set.ofArray, start)

let findMindReplacement(medicine: string) (replacements: Set<string*string>) =
    let mutable med = medicine
    let mutable count = 0
    while (med.ToCharArray() |> Array.exists(fun e-> e <> 'e')) do
        for (src, repl) in replacements do
            if med.Contains(repl) then
                med <- Utilities.replaceFirst med repl src
                count <- count + 1
    count

let execute =
    let input = "./day19/day19_input.txt"
    let content = LocalHelper.GetContentFromFile input

    let (replacements, start) = parseContent content
    findMindReplacement start replacements