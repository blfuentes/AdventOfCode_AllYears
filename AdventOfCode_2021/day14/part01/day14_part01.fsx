open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic

//#load @"../../Modules/Utilities.fs"
//open AdventOfCode_Utilities

let path = "day14_input.txt"
//let path = "test_input.txt"

let polymertemplate = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.toList

let parts = polymertemplate |> List.splitAt(polymertemplate |> List.findIndex(fun l -> l = ""))
let polymer = (fst parts).Head
let transitions = (snd parts).Tail |> List.map(fun t -> [|t.Split([|" -> "|], StringSplitOptions.None).[0]; t.Split([|" -> "|], StringSplitOptions.None).[1]|])

let buildTransitionReplacement(trans: string[] list) =
    let newTrans =
        seq {
            for idx in 0..trans.Length - 1 do
                yield [|trans.Item(idx).[0]; trans.Item(idx).[1]; (100 + idx).ToString()|]
        }
    newTrans |> Seq.toList

let rec doFirstTransition(input: string, trans: string[] list) =
    match trans with
    | [] -> input
    | x::xs -> 
        let partsOfNewString =
            seq {
                for idx in 0..input.Length - 2 do
                    if input.Substring(idx, 1) + input.Substring(idx + 1, 1) = x.[0] then
                        let tmpString = 
                            match idx with 
                            | 0 -> input.Substring(idx, 1) + x.[2] + input.Substring(idx + 1, 1)
                            | _ -> x.[2] + input.Substring(idx + 1, 1)
                        yield! tmpString
                    else
                        let oldString = 
                            match idx with
                            | 0 -> input.Substring(idx, 1) + input.Substring(idx + 1, 1)
                            | _ -> input.Substring(idx + 1, 1)
                        yield! oldString
            }
        let newString = String.Join("", partsOfNewString |> Seq.toArray)
        doFirstTransition(newString, xs)

let rec doSecondTransition(input: string, trans:string[] list) =
    match trans with
    | [] -> input
    | x::xs -> 
        let newString = input.Replace(x.[2], x.[1])
        doSecondTransition(newString, xs)

let rec performStep(current:int, steps: int, myPolymer: string, trans: string[] list) =
    match current = steps with
    | true -> myPolymer
    | false -> 
        let newInput = doFirstTransition(myPolymer, trans)
        let replacedInput = doSecondTransition(newInput, trans)
        performStep(current + 1, steps, replacedInput, trans)

let replaceTransitions = buildTransitionReplacement(transitions)    
let resultstring = performStep(0, 10, polymer, replaceTransitions)
let counters = resultstring.ToCharArray() |> Array.countBy(fun c -> c) |> Array.toList
let result = (snd((counters |> List.sortByDescending(fun c -> snd c) |> List.head)))
                - (snd ((counters |> List.sortBy(fun c -> snd c) |> List.head)))

let testinput = "NBCCNBBBCBHCB"
"NBCCNBBBCBHCB".Replace("BB", "BAB")
let first = doFirstTransition(testinput, replaceTransitions)