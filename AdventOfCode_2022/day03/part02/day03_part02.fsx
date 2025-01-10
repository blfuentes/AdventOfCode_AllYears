open System.IO
open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities

// let path = "day03/test_input_00.txt"
// let path = "day03/test_input_01.txt"
// let path = "day03/test_input_02.txt"
let path = "day03/day03_input.txt"

let inputLines = GetLinesFromFile(path) |> Array.toList
let groupOfRucksacks = inputLines |> List.chunkBySize 3 |> List.map(fun l -> l |> List.map(fun s -> s.ToCharArray()))

let getValueOfElement (input: char) =
    let minorletters = ['a'..'z'] |> List.mapi(fun idx c -> (c, idx + 1))
    let capitalleters = ['A'..'Z'] |> List.mapi(fun idx c -> (c, idx + 27))
    snd ((minorletters @ capitalleters) |> List.find(fun e -> (fst e) = input))
    
groupOfRucksacks |> List.map Utilities.commonElements |> List.map(fun c -> c |> Seq.toList) |> List.sumBy(fun l -> l |> List.sumBy(fun s -> getValueOfElement s))

// let test = splitString "vJrwpWtwJgWrhcsFMMfFFhFp"

let commonparts = groupOfRucksacks |> 
                    List.map Utilities.commonElements |> 
                    List.map(fun c -> c |> Seq.toList) |> 
                    List.sumBy(fun l -> l |> List.sumBy(fun s -> getValueOfElement s))