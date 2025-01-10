module day03_part02

open AdventOfCode_2024.Modules
open System.Text.RegularExpressions

let solveline (input: string) =
    let pattern = @"mul\(\d+,\d+\)|do\(\)|don't\(\)"
    let matches = Regex.Matches(input, pattern) |> Seq.cast<Match>

    let folder (acc: int * bool) (m': Match) =
        let result, shouldAdd = acc
        let section = input.Substring(m'.Index, m'.Length)

        match section with
        | "do()" ->
            result, true
        | "don't()" ->
            result, false
        | _ when not shouldAdd ->
            result, shouldAdd
        | _ ->
            let parts = section.Replace("mul(", "").Replace(")", "").Split(",")
            let firstValue = int parts.[0]
            let secondValue = int parts.[1]
            result + (firstValue * secondValue), shouldAdd

    let initialState = (0, true)
    let finalResult, _ = Seq.fold folder initialState matches

    finalResult

let execute() =
    let path = "day03/day03_input.txt"
    let content = LocalHelper.GetContentFromFile path
    solveline content
    