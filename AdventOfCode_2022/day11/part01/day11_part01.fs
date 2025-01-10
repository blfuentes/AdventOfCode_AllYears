module day11_part01

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

let rec playMonkey (playingMonkey: Monkey) (resultMonkeys: Monkey list) =
    match playingMonkey.Items with
    | head :: tail ->
        let op1 = if playingMonkey.OpValue1 = "old" then head else bigint.Parse(playingMonkey.OpValue1)
        let op2 = if playingMonkey.OpValue2 = "old" then head else bigint.Parse(playingMonkey.OpValue2)
        let inspectWorryLevel = 
            match playingMonkey.Op with
            | "*" -> op1 * op2
            | "+" -> op1 + op2
            | _ -> 0I
        let newWorryLevel = inspectWorryLevel / 3I;
        let catcherMonkeyIdx =
            match (newWorryLevel % playingMonkey.TestValue) = 0I with
            | true -> playingMonkey.TrueThrow
            | false -> playingMonkey.FalseThrow

        let playingIdx = resultMonkeys |> List.findIndex(fun m -> m.Idx = playingMonkey.Idx)
        let replacePlayingMonkey = {
            Idx = playingMonkey.Idx;
            Items = playingMonkey.Items.Tail;
            Op = playingMonkey.Op;
            OpValue1 = playingMonkey.OpValue1;
            OpValue2 = playingMonkey.OpValue2;
            TestValue = playingMonkey.TestValue;
            TrueThrow = playingMonkey.TrueThrow;
            FalseThrow = playingMonkey.FalseThrow;
            NumberOfInspects = playingMonkey.NumberOfInspects + 1I;
        }
        let catcherIdx = resultMonkeys |> List.findIndex(fun m -> m.Idx = catcherMonkeyIdx)
        let replaceCatcherMonkey = {
            Idx = resultMonkeys.Item(catcherIdx).Idx;
            Items = resultMonkeys.Item(catcherIdx).Items @ [newWorryLevel];
            Op = resultMonkeys.Item(catcherIdx).Op;
            OpValue1 = resultMonkeys.Item(catcherIdx).OpValue1;
            OpValue2 = resultMonkeys.Item(catcherIdx).OpValue2;
            TestValue = resultMonkeys.Item(catcherIdx).TestValue;
            TrueThrow = resultMonkeys.Item(catcherIdx).TrueThrow;
            FalseThrow = resultMonkeys.Item(catcherIdx).FalseThrow;
            NumberOfInspects = resultMonkeys.Item(catcherIdx).NumberOfInspects;
        }
        let r1ResultMonkeys = updateElement playingIdx replacePlayingMonkey resultMonkeys
        let r2ResultMonkeys = updateElement catcherIdx replaceCatcherMonkey r1ResultMonkeys 
        playMonkey replacePlayingMonkey r2ResultMonkeys
    | [] -> resultMonkeys

let rec round (currentMonkeys: Monkey list) (resultMonkeys: Monkey list) =
    match currentMonkeys with 
    | head :: tail ->
        let newResultMonkeys = playMonkey head resultMonkeys
        let newCurrentMonkeys = newResultMonkeys |> List.filter(fun m -> tail |> List.exists(fun sm -> sm.Idx = m.Idx))
        round newCurrentMonkeys newResultMonkeys
    | [] -> resultMonkeys

let rec ExecuteRound (remainingRounds: int) (resultsMonkeys: Monkey list) =
    match remainingRounds with
    | 0 -> resultsMonkeys
    | _ -> 
        let newResultMonkeys = round resultsMonkeys resultsMonkeys
        ExecuteRound (remainingRounds - 1) newResultMonkeys

let execute =
    let inputLines = GetLinesFromFile(path) |> Seq.toList
    let monkeyChunks = inputLines|> splitWhen(fun x -> x = "")
    let monkeys = monkeyChunks |> List.map(fun m -> parseMonkeyChunk m)
    let boringMonkeys = ExecuteRound 20 monkeys
    let monkeyBusines = boringMonkeys |> List.sortByDescending(fun m -> m.NumberOfInspects) |> List.take(2)
    monkeyBusines.Item(0).NumberOfInspects * monkeyBusines.Item(1).NumberOfInspects 