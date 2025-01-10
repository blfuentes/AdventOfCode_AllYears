module day02_part01


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
    let parts = processLine line |> Array.map  int |> Array.sortDescending  
    parts.[0] - parts.[parts.Length - 1]

let execute() =
    let inputLines = GetLinesFromFile(path)
    inputLines |> Array.sumBy calculateCheckSumRow