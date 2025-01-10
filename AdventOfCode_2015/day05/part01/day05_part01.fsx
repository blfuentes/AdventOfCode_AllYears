open System.IO

// let path = "test_input_01.txt"
let path = "day05_input.txt"

let inputLines = File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |>Seq.toList

let containsThreeVowels (input: string) =
    input.ToCharArray() |> Array.filter(fun c -> c = 'a' || c = 'e' || c = 'i' || c = 'o' || c = 'u') |> Array.countBy(fun c -> c) |> Array.sumBy(fun c -> snd c) >= 3

let twiceInARow (input: string) =
    (['a'..'z'] |> List.filter(fun c -> (input.Contains((string c)+(string c))))).Length > 0

let notBadString (input: string) =
    not (input.Contains("ab")) && not (input.Contains("cd")) && not (input.Contains("pq")) && not (input.Contains("xy"))

let isNiceString (input: string) =
    containsThreeVowels input && twiceInARow input && notBadString input

isNiceString "ugknbfddgicrmopn"
isNiceString "aaa"
isNiceString "jchzalrnumimnmhp"
isNiceString "haegwjzuvuyypxyu"
isNiceString "dvszwmarrgswjxmb"

(inputLines |> List.filter(fun l -> isNiceString l)).Length