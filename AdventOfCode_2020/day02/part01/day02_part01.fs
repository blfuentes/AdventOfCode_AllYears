module day02_part01

open AdventOfCode_Utilities
open CustomDataTypes
open AdventOfCode_2020.Modules


let path = "day02/day02_input.txt"
let inputLines = List.ofArray <| LocalHelper.GetLinesFromFile(path)

let extract l =
    match l with
    | Regex @"(?<min>\d+)-(?<max>\d+) (?<elem>\w): (?<code>\w+)" [m; M; e; c] -> Some {min = m |> int; max = M |> int; element = e; code = c }
    | _ -> None

let inputCheck = inputLines |> List.map extract
 
let passwordIsValid(check: PasswordPolicy) : bool =
    let numberOfElements = check.code |> List.ofSeq |> List.map string |> List.filter (fun a -> a = check.element) |> List.length
    numberOfElements >= check.min && numberOfElements <= check.max

let execute =
    inputCheck |> List.filter (fun check -> passwordIsValid check.Value) |> List.length