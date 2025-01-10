#load @"../../../AdventOfCode_Utilities/Modules/Utilities.fs"
#load @"../../Modules/LocalHelper.fs"

open System
open System.Collections.Generic
open System.Collections.Generic
open System.Text.RegularExpressions
open AdventOfCode_Utilities
open AdventOfCode_2016.Modules
open System.Numerics

let path = "day10/test_input_01.txt"
//let path = "day10/day10_input.txt"

type TargetType =
    | Bot of Id: int
    | Output of Target: int

type Instruction =
    | GiveToBot of BotId: int * Value: int
    | Compare of BotId: int * LowTarget: TargetType * HighTarget: TargetType

type BotInfo = {
    Id: int;
    Microchips: int list;
    Output: int list;
}

let parseInstructions (lines: string array) =
    let pattern = "(?<type>(value|bot)) (?<numbers>\d+)[aA-zZ ]*(?<lowtype>(bot|output)) (?<lowtarget>\d+)(([aA-zZ ]*((?<hightype>(bot|output)) (?<hightarget>\d+)))|)"
    let regex = new Regex(pattern)
    lines
    |> Array.map(fun l -> 
        let found = regex.Match(l)
        match found.Success with
        | false -> failwith "cannot parse line to instruction"
        | true ->
            let ins = found.Groups["type"].Value
            let idValue = (int)found.Groups["numbers"].Value
            let lowtarget = (int)found.Groups["lowtarget"].Value
            if ins = "value" then
                GiveToBot(lowtarget, idValue)
            else
                let lowtype = if found.Groups["lowtype"].Value = "bot" then Bot(lowtarget) else Output(lowtarget)
                let hightarget = (int)found.Groups["hightarget"].Value
                let hightype = if found.Groups["hightype"].Value = "bot" then Bot(hightarget) else Output(hightarget)
                Compare(idValue, lowtype, hightype)
    )

let executeGiveToBot (botid: int, value: int) (bots: BotInfo list) =
    let mutable updated = false
    let updateBot bot =
        if bot.Id = botid && bot.Microchips.Length < 2 then
            updated <- true
            { bot with Microchips = (value :: bot.Microchips) |> List.sort }
        else
            bot
    let found = bots |> List.tryFind(fun b -> b.Id = botid)
    match found with
    | Some(b) ->
        (List.map updateBot bots, updated)
    | None ->
        ({ Id= botid; Microchips = [value]; Output = [] } :: bots, updated)

let executeCompare (botid: int, lowtarget: TargetType, hightarget: TargetType) (bots: BotInfo list) =
    let updateCompareBot bot idgiver idreceiver newvalue =
        if bot.Id = idreceiver then
            { bot with Output = newvalue :: bot.Output }
        else
            if bot.Id = idgiver then
                { bot with Microchips = (bot.Microchips |> List.except([newvalue])) }
            else
                bot
    let giver = bots |> List.find(fun b -> b.Id = botid)
    let lows' =
        match lowtarget with
        | Bot(receiver) ->
            executeGiveToBot (receiver, giver.Microchips[0]) bots
        | Output(target) ->
            (bots |> List.map(fun b -> updateCompareBot b giver.Id target giver.Microchips[0]), true)
    let highs' =
        match hightarget with
        | Bot(receiver) ->
            executeGiveToBot (receiver, giver.Microchips[1]) bots
        | Output(target) ->
            (bots |> List.map(fun b -> updateCompareBot b giver.Id target giver.Microchips[1]), true)    
            
    highs'

let rec runInstructions (instructions: Instruction list) (bots: BotInfo list) (index: int) =
    match instructions with
    | [] ->
        (bots, false)
    | _ ->
        let workingIndex = if index = instructions.Length then 0 else index
        let instruction = instructions[workingIndex]
        match instruction with
        | GiveToBot(botid,value) -> 
            let newbots = executeGiveToBot (botid, value) bots
            runInstructions instructions.Tail (fst newbots) workingIndex
        | Compare(botid, lowtarget, hightarget) -> 
            let giver = bots |> List.tryFind(fun b -> b.Id = botid)
            match giver with
            | Some(g) ->
                if g.Microchips.Length = 2 then
                    let newbots = executeCompare (botid, lowtarget, hightarget) bots
                    runInstructions instructions.Tail (fst newbots) workingIndex
                else
                    runInstructions instructions bots (workingIndex + 1)
            | None ->
                runInstructions instructions bots (workingIndex + 1) 

let lines = LocalHelper.GetLinesFromFile path
let instructions = List.ofArray(parseInstructions lines)
let result = runInstructions instructions [] 0
result