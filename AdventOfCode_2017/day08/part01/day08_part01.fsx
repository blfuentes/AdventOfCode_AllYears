#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open AdventOfCode_2017.Modules

open AdventOfCode_Utilities

type Comparison =
    | Greater
    | Lesser
    | Equal
    | NotEqual
    | EqualOrGreater
    | EquealOrLesser

type SelectorType = {
    Element: string;
    Operation: Comparison
    ToCompare: int
}

type Op =
    | Inc
    | Dec

type Instruction = {
    Target: string;
    Value: int;
    Operation: Op;
    Selector: SelectorType;
}

let parseLines (lines: string array) : (Instruction list*(string*int) list) =
    let registries = Set.empty
    let parseLine (l: string) =
        let parts = l.Split(" ")
        let target = parts[0]
        let opValue = (int)parts[2]
        let sel = parts[4]
        let comp = parts[5]
        let toComp = (int)parts[6]

        let op = if parts[1] = "inc" then Inc else Dec
        let selector = 
            match comp with
            | ">" -> { Element = sel; Operation = Greater; ToCompare = toComp }
            | "<" -> { Element = sel; Operation = Lesser; ToCompare = toComp }
            | "==" -> { Element = sel; Operation = Equal; ToCompare = toComp }
            | "!=" -> { Element = sel; Operation = NotEqual; ToCompare = toComp }
            | ">=" -> { Element = sel; Operation = EqualOrGreater; ToCompare = toComp }
            | "<=" -> { Element = sel; Operation = EquealOrLesser; ToCompare = toComp }
            | _ -> failwith "operation not found"
        registries.Add((target, 0)).Add(sel, 0) |> ignore
        { Target = target; Value = opValue; Operation = op; Selector = selector }
    (List.ofArray (lines |> Array.map parseLine), registries |> Set.toList)

let execute op v1 v2 =
    match op with
    | Inc -> v1 + v2
    | Dec -> v1 - v2

let rec runInstructions (instructions: Instruction list) (registry: (string*int) list) =
    match instructions with
    | [] -> registry
    | instruction :: rest ->
        let comparer = registry |> List.find(fun (r, v) -> r = instruction.Selector.Element)
        let tmpresult = registry |> List.find (fun (r, v) -> r = instruction.Target)
        let newResult =
            (fst tmpresult, 
                match instruction.Selector.Operation with
                | Greater ->
                    if (snd comparer) > instruction.Selector.ToCompare then
                        execute instruction.Operation (snd tmpresult) instruction.Value
                    else
                        instruction.Value
                | Lesser ->
                    if (snd comparer) < instruction.Selector.ToCompare then
                        execute instruction.Operation (snd tmpresult) instruction.Value
                    else
                        instruction.Value
                | Equal ->
                    if (snd comparer) = instruction.Selector.ToCompare then
                        execute instruction.Operation (snd tmpresult) instruction.Value
                    else
                        instruction.Value
                | NotEqual ->
                    if (snd comparer) <> instruction.Selector.ToCompare then
                        execute instruction.Operation (snd tmpresult) instruction.Value
                    else
                        instruction.Value
                | EqualOrGreater ->
                    if (snd comparer) >= instruction.Selector.ToCompare then
                        execute instruction.Operation (snd tmpresult) instruction.Value
                    else
                        instruction.Value
                | EquealOrLesser ->
                    if (snd comparer) <= instruction.Selector.ToCompare then
                        execute instruction.Operation (snd tmpresult) instruction.Value
                    else
                        instruction.Value
            )
        newResult :: List.except ([tmpresult]) registry

let path = "day08/test_input_01.txt"
//let path = "day08/day08_input.txt"

let lines = LocalHelper.GetLinesFromFile path
let (instructions, initialvalues) = parseLines lines
//runInstructions instructions initialvalues