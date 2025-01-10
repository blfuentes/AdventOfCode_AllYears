module day14_part02

open System.Collections.Generic
open AdventOfCode_2015.Modules

type Reindeer = {
    Name: string
    Speed: int
    Stamina: int
    Resting: int
}

let parseContent(lines: string array) =
    lines
    |> Array.map(fun l ->
        let who = l.Split(" ")[0]
        let (speed, time, resting) = 
            (
                (int)(l.Split(" ")[3]),
                (int)(l.Split(" ")[6]),
                (int)(l.Split(" ")[13])
            )
        { Name = who; Speed = speed; Stamina = time; Resting = resting }
    )

let calculatePosition ((reindeer, second): Reindeer * int) =
    let cycleTime = reindeer.Stamina + reindeer.Resting
    let fullCycles = second / cycleTime
    let remainingTime = second % cycleTime
    let activeTime = fullCycles * reindeer.Stamina + min remainingTime reindeer.Stamina
    activeTime * reindeer.Speed

let calculateWinningPoints((reindeers, seconds): Reindeer array*int) =
    let winningTable = Dictionary<string, int array>()
    reindeers
    |> Array.iter(fun r ->
        winningTable.Add(r.Name, Array.zeroCreate seconds)
    )
    [1..seconds]
    |> List.iter(fun s ->
        let roundTimes =
            reindeers
            |> Array.map(fun r -> (r.Name, calculatePosition(r, s)))
        let maxTime = roundTimes |> Array.maxBy snd
        let winners = roundTimes |> Array.filter(fun (r, t) -> t = snd maxTime)
        winners
        |> Array.iter(fun (r, _) ->
            winningTable[r][s-1] <- 1
        )
    )
    let points =
        [for kvp in winningTable do
            yield (kvp.Key, kvp.Value |> Array.sum)
        ]
    points
    |> List.map snd
    |> List.max
        
let execute =
    let input = "./day14/day14_input.txt"
    let content = LocalHelper.GetLinesFromFile input

    let reindeers = parseContent content
    calculateWinningPoints(reindeers, 2503)