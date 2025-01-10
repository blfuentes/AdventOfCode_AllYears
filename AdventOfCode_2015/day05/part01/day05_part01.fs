module day05_part01

open AdventOfCode_2015.Modules.LocalHelper

let path = "day05/day05_input.txt"

let inputLines = GetLinesFromFile(path) |>Seq.toList

let containsThreeVowels (input: string) =
    input.ToCharArray() |> Array.filter(fun c -> c = 'a' || c = 'e' || c = 'i' || c = 'o' || c = 'u') |> Array.countBy(fun c -> c) |> Array.sumBy(fun c -> snd c) >= 3

let twiceInARow (input: string) =
    (['a'..'z'] |> List.filter(fun c -> (input.Contains((string c)+(string c))))).Length > 0

let notBadString (input: string) =
    not (input.Contains("ab")) && not (input.Contains("cd")) && not (input.Contains("pq")) && not (input.Contains("xy"))

let isNiceString (input: string) =
    containsThreeVowels input && twiceInARow input && notBadString input

let execute =
    (inputLines |> List.filter(fun l -> isNiceString l)).Length