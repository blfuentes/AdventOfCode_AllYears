module day08_part01

open System
open System.Text.RegularExpressions
open AdventOfCode_2015.Modules
open System.Globalization

type StringData = {
    numOfChars: int
    numOfDataChars: int
}

let rec replaceHex(original: string) (hexValues: string list) =
    match hexValues with
    | [] -> original
    | head::tail ->
        let replaced = Int32.Parse(head.Substring(2, 2), NumberStyles.AllowHexSpecifier) |> char |> string
        let escapedQuotes = original.Replace(head, replaced)
        replaceHex escapedQuotes tail

let countCharsData(input: string) =
    let oldq = $"\\\""
    let replacedq = $"\""
    let escapedQuotes = input.Replace(oldq, replacedq).Replace("\\\\", "\\")
    let hexValuesregex = "\\\x[0-9a-fA-F]{2}"
    if escapedQuotes.Contains("\\x") then
        let foundValues = Regex.Matches(escapedQuotes, hexValuesregex)
        if foundValues.Count > 0 then
            let hexValues = foundValues |> Seq.cast<Match> |> Seq.map (fun m -> m.Value)
            let replacedHex = replaceHex escapedQuotes (hexValues |> List.ofSeq)
            replacedHex.Length - 2
        else
            escapedQuotes.Length - 2
    else
        escapedQuotes.Length - 2

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
    codes |> List.sumBy (fun c -> c.numOfChars - c.numOfDataChars)