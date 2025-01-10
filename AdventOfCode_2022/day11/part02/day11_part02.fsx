open AdventOfCode_2022.Modules.LocalHelper

#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/DataModels.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions
open AoC_2022.Modules.DataModels
open AdventOfCode_Utilities

// let path = "day11/test_input_01.txt"
let path = "day11/day11_input.txt"

let inputLines = GetLinesFromFile(path) |> Array.toList

let parseMonkeyChunk (lines: string list) =
    let newMonkey = {
        Idx = int(lines.Item(0).Split(' ').[1].Replace(":", ""));
        Items = lines.Item(1).Split(':').[1].Split(',') |> Array.map(fun i -> bigint.Parse(i.Trim())) |> Array.toList
        Op = lines.Item(2).Split(' ').[6];
        OpValue1 = lines.Item(2).Split(' ').[5];
        OpValue2 = lines.Item(2).Split(' ').[7];
        TestValue = bigint.Parse(lines.Item(3).Split(' ').[5]);
        TrueThrow = int(lines.Item(4).Split(' ').[9]);
        FalseThrow = int(lines.Item(5).Split(' ').[9]);
        NumberOfInspects = 0I;
    }
    newMonkey

let monkeyChunks = inputLines|> splitWhen(fun x -> x = "")
let monkeys = monkeyChunks |> List.map(fun m -> parseMonkeyChunk m)
let common = monkeys |> List.map(fun m -> m.TestValue) 
let value = List.fold (fun x y -> x * y) 1I common

let calculateWorryLevel(playingMonkey: Monkey) =
    let items = playingMonkey.Items |> List.map(fun i -> 
        let op1 = if playingMonkey.OpValue1 = "old" then i else bigint.Parse(playingMonkey.OpValue1)
        let op2 = if playingMonkey.OpValue2 = "old" then i else bigint.Parse(playingMonkey.OpValue2)
        match playingMonkey.Op with
        | "*" -> (op1 * op2) % value
        | "+" -> (op1 + op2) % value
        | _ -> 0
    )
    items
let playMonkey (playingMonkey: Monkey) (resultMonkeys: Monkey list) =
    if playingMonkey.Items.Length = 0 then
        resultMonkeys
    else 
        let items = calculateWorryLevel playingMonkey
        let throwTrue = items |> List.filter(fun i -> i % playingMonkey.TestValue = 0)
        let throwFalse = items |> List.filter(fun i -> i % playingMonkey.TestValue <> 0)
        let playingIdx = resultMonkeys |> List.findIndex(fun m -> m.Idx = playingMonkey.Idx)
        let replacePlayingMonkey = {
            Idx = playingMonkey.Idx;
            Items = [];
            Op = playingMonkey.Op;
            OpValue1 = playingMonkey.OpValue1;
            OpValue2 = playingMonkey.OpValue2;
            TestValue = playingMonkey.TestValue;
            TrueThrow = playingMonkey.TrueThrow;
            FalseThrow = playingMonkey.FalseThrow;
            NumberOfInspects = playingMonkey.NumberOfInspects + bigint(items.Length);
        }
        let catcherTrueIdx = resultMonkeys |> List.findIndex(fun m -> m.Idx = playingMonkey.TrueThrow)
        let replaceCatcherTrueMonkey = {
            Idx = resultMonkeys.Item(catcherTrueIdx).Idx;
            Items = resultMonkeys.Item(catcherTrueIdx).Items @ throwTrue;
            Op = resultMonkeys.Item(catcherTrueIdx).Op;
            OpValue1 = resultMonkeys.Item(catcherTrueIdx).OpValue1;
            OpValue2 = resultMonkeys.Item(catcherTrueIdx).OpValue2;
            TestValue = resultMonkeys.Item(catcherTrueIdx).TestValue;
            TrueThrow = resultMonkeys.Item(catcherTrueIdx).TrueThrow;
            FalseThrow = resultMonkeys.Item(catcherTrueIdx).FalseThrow;
            NumberOfInspects = resultMonkeys.Item(catcherTrueIdx).NumberOfInspects;
        }
        let catcherFalseIdx = resultMonkeys |> List.findIndex(fun m -> m.Idx = playingMonkey.FalseThrow)
        let replaceCatcherFalseMonkey = {
            Idx = resultMonkeys.Item(catcherFalseIdx).Idx;
            Items = resultMonkeys.Item(catcherFalseIdx).Items @ throwFalse;
            Op = resultMonkeys.Item(catcherFalseIdx).Op;
            OpValue1 = resultMonkeys.Item(catcherFalseIdx).OpValue1;
            OpValue2 = resultMonkeys.Item(catcherFalseIdx).OpValue2;
            TestValue = resultMonkeys.Item(catcherFalseIdx).TestValue;
            TrueThrow = resultMonkeys.Item(catcherFalseIdx).TrueThrow;
            FalseThrow = resultMonkeys.Item(catcherFalseIdx).FalseThrow;
            NumberOfInspects = resultMonkeys.Item(catcherFalseIdx).NumberOfInspects;
        }
        let r1ResultMonkeys = updateElement playingIdx replacePlayingMonkey resultMonkeys
        let r2ResultMonkeys = updateElement catcherTrueIdx replaceCatcherTrueMonkey r1ResultMonkeys 
        let r3ResultMonkeys = updateElement catcherFalseIdx replaceCatcherFalseMonkey r2ResultMonkeys 
        r3ResultMonkeys

let rec round (currentMonkeys: Monkey list) (resultMonkeys: Monkey list) =
    match currentMonkeys with 
    | head :: tail ->
        let newResultMonkeys = playMonkey head resultMonkeys
        let newCurrentMonkeys = newResultMonkeys |> List.filter(fun m -> tail |> List.exists(fun sm -> sm.Idx = m.Idx))
        round newCurrentMonkeys newResultMonkeys
    | [] -> resultMonkeys

let rec ExecuteRound (remainingRounds: int) (resultsMonkeys: Monkey list) =
    printfn "%i rounds left" remainingRounds
    match remainingRounds with
    | 0 -> resultsMonkeys
    | _ -> 
        let newResultMonkeys = round resultsMonkeys resultsMonkeys
        ExecuteRound (remainingRounds - 1) newResultMonkeys


let boringMonkeys = ExecuteRound 10000 monkeys
for mon in boringMonkeys do
    printfn "Monkey %i inspected items %A times." mon.Idx mon.NumberOfInspects
let monkeyBusines = boringMonkeys |> List.sortByDescending(fun m -> m.NumberOfInspects) |> List.take(2)
printfn "Result: %A" (monkeyBusines.Item(0).NumberOfInspects * monkeyBusines.Item(1).NumberOfInspects)



//round monkeys monkeys

//let newMonkeys = playMonkey 0 monkeys.Head monkeys