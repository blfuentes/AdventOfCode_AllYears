open AdventOfCode_2023.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

//let path = "day01/test_input_02.txt"
let path = "day01/day01_input.txt"

let lines = GetLinesFromFile path

let convertDigit (input: string) =
    match input with
    | "one" -> 1
    | "two" -> 2
    | "three" -> 3
    | "four" -> 4
    | "five" -> 5
    | "six" -> 6
    | "seven" -> 7
    | "eight" -> 8
    | "nine" -> 9
    | _ -> (int)input

let findNumbers input =
    let firstNumber = Regex.Match(input, @"\d|one|two|three|four|five|six|seven|eight|nine").Value
    let secondNumber = Regex.Match(input, @"\d|one|two|three|four|five|six|seven|eight|nine", RegexOptions.RightToLeft).Value
    convertDigit(firstNumber) * 10 + convertDigit(secondNumber)


//let check = getFirstAndLast "six7sixqrdfive3twonehsk" validDigits
lines |> Array.sumBy findNumbers