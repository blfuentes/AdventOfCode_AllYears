module day08_part02

open AdventOfCode_2015.Modules

type StringData = {
    numOfChars: int
    numOfDataChars: int
}

let countCharsData(input: string) =
    let specialChars = input.ToCharArray() |> Array.filter (fun c -> c = '\\' || c = '"')
    input.Length + specialChars.Length + 2

let parseInput (input: string list) =
    let stringDatas =
        seq {
            for line in input do
                let nChars = line.Length
                yield {
                    numOfChars = nChars
                    numOfDataChars = countCharsData line
                }
        }
    stringDatas |> List.ofSeq

let execute =
    let path = "day08/day08_input.txt"
    let lines = LocalHelper.GetLinesFromFile path |> List.ofSeq
    let codes = parseInput lines
    codes |> List.sumBy (fun c ->c.numOfDataChars - c.numOfChars)