module day02_part02


open AdventOfCode_Utilities
open AdventOfCode_2017.Modules.LocalHelper

let path = "day02/day02_input.txt"

let processLine (line: string) =
    let values = 
        seq {
            for c in line.ToCharArray() do
                if ['0'..'9'] |> List.contains c then
                    yield c
                else
                    yield! " "
        } |> List.ofSeq
    System.String.Concat(values).Split(' ')

let calculateCheckSumRow (line: string) =
    let possible = comb 2 ((processLine line) |> List.ofArray)
    let found = possible |> List.find(fun el -> int(el.Item(0)) % int(el.Item(1)) = 0 || int(el.Item(1)) % int(el.Item(0)) = 0)
    if int(found.Item(0)) % int(found.Item(1)) = 0 then int(found.Item(0)) / int(found.Item(1)) else int(found.Item(1)) / int(found.[0])

let execute() =
    let inputLines = GetLinesFromFile(path)
    inputLines |> Array.sumBy calculateCheckSumRow