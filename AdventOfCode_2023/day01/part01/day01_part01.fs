module day01_part01

open System.Text.RegularExpressions

open AdventOfCode_2023.Modules.LocalHelper

let path = "day01/day01_input.txt"

let findNumbers input =
    let matches = Regex.Matches(input, @"\d")
    matches |> Seq.cast<Match> |> Seq.map(fun m -> m.Value) |> Seq.toList

let getNumbersFromLine(line: string) =
    let numbers = findNumbers line
    if numbers.Length > 0 then (int)(numbers.Head + numbers.Item(numbers.Length - 1))
    else 0

let execute =
    let lines = GetLinesFromFile path
    Array.sumBy getNumbersFromLine lines