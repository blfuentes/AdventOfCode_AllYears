open System.IO
open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day03/test_input_01.txt"
let path = "day03/day03_input.txt"

let inputLines = GetLinesFromFile(path) |> Array.toList

let splitStringInTwo (str: string) =
    let len = str.Length
    let half = len / 2
    let (left, right) = 
        if len % 2 = 0 then
            // If the length is even, return left and right halves of equal length
            (str.Substring(0, half), str.Substring(half))
        else
            // If the length is odd, return left half with one more character
            (str.Substring(0, half + 1), str.Substring(half + 1))
    [left; right]

let getValueOfElement (input: char) =
    let minorletters = ['a'..'z'] |> List.mapi(fun idx c -> (c, idx + 1))
    let capitalleters = ['A'..'Z'] |> List.mapi(fun idx c -> (c, idx + 27))
    snd ((minorletters @ capitalleters) |> List.find(fun e -> (fst e) = input))
    
// let test = splitString "vJrwpWtwJgWrhcsFMMfFFhFp"

let rucksacks = inputLines |> List.map splitStringInTwo
let commonparts = rucksacks |> 
                    List.map(fun l -> (l |> List.map(fun s -> s.ToCharArray())) |> Utilities.commonElements) |> 
                    List.map(fun c -> c |> Seq.toList) |> 
                    List.sumBy(fun l -> l |> List.sumBy(fun s -> getValueOfElement s))