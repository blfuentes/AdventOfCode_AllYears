open System.IO
open System.Collections.Generic
open System
open AdventOfCode_2020.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Helpers.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_Utilities
open AdventOfCode_2020.Modules.Helpers
open AdventOfCode_Utilities

//let file = "test_input.txt"
let file = "day06_input.txt"
let path = "day06/" + file
let inputLines = GetLinesFromFile(path) |> List.ofArray

let concatStringList (list:string list) =
   seq {
       for l in list do
           yield l.ToCharArray()
   } |> List.ofSeq

let commonElements (input: char array list) =
    let inputAsList = input |> List.map (List.ofArray)
    let inputAsSet = List.map Set.ofList inputAsList
    let elements =  Seq.reduce Set.intersect inputAsSet
    elements

let commonElements2 (input: char array list) =
    input |> List.map (List.ofArray) |> Seq.map Set.ofList |> Set.intersectMany

let testList2 = [[|'a'; 'b'|]; [|'a'; 'c'|]; [|'a'; 'd'|]]
commonElements testList2
commonElements2 testList2  

let answers = getLinesGroupBySeparator2 inputLines ""
answers |> List.map (fun x -> concatStringList x)

let result = answers |> List.map (fun x -> commonElements (concatStringList x)) |> List.map (fun x -> List.length (Set.toList x)) |> List.reduce(+)
let result2 = answers |> List.map (fun x -> commonElements2 (concatStringList x)) |> List.map (fun x -> List.length (Set.toList x)) |> List.reduce(+)
