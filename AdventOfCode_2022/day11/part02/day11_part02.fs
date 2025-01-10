module day11_part02

open System
open AdventOfCode_Utilities
open AoC_2022.Modules
open AdventOfCode_2022.Modules.LocalHelper

let path = "day11/day11_input.txt"

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

let calculateWorryLevel(playingMonkey: Monkey) (reducer: bigint)=
    let items = playingMonkey.Items |> List.map(fun i -> 
        let op1 = if playingMonkey.OpValue1 = "old" then i else bigint.Parse(playingMonkey.OpValue1)
        let op2 = if playingMonkey.OpValue2 = "old" then i else bigint.Parse(playingMonkey.OpValue2)
        match playingMonkey.Op with
        | "*" -> (op1 * op2) % reducer
        | "+" -> (op1 + op2) % reducer
        | _ -> 0I
    )
    items
let playMonkey (playingMonkey: Monkey) (resultMonkeys: Monkey list) (reducer: bigint)=
    if playingMonkey.Items.Length = 0 then
        resultMonkeys
    else 
        let items = calculateWorryLevel playingMonkey reducer
        let throwTrue = items |> List.filter(fun i -> i % playingMonkey.TestValue = 0I)
        let throwFalse = items |> List.filter(fun i -> i % playingMonkey.TestValue <> 0I)
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

let rec round (currentMonkeys: Monkey list) (resultMonkeys: Monkey list) (reducer: bigint)=
    match currentMonkeys with 
    | head :: tail ->
        let newResultMonkeys = playMonkey head resultMonkeys reducer
        let newCurrentMonkeys = newResultMonkeys |> List.filter(fun m -> tail |> List.exists(fun sm -> sm.Idx = m.Idx))
        round newCurrentMonkeys newResultMonkeys reducer
    | [] -> resultMonkeys

let rec ExecuteRound (remainingRounds: int) (resultsMonkeys: Monkey list) (reducer: bigint)=
    match remainingRounds with
    | 0 -> resultsMonkeys
    | _ -> 
        let newResultMonkeys = round resultsMonkeys resultsMonkeys reducer
        ExecuteRound (remainingRounds - 1) newResultMonkeys reducer

let execute =
    let inputLines = GetLinesFromFile(path) |> Seq.toList
    let monkeyChunks = inputLines|> splitWhen(fun x -> x = "")
    let monkeys = monkeyChunks |> List.map(fun m -> parseMonkeyChunk m)
    let common = monkeys |> List.map(fun m -> m.TestValue) 
    let value = List.fold (fun x y -> x * y) 1I common
    let boringMonkeys = ExecuteRound 10000 monkeys value
    let monkeyBusines = boringMonkeys |> List.sortByDescending(fun m -> m.NumberOfInspects) |> List.take(2)
    (monkeyBusines.Item(0).NumberOfInspects * monkeyBusines.Item(1).NumberOfInspects)
    