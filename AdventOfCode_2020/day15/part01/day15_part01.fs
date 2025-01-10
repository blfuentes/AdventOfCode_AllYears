module day15_part01

open System.Collections.Generic
open AdventOfCode_2020.Modules

let AddOrUpdate (d: Dictionary<int,(int option * int)>) (key: int) (value: int) =
    match d.TryGetValue(key) with
    | (true, (old, recent)) ->
        d[key] <- (Some(recent), value)
    | false, _ ->
        d.Add(key, (None, value))

let parseContent(lines: string) =
    lines.Split(",") |> Array.map int

let playturns (bespoken: int array) (numOfTurns: int) =
    let seen = Dictionary<int, (int option * int)>()
    let rec speak (last: int) (turn: int) =
        if turn = numOfTurns then
            last
        else
            if not (seen.ContainsKey(last)) then
                let spoken = 0
                AddOrUpdate seen spoken (turn + 1)
                speak spoken (turn + 1)
            else
                match seen[last] with
                | Some(o), recent ->
                    let spoken = recent - o
                    AddOrUpdate seen spoken (turn + 1)
                    speak spoken (turn + 1)
                | None, recent ->
                    let spoken = 0
                    AddOrUpdate seen spoken (turn + 1)
                    speak spoken (turn + 1)

    bespoken
    |> Array.iteri(fun i v ->
        AddOrUpdate seen v (i + 1)
    )

    speak bespoken[bespoken.Length - 1] bespoken.Length

let execute =
    let path = "day15/day15_input.txt"
    let content = LocalHelper.GetContentFromFile path

    let bespokennumbers = parseContent content
    playturns bespokennumbers 2020