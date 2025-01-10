module day14_part01

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

let execute =
    let input = "./day14/day14_input.txt"
    let content = LocalHelper.GetLinesFromFile input

    let reindeers = parseContent content

    reindeers
    |> Array.map(fun r ->
        calculatePosition(r, 2503)
    ) |> Array.max
