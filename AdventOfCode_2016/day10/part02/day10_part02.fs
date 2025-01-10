module day10_part02

open AdventOfCode_2016.Modules
open System.Text.RegularExpressions
open System.Collections.Generic

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

let executeGiveToBot(botid: int, value: int) (bots: Dictionary<int, BotInfo>) =
    if bots.ContainsKey botid then
        bots[botid] <- { bots[botid] with Microchips = (value :: bots[botid].Microchips) |> List.sort }
    else
        bots.Add(botid, { Id = botid; Microchips = [value]; Output = [] })

let executeCompare (botid: int, lowtarget: TargetType, hightarget: TargetType) (bots: Dictionary<int, BotInfo>) =
    match lowtarget with
    | Bot(id) ->
        bots[id] <- { bots[id] with Microchips = (bots[botid].Microchips[0] :: bots[id].Microchips) |> List.sort }
    | _ -> ignore()
    match lowtarget with
    | Output(target) ->
        bots[target] <- {bots[target]  with Output = (bots[botid].Microchips[0] :: bots[target].Output) |> List.sort }
    | _ -> ignore()
    match hightarget with
    | Bot(id) ->
        bots[id] <- {bots[id] with Microchips = (bots[botid].Microchips[1] :: bots[id].Microchips) |> List.sort }
    | _ -> ignore()
    match hightarget with
    | Output(target) ->
        bots[target] <- {bots[target] with Output = (bots[botid].Microchips[1] :: bots[target].Output) |> List.sort }
    | _ -> ignore()
    let found = 
        if bots[botid].Microchips[0] = 17 && bots[botid].Microchips[1] = 61 then
            botid
        else
            -1

    bots[botid] <- { bots[botid] with Microchips = [] }
    found


let rec runInstructions (instructions: Instruction list) (bots: Dictionary<int, BotInfo>) (found: int) (index: int) =
    match instructions with
    | [] ->
        found
    | _ ->
        let workingIndex = if index = instructions.Length then 0 else index
        let instruction = instructions[workingIndex]
        match instruction with
        | GiveToBot(botid,value) -> 
            let newbots = executeGiveToBot (botid, value) bots
            runInstructions (instructions |> List.except([instruction])) bots found workingIndex
        | Compare(botid, lowtarget, hightarget) -> 
            let giver = bots.TryGetValue(botid)
            match giver with
            | (true, g) ->
                if g.Microchips.Length = 2 then
                    let newfound = executeCompare (botid, lowtarget, hightarget) bots
                    let newfound' =
                        if newfound <> -1 then
                            newfound
                        else
                            found
                    runInstructions (instructions |> List.except([instruction])) bots newfound' workingIndex
                else
                    runInstructions instructions bots found (workingIndex + 1)
            | (false, _) ->
                bots.Add(botid, {Id = botid; Microchips = []; Output = [] })
                runInstructions instructions  bots found (workingIndex + 1) 


let execute =
    let path = "day10/day10_input.txt"
    let lines = LocalHelper.GetLinesFromFile path
    let instructions = List.ofArray(parseInstructions lines)
    let bots = new Dictionary<int, BotInfo>()
    let result = runInstructions instructions bots -1 0
    List.reduce(*) (bots[0].Output @ bots[1].Output @ bots[2].Output)